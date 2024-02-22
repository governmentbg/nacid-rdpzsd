using Infrastructure.DomainValidation;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Services.EntityServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.Base
{
    public abstract class BaseSinglePartService<TPart, TPartInfo, THistory, THistoryInfo, TLot>
        where TPart : Part<TPartInfo>, ISinglePart<TPart, TLot, THistory>, new()
        where TPartInfo : PartInfo, new()
        where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
        where THistoryInfo : PartInfo, new()
        where TLot : EntityVersion
    {
        protected readonly RdpzsdDbContext context;
        protected readonly DomainValidatorService domainValidatorService;
        protected readonly UserContext userContext;
        protected readonly PersonLotService personLotService;
        protected readonly BaseHistoryPartService<TPart, TPartInfo, THistory, THistoryInfo, TLot> baseHistoryPartService;

        public BaseSinglePartService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            UserContext userContext,
            PersonLotService personLotService,
            BaseHistoryPartService<TPart, TPartInfo, THistory, THistoryInfo, TLot> baseHistoryPartService
        )
        {
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.userContext = userContext;
            this.personLotService = personLotService;
            this.baseHistoryPartService = baseHistoryPartService;
        }

        public async virtual Task<TPart> Get(int lotId)
        {
            var part = await new TPart().IncludeAll(context.Set<TPart>().AsQueryable())
                        .AsNoTracking()
                        .SingleOrDefaultAsync(e => e.Id == lotId);

            return part;
        }

        public async virtual Task<TPart> Post(TPart part, PersonLotActionType actionType)
        {
            part.ValidateProperties(context, domainValidatorService);

            EntityService.ClearSkipProperties(part);

            part.State = PartState.Actual;
            await context.Set<TPart>().AddAsync(part);

            var createDate = DateTime.Now;

            SetPartInfo(part, createDate);
            await personLotService.AddPersonLotAction(part.Id, createDate, actionType, null);
            await context.SaveChangesAsync();

            return await Get(part.Id);
        }

        public async virtual Task<TPart> Put(TPart updatePart, PersonLotActionType actionType)
        {
            updatePart.ValidateProperties(context, domainValidatorService);

            var actualPart = await new TPart().IncludeAll(context.Set<TPart>().AsQueryable())
                        .Include(e => e.PartInfo)
                        .SingleOrDefaultAsync(e => e.Id == updatePart.Id && e.State == PartState.Actual);

            await baseHistoryPartService.CreateHistory(actualPart, context);

            var updateDate = DateTime.Now;

            SetPartInfo(updatePart, updateDate);
            updatePart.State = PartState.Actual;
            EntityService.Update(actualPart, updatePart, context);
            await personLotService.AddPersonLotAction(updatePart.Id, updateDate, actionType, null);
            await context.SaveChangesAsync();

            return await Get(actualPart.Id);
        }

        public async virtual Task<PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>> GetHistory(int lotId)
        {
            var actual = await new TPart().IncludeAll(context.Set<TPart>().AsNoTracking())
                .Include(e => e.PartInfo.Institution)
                .Include(e => e.PartInfo.Subordinate)
                .SingleAsync(e => e.Id == lotId);

            var histories = await new THistory().IncludeAll(context.Set<THistory>().AsNoTracking())
                .Include(e => e.PartInfo.Institution)
                .Include(e => e.PartInfo.Subordinate)
                .Where(e => e.PartId == lotId)
                .OrderByDescending(e => e.PartInfo.ActionDate)
                .ToListAsync();

            return new PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>(actual, histories);
        }

        private void SetPartInfo(TPart part, DateTime updateDate)
        {
            part.PartInfo = new TPartInfo
            {
                ActionDate = updateDate,
                UserFullname = userContext.UserFullname,
                UserId = userContext.UserId.Value,
                InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null
            };
        }
    }
}
