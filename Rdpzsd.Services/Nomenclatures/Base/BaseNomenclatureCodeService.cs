using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures.Base
{
    public abstract class BaseNomenclatureCodeService<T, TFilter> : BaseNomenclatureService<T, TFilter>
        where T : NomenclatureCode, IIncludeAll<T>, new()
        where TFilter : NomenclatureCodeFilterDto<T>, new()
    {

		protected BaseNomenclatureCodeService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
			) : base(context, excelProcessorService, userContext)
		{
		}

		public async virtual Task<T> GetByCode(string code)
		{
			var entity = await new T().IncludeAll(context.Set<T>().AsNoTracking())
				.SingleOrDefaultAsync(e => e.Code == code);

			return entity;
		}
	}
}
