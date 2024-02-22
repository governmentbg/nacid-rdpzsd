using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures.Base
{
	public abstract class BaseNomenclatureService<T, TFilter>
		where T : Nomenclature, IIncludeAll<T>, new()
		where TFilter : NomenclatureFilterDto<T>, new()
	{
		protected readonly RdpzsdDbContext context;
		protected readonly ExcelProcessorService excelProcessorService;
		protected readonly UserContext userContext;

		protected BaseNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
			)
		{
			this.context = context;
			this.excelProcessorService = excelProcessorService;
			this.userContext = userContext;
		}

		public async virtual Task<SearchResultDto<T>> GetAll(TFilter filter)
		{
			if (filter == null)
			{
				filter = new TFilter();
			}

			var query = new T().IncludeAll(context.Set<T>().AsNoTracking());
			query = filter.WhereBuilder(query, userContext, context);
			query = filter.OrderBuilder(query);

			var result = new SearchResultDto<T>
			{
				TotalCount = await query.CountAsync(),
				Result = filter.GetAllData
					? await query.ToListAsync()
					: await query.Skip(filter.Offset).Take(filter.Limit).ToListAsync()

			};

			return result;
		}

		public async virtual Task<T> GetByAlias(string alias)
		{
			var entity = await new T().IncludeAll(context.Set<T>().AsNoTracking())
				.SingleOrDefaultAsync(e => e.Alias == alias);

			return entity;
		}

		public async virtual Task<MemoryStream> ExportExcel(TFilter filter)
		{
			filter.GetAllData = true;
			await GetAll(filter);

			return new MemoryStream();
		}
	}
}
