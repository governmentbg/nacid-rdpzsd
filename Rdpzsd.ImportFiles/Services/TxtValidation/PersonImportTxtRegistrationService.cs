using Infrastructure.Constants;
using Infrastructure.Extensions;
using Infrastructure.Integrations.RdDocumentsIntegration;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary;
using Rdpzsd.Models.Models.RdpzsdImports.Collections;
using Rdpzsd.Services.EntityServices;
using Rdpzsd.Services.Nomenclatures;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Services.TxtValidation
{
    public class PersonImportTxtRegistrationService
    {
        protected readonly RdpzsdDbContext context;
        private readonly NomenclatureDictionariesService nomenclatureDictionariesService;
        private readonly RsoDocumentsIntegrationService rdDocumentsIntegrationService;
        private readonly CountryNomenclatureService countryNomenclatureService;
        private readonly BaseHistoryPartService<PersonBasic, PersonBasicInfo, PersonBasicHistory, PersonBasicHistoryInfo, PersonLot> baseHistoryPartService;
        private HashSet<string> personUanHashSet = new HashSet<string>();
        private Dictionary<int, School> schoolDict = new Dictionary<int, School>();
        private Country country = null;

        public PersonImportTxtRegistrationService(
            RdpzsdDbContext context,
            NomenclatureDictionariesService nomenclatureDictionariesService,
            RsoDocumentsIntegrationService rdDocumentsIntegrationService,
            CountryNomenclatureService countryNomenclatureService,
            BaseHistoryPartService<PersonBasic, PersonBasicInfo, PersonBasicHistory, PersonBasicHistoryInfo, PersonLot> baseHistoryPartService
        )
        {
            this.context = context;
            this.nomenclatureDictionariesService = nomenclatureDictionariesService;
            this.rdDocumentsIntegrationService = rdDocumentsIntegrationService;
            this.countryNomenclatureService = countryNomenclatureService;
            this.baseHistoryPartService = baseHistoryPartService;
        }

        public void LoadDictionaries()
        {
            personUanHashSet = nomenclatureDictionariesService.GetPersonUanHashSetAsync();
            schoolDict = nomenclatureDictionariesService.GetScoolDictByMigrationId();
        }

        public async Task Register(List<PersonLot> personLots, DateTime updateDate, int personImportId)
        {
            LoadDictionaries();

            using var transaction = context.BeginTransaction();

            var personUans = personLots.Where(e => e.Uan != null).Select(e => e.Uan);
            var personUins = personLots.Where(e => e.PersonBasic.Uin != null).Select(e => e.PersonBasic.Uin);
            var personForeignerNumbers = personLots.Where(e => e.PersonBasic.ForeignerNumber != null).Select(e => e.PersonBasic.ForeignerNumber);

            var actualPersonBasics = new PersonBasic().IncludeAll(context.Set<PersonBasic>().AsQueryable())
                .Include(e => e.Lot.PersonSecondary)
                .Include(e => e.Lot.PersonDiplomaCopies)
                .Include(e => e.PartInfo);

            var actualPersonUansDict = actualPersonBasics.Where(e => e.Lot.Uan != null && personUans.Contains(e.Lot.Uan)).ToDictionary(e => e.Lot.Uan, e => e);
            var actualPersonUinsDict = actualPersonBasics.Where(e => e.Uin != null && personUins.Contains(e.Uin)).ToDictionary(e => e.Uin, e => e);
            var actualPersonForeignerNumbersDict = actualPersonBasics.Where(e => e.ForeignerNumber != null && personForeignerNumbers.Contains(e.ForeignerNumber)).ToDictionary(e => e.ForeignerNumber, e => e);

            var personLotsForAdd = new List<PersonLot>();
            var personImportUans = new List<PersonImportUan>();

            foreach (var personLot in personLots)
            {
                if (personLot.Uan != null && actualPersonUansDict.GetValueOrDefault(personLot.Uan) != null)
                {
                    var actualPersonBasic = actualPersonUansDict[personLot.Uan];
                    var personLotAction = personLot.PersonLotActions.First();
                    await AddPersonSecondary(actualPersonBasic.Lot, personLotAction, updateDate, false, actualPersonBasic.Uin ?? actualPersonBasic.ForeignerNumber ?? actualPersonBasic.IdnNumber);
                    await UpdatePersonBasic(actualPersonBasic, personLot.PersonBasic, personLotAction, updateDate, personImportUans, personImportId);
                }
                else if (personLot.PersonBasic.ForeignerNumber != null && actualPersonForeignerNumbersDict.GetValueOrDefault(personLot.PersonBasic.ForeignerNumber) != null)
                {
                    var actualPersonBasic = actualPersonForeignerNumbersDict[personLot.PersonBasic.ForeignerNumber];
                    var personLotAction = personLot.PersonLotActions.First();
                    await AddPersonSecondary(actualPersonBasic.Lot, personLotAction, updateDate, false, actualPersonBasic.Uin ?? actualPersonBasic.ForeignerNumber ?? actualPersonBasic.IdnNumber);
                    await UpdatePersonBasic(actualPersonBasic, personLot.PersonBasic, personLotAction, updateDate, personImportUans, personImportId);
                }
                else if (personLot.PersonBasic.Uin != null && actualPersonUinsDict.GetValueOrDefault(personLot.PersonBasic.Uin) != null)
                {
                    var actualPersonBasic = actualPersonUinsDict[personLot.PersonBasic.Uin];
                    var personLotAction = personLot.PersonLotActions.First();
                    await AddPersonSecondary(actualPersonBasic.Lot, personLotAction, updateDate, false, actualPersonBasic.Uin ?? actualPersonBasic.ForeignerNumber ?? actualPersonBasic.IdnNumber);
                    await UpdatePersonBasic(actualPersonBasic, personLot.PersonBasic, personLotAction, updateDate, personImportUans, personImportId);
                }
                else if (personLot.Uan == null)
                {
                    personLot.GenerateUan(context, personUanHashSet);
                    await AddPersonSecondary(personLot, personLot.PersonLotActions.First(), updateDate, true, personLot.PersonBasic.Uin ?? personLot.PersonBasic.ForeignerNumber ?? personLot.PersonBasic.IdnNumber);
                    personUanHashSet.Add(personLot.Uan);
                    personLotsForAdd.Add(personLot);
                    personImportUans.Add(new PersonImportUan { PersonImportId = personImportId, Uan = personLot.Uan });
                }
            }

            personImportUans = personImportUans.GroupBy(e => e.Uan).Select(e => e.First()).ToList();
            context.PersonLots.AddRange(personLotsForAdd);
            context.PersonImportUans.AddRange(personImportUans);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private async Task UpdatePersonBasic(PersonBasic actualPart, PersonBasic updatePart, PersonLotAction personLotAction, DateTime updateDate, List<PersonImportUan> personImportUans, int personImportId)
        {
            await baseHistoryPartService.CreateHistory(actualPart, context);
            EntityService.Update(actualPart, updatePart, context, true);

            var newPersonLotAction = new PersonLotAction
            {
                ActionDate = updateDate,
                ActionType = PersonLotActionType.PersonBasicEditTxt,
                InstitutionId = personLotAction.InstitutionId,
                LotId = actualPart.Id,
                SubordinateId = personLotAction.SubordinateId,
                UserFullname = personLotAction.UserFullname,
                UserId = personLotAction.UserId
            };
            await context.PersonLotActions.AddAsync(newPersonLotAction);

            if (actualPart.Lot.State == LotState.Erased) {
                actualPart.Lot.State = LotState.Actual;
            }

            personImportUans.Add(new PersonImportUan { PersonImportId = personImportId, Uan = actualPart.Lot.Uan });
        }

        private async Task AddPersonSecondary(PersonLot personLot, PersonLotAction personLotAction, DateTime updateDate, bool isNewLot, string studentUinFn)
        {
            if (personLot.PersonSecondary == null)
            {
                var rdDocumentsDto = await rdDocumentsIntegrationService.GetRsoDocuments(studentUinFn);

                if (rdDocumentsDto != null && rdDocumentsDto.Any())
                {
                    if (!personLot.PersonDiplomaCopies.Any())
                    {
                        var rdSecondaries = rdDocumentsDto
                            .Where(e => e.IntDocID != 107)
                            .ToList()
                            .ToPersonDiplomaCopy(personLot.Id);

                        if (isNewLot)
                        {
                            personLot.PersonDiplomaCopies.AddRange(rdSecondaries);
                        }
                        else
                        {
                            context.PersonDiplomaCopies.AddRange(rdSecondaries);
                        }
                    }

                    var secondaryDocsIds = new List<int> { 31, 32, 35, 60, 62 };

                    var secondaryDocDto = rdDocumentsDto
                           .Where(e => secondaryDocsIds.Contains(e.IntDocID.Value))
                           .OrderByDescending(e => e.DtRegDate)
                           .FirstOrDefault();

                    if (secondaryDocDto != null)
                    {
                        if (secondaryDocDto.IntSchoolID.HasValue)
                        {
                            var secondarySchool = schoolDict.GetDictValueOrNull(secondaryDocDto.IntSchoolID.Value);

                            if (secondarySchool != null)
                            {
                                country = await countryNomenclatureService.GetByCode(CountryConstants.BulgariaCode);
                            }
                            else
                            {
                                country = await countryNomenclatureService.GetByCode(CountryConstants.Missing);
                            }

                            var personSecondary = new PersonSecondary(secondaryDocDto, secondarySchool, country);
                            personSecondary.State = PartState.Actual;
                            personSecondary.PartInfo = new PersonSecondaryInfo
                            {
                                ActionDate = updateDate,
                                UserFullname = personLotAction.UserFullname,
                                UserId = personLotAction.UserId,
                                InstitutionId = personLotAction.InstitutionId,
                                SubordinateId = personLotAction.SubordinateId
                            };
                            personSecondary.Country = null;
                            personSecondary.School = null;

                            if (isNewLot)
                            {
                                personLot.PersonSecondary = personSecondary;
                            }
                            else
                            {
                                personSecondary.Id = personLot.Id;
                                await context.PersonSecondaries.AddAsync(personSecondary);
                            }
                        }
                    }
                }
            }
        }
    }
}
