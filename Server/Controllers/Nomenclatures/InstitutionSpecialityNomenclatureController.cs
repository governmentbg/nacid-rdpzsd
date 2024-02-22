

using Infrastructure.User;
using Logs.Dtos.AwNomenclatureDtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.Nomenclatures;
using System.Threading.Tasks;

namespace Server.Controllers.Nomenclatures
{
    [ApiController]
	[Route("api/Nomenclature/InstitutionSpeciality")]
	public class InstitutionSpecialityNomenclatureController : ControllerBase
	{
		protected readonly InstitutionSpecialityNomenclatureService nomenclatureService;
		protected readonly UserContext userContext;
		private readonly ActionWorkflowService<AwInstitutionSpecialityNomenclatureDto> actionWorkflowService;

		public InstitutionSpecialityNomenclatureController(
			InstitutionSpecialityNomenclatureService nomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwInstitutionSpecialityNomenclatureDto> actionWorkflowService
			)
		{
			this.nomenclatureService = nomenclatureService;
			this.userContext = userContext;
			this.actionWorkflowService = actionWorkflowService;
		}

		[HttpPost]
		public async virtual Task<ActionResult<SearchResultDto<InstitutionSpeciality>>> GetAll([FromBody] InstitutionSpecialityFilterDto filter)
		{
			return Ok(await nomenclatureService.GetAll(filter));
		}

		[HttpPost("Excel")]
		public async virtual Task<FileStreamResult> ExportExcel([FromBody] InstitutionSpecialityFilterDto filter)
		{
			await actionWorkflowService.LogExcelAction();
			var excelStream = await nomenclatureService.ExportExcel(filter);

			return new FileStreamResult(excelStream, "application/vnd.ms-excel");
		}
	}
}
