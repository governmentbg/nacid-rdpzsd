using Infrastructure.User;
using Logs.Interfaces;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using Rdpzsd.Services.Nomenclatures.Base;
using System.Threading.Tasks;

namespace Server.Controllers.Nomenclatures.Base
{
	public abstract class BaseNomenclatureController<T, TFilter, TService, TActionWorkflowOperation> : ControllerBase
		where T : Nomenclature, IIncludeAll<T>, new()
		where TFilter : NomenclatureFilterDto<T>, new()
		where TService : BaseNomenclatureService<T, TFilter>
		where TActionWorkflowOperation: IActionWorkflowOperation, new()
	{
		protected readonly TService nomenclatureService;
		protected readonly UserContext userContext;
		private readonly ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService;

		public BaseNomenclatureController(
			TService nomenclatureService,
			UserContext userContext,
			ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService
			)
		{
			this.nomenclatureService = nomenclatureService;
			this.userContext = userContext;
			this.actionWorkflowService = actionWorkflowService;
		}

		[HttpPost]
		public async virtual Task<ActionResult<SearchResultDto<T>>> GetAll([FromBody] TFilter filter)
		{
			return Ok(await nomenclatureService.GetAll(filter));
		}

		[HttpGet("Alias")]
		public async virtual Task<ActionResult<T>> GetByAlias(string alias)
		{
			return await nomenclatureService.GetByAlias(alias);
		}

		[HttpPost("Excel")]
		public async virtual Task<FileStreamResult> ExportExcel([FromBody] TFilter filter)
		{
			await actionWorkflowService.LogExcelAction();
			var excelStream = await nomenclatureService.ExportExcel(filter);

			return new FileStreamResult(excelStream, "application/vnd.ms-excel");
		}
	}
}
