using Infrastructure.User;
using Logs.Interfaces;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using Rdpzsd.Services.Nomenclatures.Base;
using System.Threading.Tasks;

namespace Server.Controllers.Nomenclatures.Base
{
	public abstract class BaseNomenclatureCodeController<T, TFilter, TService, TActionWorkflowOperation> : BaseNomenclatureController<T, TFilter, TService, TActionWorkflowOperation>
		where T : NomenclatureCode, IIncludeAll<T>, new()
		where TFilter : NomenclatureCodeFilterDto<T>, new()
		where TService : BaseNomenclatureCodeService<T, TFilter>
		where TActionWorkflowOperation : IActionWorkflowOperation, new()
	{
		public BaseNomenclatureCodeController(
			TService nomenclatureService,
			UserContext userContext,
			ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService
			) : base(nomenclatureService, userContext, actionWorkflowService)
		{
		}

		[HttpGet("Code")]
		public async virtual Task<ActionResult<T>> GetByCode(string code)
		{
			return await nomenclatureService.GetByCode(code);
		}
	}
}
