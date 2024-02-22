using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Search.Base
{
    public abstract class BaseRdpzsdImportSearchService<TRdpzsdImport, TFilter>
        where TRdpzsdImport : EntityVersion, IIncludeAll<TRdpzsdImport>, new()
        where TFilter : FilterDto, IWhere<TRdpzsdImport>, new()
    {
        protected readonly RdpzsdDbContext context;
        protected readonly UserContext userContext;

        public BaseRdpzsdImportSearchService(
           RdpzsdDbContext context,
           UserContext userContext
        )
        {
            this.context = context;
            this.userContext = userContext;
        }

        protected virtual IQueryable<int> GetInitialQuery(TFilter filter)
        {
            if (filter == null)
            {
                filter = new TFilter();
            }

            var query = context.Set<TRdpzsdImport>()
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .AsQueryable();

            var queryIds = filter.WhereBuilder(query, userContext, context).Select(e => e.Id);

            return queryIds;
        }

        public async virtual Task<SearchResultDto<TRdpzsdImport>> GetAll(TFilter filter)
        {
            var queryIds = GetInitialQuery(filter);

            var loadIds = await queryIds
                .Take(filter.Limit)
                .ToListAsync();

            var result = await new TRdpzsdImport().IncludeAll(context.Set<TRdpzsdImport>().AsNoTracking())
                .OrderByDescending(e => e.Id)
                .Where(e => loadIds.Contains(e.Id))
                .ToListAsync();

            var searchResult = new SearchResultDto<TRdpzsdImport>
            {
                Result = result
            };

            return searchResult;
        }

        public async virtual Task<int> GetCount(TFilter filter)
        {
            var queryIds = GetInitialQuery(filter);

            return await queryIds.CountAsync();
        }
    }
}
