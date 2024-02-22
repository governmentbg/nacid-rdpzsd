using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Integrations.RdDocumentsIntegration;
using Infrastructure.User;
using Infrastructure.User.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History;
using Rdpzsd.Services.EntityServices;
using Rdpzsd.Services.Nomenclatures;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts
{
    public class PersonSecondaryService : BaseSinglePartService<PersonSecondary, PersonSecondaryInfo, PersonSecondaryHistory, PersonSecondaryHistoryInfo, PersonLot>
    {
        private readonly RsoDocumentsIntegrationService rdDocumentsIntegrationService;
        private readonly SchoolNomenclatureService schoolNomenclatureService;
        private readonly CountryNomenclatureService countryNomenclatureService;

        public PersonSecondaryService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            UserContext userContext,
            PersonLotService personLotService,
            RsoDocumentsIntegrationService rdDocumentsIntegrationService,
            SchoolNomenclatureService schoolNomenclatureService,
            CountryNomenclatureService countryNomenclatureService,
            BaseHistoryPartService<PersonSecondary, PersonSecondaryInfo, PersonSecondaryHistory, PersonSecondaryHistoryInfo, PersonLot> baseHistoryPartService
        ) : base(context, domainValidatorService, userContext, personLotService, baseHistoryPartService)
        {
            this.rdDocumentsIntegrationService = rdDocumentsIntegrationService;
            this.schoolNomenclatureService = schoolNomenclatureService;
            this.countryNomenclatureService = countryNomenclatureService;
        }

        public override async Task<PersonSecondary> Put(PersonSecondary updatePart, PersonLotActionType actionType)
        {
            updatePart.ValidateProperties(context, domainValidatorService);

            var actualPart = await context.PersonSecondaries
                        .Include(e => e.PartInfo)
                        .Include(e => e.PersonSecondaryRecognitionDocument)
                        .SingleOrDefaultAsync(e => e.Id == updatePart.Id && e.State == PartState.Actual);

            if (actualPart.FromRso)
            {
                actualPart.CountryId = updatePart.CountryId;
                actualPart.SchoolId = updatePart.SchoolId;
                actualPart.GraduationYear = updatePart.GraduationYear;
                actualPart.DiplomaDate = updatePart.DiplomaDate;
                actualPart.DiplomaNumber = updatePart.DiplomaNumber;
                actualPart.ForeignSchoolName = updatePart.ForeignSchoolName;
                actualPart.Profession = updatePart.Profession;

                if (context.ChangeTracker.HasChanges())
                {
                    updatePart.FromRso = false;
                }
            }

            var partHistory = new PersonSecondaryHistory();
            EntityService.CloneProperties(actualPart, partHistory);
            partHistory.PartId = actualPart.Id;

            await context.PersonSecondaryHistories.AddAsync(partHistory);

            var updateDate = DateTime.Now;

            updatePart.PartInfo = new PersonSecondaryInfo
            {
                ActionDate = updateDate,
                UserFullname = userContext.UserFullname,
                UserId = userContext.UserId.Value,
                InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                   ? userContext.Institution.ChildInstitutions.First().Id
                   : null
            };

            updatePart.State = PartState.Actual;
            EntityService.Update(actualPart, updatePart, context);
            await personLotService.AddPersonLotAction(updatePart.Id, updateDate, actionType, null);
            await context.SaveChangesAsync();

            return await Get(actualPart.Id);
        }

        public async Task<PersonSecondary> GetFromRso(int lotId)
        {
            var personlot = await context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic)
                .Include(e => e.PersonDiplomaCopies)
                .SingleAsync(e => e.Id == lotId);

            var rdDocumentsDto = await rdDocumentsIntegrationService.GetRsoDocuments(personlot.PersonBasic.Uin ?? personlot.PersonBasic.ForeignerNumber ?? personlot.PersonBasic.IdnNumber);

            if (rdDocumentsDto != null)
            {
                var rdSecondaries = rdDocumentsDto
                    .Where(e => e.IntDocID != 107)
                    .ToList()
                    .ToPersonDiplomaCopy(lotId);

                if (!personlot.PersonDiplomaCopies.Any())
                {
                    context.PersonDiplomaCopies.AddRange(rdSecondaries);
                }
                else
                {
                    foreach (var rdSecondary in rdSecondaries)
                    {
                        var originalItem = personlot
                            .PersonDiplomaCopies
                            .SingleOrDefault(e => e.IntDocID == rdSecondary.IntDocID && e.DtRegDate == rdSecondary.DtRegDate);

                        rdSecondary.Id = originalItem.Id;
                        originalItem = rdSecondary;

                        context.Update(originalItem);
                    }
                }

                await context.SaveChangesAsync();

                var secondaryDocsIds = new List<int> { 31, 32, 35, 60, 62 };

                var secondaryDocDto = rdDocumentsDto
                    .Where(e => secondaryDocsIds.Contains(e.IntDocID.Value))
                    .OrderByDescending(e => e.DtRegDate)
                    .FirstOrDefault();

                if (secondaryDocDto != null)
                {
                    if (secondaryDocDto.IntSchoolID.HasValue)
                    {
                        var secondarySchool = await schoolNomenclatureService.GetSchoolByMigrationId(secondaryDocDto.IntSchoolID.Value);
                        Country country = null;

                        if (secondarySchool != null)
                        {
                            country = await countryNomenclatureService.GetByCode(CountryConstants.BulgariaCode);
                        }
                        else
                        {
                            country = await countryNomenclatureService.GetByCode(CountryConstants.Missing);
                        }

                        var personSecondary = new PersonSecondary(secondaryDocDto, secondarySchool, country);

                        return personSecondary;
                    }
                    else
                    {
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.School_SchoolByMigrationIdNotFound);
                        return null;
                    }
                }
                else
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_NotFoundSecondaryByUin, personlot.PersonBasic.Uin ?? personlot.PersonBasic.ForeignerNumber ?? personlot.PersonBasic.IdnNumber);
                    return null;
                }
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_UnableToConnectToRdDocuments);
                return null;
            }
        }

        public async Task<MemoryStream> GetImagesFromRso(double? intId)
        {
            var rdImagesDto = await rdDocumentsIntegrationService.GetRsoDocumentsImages(intId);

            if (rdImagesDto != null)
            {
                var pdfDocument = new Document(PageSize.A4, 5f, 5f, 5f, 5f);
                var memoryStream = new MemoryStream();

                PdfWriter writer = PdfWriter.GetInstance(pdfDocument, memoryStream);
                writer.CloseStream = false;

                pdfDocument.Open();

                foreach (var rdImage in rdImagesDto)
                {
                    byte[] rdImageByteArray = Convert.FromBase64String(rdImage.ImgData);
                    AddImageToDocument(pdfDocument, rdImageByteArray);

                    pdfDocument.NewPage();
                }

                pdfDocument.Close();
                memoryStream.Position = 0;

                return memoryStream;
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_UnableToConnectToRdDocuments);
                return null;
            }
        }

        private static void AddImageToDocument(Document pdfDocument, byte[] imageArray)
        {
            var image = Image.GetInstance(imageArray);

            if (image.Width > image.Height)
            {
                image.RotationDegrees = 90f;
                image.ScaleAbsoluteWidth(pdfDocument.PageSize.Height - 10);
                image.ScaleAbsoluteHeight(pdfDocument.PageSize.Width - 10);
            }
            else
            {
                image.ScaleAbsoluteWidth(pdfDocument.PageSize.Width - 10);
                image.ScaleAbsoluteHeight(pdfDocument.PageSize.Height - 10);
            }

            pdfDocument.Add(image);
        }
    }
}
