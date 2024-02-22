using Infrastructure.DomainValidation;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Services.EntityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.Base
{
    public abstract class BaseMultiPartService<TPart, TPartInfo, THistory, THistoryInfo, TLot, TFilter>
        where TPart : Part<TPartInfo>, IMultiPart<TPart, TLot, THistory>, new()
        where TPartInfo : PartInfo, new()
        where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
        where THistoryInfo : PartInfo, new()
        where TLot : EntityVersion
        where TFilter : FilterDto, IWhere<TPart>, new()
    {
        protected readonly RdpzsdDbContext context;
        protected readonly DomainValidatorService domainValidatorService;
        protected readonly UserContext userContext;
        protected readonly PersonLotService personLotService;
        protected readonly BaseHistoryPartService<TPart, TPartInfo, THistory, THistoryInfo, TLot> baseHistoryPartService;

        public BaseMultiPartService(
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

        public async virtual Task<List<TPart>> GetAll(int lotId)
        {
            var parts = await new TPart().IncludeAll(context.Set<TPart>().AsQueryable())
                        .AsNoTracking()
                        .Where(e => e.LotId == lotId && e.State != PartState.Erased)
                        .OrderByDescending(e => e.Id)
                        .ToListAsync();

            return parts;
        }

        public async virtual Task<TPart> Get(int id)
        {
            var part = await new TPart().IncludeAll(context.Set<TPart>().AsQueryable())
                        .AsNoTracking()
                        .Include(e => e.PartInfo)
                        .SingleOrDefaultAsync(e => e.Id == id);

            return part;
        }

        public async virtual Task<TPart> Post(TPart part, PersonLotActionType actionType, string actionNote)
        {
            part.ValidateProperties(context, domainValidatorService);

            EntityService.ClearSkipProperties(part);

            part.State = PartState.Actual;
            await context.Set<TPart>().AddAsync(part);

            var createDate = DateTime.Now;

            SetPartInfo(part, createDate);
            await personLotService.AddPersonLotAction(part.LotId, createDate, actionType, actionNote);
            await context.SaveChangesAsync();

            return await Get(part.Id);
        }

        public async virtual Task<TPart> Put(TPart actualPart, TPart updatePart, PersonLotActionType actionType, string actionNote)
        {
            updatePart.ValidateProperties(context, domainValidatorService);

            await CreateHistory(actualPart);

            var updateDate = DateTime.Now;
            SetPartInfo(updatePart, updateDate);

            EntityService.ClearSkipProperties(actualPart);
            context.Set<TPart>().Attach(actualPart);

            updatePart.State = PartState.Actual;
            EntityService.Update(actualPart, updatePart, context);
            await personLotService.AddPersonLotAction(actualPart.LotId, updateDate, actionType, actionNote);
            await context.SaveChangesAsync();

            return await Get(actualPart.Id);
        }

        public async virtual Task<TPart> Erase(TPart actualPart, PersonLotActionType actionType, string actionNote)
        {
            await CreateHistory(actualPart);

            var updateDate = DateTime.Now;
            UpdatePartInfo(actualPart, updateDate);

            EntityService.ClearSkipProperties(actualPart);
            context.Set<TPart>().Attach(actualPart);
            actualPart.State = PartState.Erased;

            await personLotService.AddPersonLotAction(actualPart.LotId, updateDate, actionType, actionNote);
            await context.SaveChangesAsync();

            return null;
        }

        public async Task<SearchResultDto<TPart>> GetSearchDto(TFilter filter)
        {
            if (filter == null)
            {
                filter = new TFilter();
            }

            var query = context.Set<TPart>()
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .AsQueryable();

            var queryIds = filter.WhereBuilder(query, userContext, context).Select(e => e.Id);

            var loadIds = await queryIds
                .Take(filter.Limit)
                .ToListAsync();

            var result = await new TPart().IncludeAll(context.Set<TPart>().AsQueryable())
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Where(e => loadIds.Contains(e.Id))
                .ToListAsync();

            var searchResult = new SearchResultDto<TPart>
            {
                TotalCount = await queryIds.CountAsync(),
                Result = result
            };

            return searchResult;
        }

        public async virtual Task<PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>> GetHistory(int id)
        {
            var actual = await new TPart().IncludeAll(context.Set<TPart>().AsNoTracking())
                .Include(e => e.PartInfo.Institution)
                .Include(e => e.PartInfo.Subordinate)
                .SingleAsync(e => e.Id == id);

            var histories = await new THistory().IncludeAll(context.Set<THistory>().AsNoTracking())
                .Include(e => e.PartInfo.Institution)
                .Include(e => e.PartInfo.Subordinate)
                .Where(e => e.PartId == id)
                .OrderByDescending(e => e.PartInfo.ActionDate)
                .ToListAsync();

            return new PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>(actual, histories);
        }

        public async Task CreateHistory(TPart actualPart)
        {
            await baseHistoryPartService.CreateHistory(actualPart, context);
        }

        public void SetPartInfo(TPart part, DateTime updateDate)
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

        public void UpdatePartInfo(TPart part, DateTime updateDate)
        {
            context.Entry(part.PartInfo).State = EntityState.Modified;
            part.PartInfo.ActionDate = updateDate;
            part.PartInfo.UserFullname = userContext.UserFullname;
            part.PartInfo.UserId = userContext.UserId.Value;
            part.PartInfo.InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null;
            part.PartInfo.SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null;
        }
    }
}
