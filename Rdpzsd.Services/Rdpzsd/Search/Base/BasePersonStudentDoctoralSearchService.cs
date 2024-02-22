using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentDoctoral;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Search.Base
{
    public class BasePersonStudentDoctoralSearchService<TPart, TPartInfo, TSemester, THistory, TFilter>
        where TPart : BasePersonStudentDoctoral<TPartInfo, TSemester>, IMultiPart<TPart, PersonLot, THistory>
        where TPartInfo : PartInfo
        where TSemester : BasePersonSemester, new()
        where THistory : EntityVersion
        where TFilter : BasePersonStudentDoctoralFilterDto<TPart, TPartInfo, TSemester, THistory>, new()
    {
        private readonly RdpzsdDbContext context;
        private readonly UserContext userContext;
        private readonly ExcelProcessorService excelProcessorService;

        public BasePersonStudentDoctoralSearchService(
           RdpzsdDbContext context,
           UserContext userContext,
           ExcelProcessorService excelProcessorService
        )
        {
            this.context = context;
            this.userContext = userContext;
            this.excelProcessorService = excelProcessorService;
        }

        protected virtual IQueryable<int> GetInitialLotIdQuery(TFilter filter)
        {
            if (filter == null)
            {
                filter = new TFilter();
            }

            var query = context.Set<TPart>()
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .AsQueryable();

            var queryIds = filter
                .WhereBuilder(query, userContext, context)
                .Select(e => e.LotId)
                .Distinct();

            return queryIds;
        }

        public async virtual Task<SearchResultDto<PersonStudentDoctoralSearchDto>> GetAll(TFilter filter)
        {
            var queryLotIds = GetInitialLotIdQuery(filter);

            var loadIds = await queryLotIds
                .Take(filter.Limit)
                .ToListAsync();

            var result = context.PersonLots
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Where(e => loadIds.Contains(e.Id));

            result = ConstructIncludes(result);

            var searchResult = new SearchResultDto<PersonStudentDoctoralSearchDto>
            {
                Result = await result.ToPersonStudentDoctoralSearchDto().ToListAsync()
            };

            return searchResult;
        }

        public async virtual Task<int> GetCount(TFilter filter)
        {
            var queryIds = GetInitialLotIdQuery(filter);

            return await queryIds.CountAsync();
        }

        protected virtual IQueryable<PersonLot> ConstructIncludes(IQueryable<PersonLot> query)
        {
            return query;
        }
    }
}
