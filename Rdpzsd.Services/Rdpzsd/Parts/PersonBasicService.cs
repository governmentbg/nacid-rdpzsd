using Infrastructure.DomainValidation;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Services.EntityServices;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts
{
    public class PersonBasicService : BaseSinglePartService<PersonBasic, PersonBasicInfo, PersonBasicHistory, PersonBasicHistoryInfo, PersonLot>
    {
        private readonly ImageFileService imageFileService;

        public PersonBasicService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            UserContext userContext,
            PersonLotService personLotService,
            ImageFileService imageFileService,
            BaseHistoryPartService<PersonBasic, PersonBasicInfo, PersonBasicHistory, PersonBasicHistoryInfo, PersonLot> baseHistoryPartService
        ) : base(context, domainValidatorService, userContext, personLotService, baseHistoryPartService)
        {
            this.imageFileService = imageFileService;
        }

        public override async Task<PersonBasic> Put(PersonBasic updatePart, PersonLotActionType actionType)
        {
            var personLot = await context.PersonLots.SingleAsync(e => e.Id == updatePart.Id);

            if (personLot.State == LotState.Erased)
            {
                personLot.State = LotState.Actual;
            }

            updatePart.ValidateProperties(context, domainValidatorService);

            var actualPart = await new PersonBasic().IncludeAll(context.Set<PersonBasic>().AsQueryable())
                        .Include(e => e.PartInfo)
                        .SingleOrDefaultAsync(e => e.Id == updatePart.Id && e.State == PartState.Actual);

            await baseHistoryPartService.CreateHistory(actualPart, context);

            var updateDate = DateTime.Now;

            updatePart.PartInfo = new PersonBasicInfo
            {
                ActionDate = updateDate,
                UserFullname = userContext.UserFullname,
                UserId = userContext.UserId.Value,
                InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null
            };

            if (updatePart.PersonImage != null)
            {
                if (actualPart.PersonImage != null)
                {
                    await personLotService.AddPersonLotAction(updatePart.Id, DateTime.Now, PersonLotActionType.PersonImageEdit, null);
                }
                else
                {
                    await personLotService.AddPersonLotAction(updatePart.Id, DateTime.Now, PersonLotActionType.PersonImageAdd, null);
                }
            }

            updatePart.State = PartState.Actual;
            EntityService.Update(actualPart, updatePart, context);
            await personLotService.AddPersonLotAction(updatePart.Id, updateDate, actionType, null);

            await context.SaveChangesAsync();

            return await Get(actualPart.Id);
        }

        public async Task<List<string>> GetHistoryImages(int lotId)
        {
            var personBasic = await context.PersonBasics
                .AsNoTracking()
                .Include(e => e.PersonImage)
                .Include(e => e.Histories)
                    .ThenInclude(s => s.PersonImage)
                .SingleOrDefaultAsync(e => e.Id == lotId && e.PersonImage != null);

            var imagesBase64 = new List<string>();

            if (personBasic != null)
            {
                imagesBase64.Add(await imageFileService.GetBase64ImageUrlAsync(personBasic.PersonImage.Key, personBasic.PersonImage.DbId));
                var addedHashes = new List<string> { personBasic.PersonImage.Hash };

                foreach (var historyPersonBasic in personBasic.Histories)
                {
                    if (historyPersonBasic.PersonImage != null && !addedHashes.Contains(historyPersonBasic.PersonImage.Hash))
                    {
                        imagesBase64.Add(await imageFileService.GetBase64ImageUrlAsync(historyPersonBasic.PersonImage.Key, historyPersonBasic.PersonImage.DbId));
                        addedHashes.Add(historyPersonBasic.PersonImage.Hash);
                    }
                }
            }

            return imagesBase64;
        }
    }
}
