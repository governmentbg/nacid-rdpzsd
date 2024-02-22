using Infrastructure.Constants;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Search;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Search
{
    [ApiController]
	[Route("api/[controller]")]
	public class PersonLotSearchController : ControllerBase
	{
		private readonly PersonLotNewSearchService personLotNewSearchService;
		private readonly PersonUanExportSearchService personUanExportSearchService;
		private readonly PersonSearchApprovalService personSearchApprovalService;
		private readonly PermissionService permissionService;
		private readonly ActionWorkflowService<AwPersonLotSearchDto> actionWorkflowService;

		public PersonLotSearchController(
			PersonLotNewSearchService personLotNewSearchService,
			PersonUanExportSearchService personUanExportSearchService,
			PersonSearchApprovalService personSearchApprovalService,
			PermissionService permissionService,
			ActionWorkflowService<AwPersonLotSearchDto> actionWorkflowService
		)
		{
			this.personLotNewSearchService = personLotNewSearchService;
			this.personUanExportSearchService = personUanExportSearchService;
			this.personSearchApprovalService = personSearchApprovalService;
			this.permissionService = permissionService;
			this.actionWorkflowService = actionWorkflowService;
		}

		[HttpPost("New")]
		public async Task<ActionResult<SearchResultDto<PersonSearchDto>>> GetAllNew([FromBody] PersonLotNewFilterDto filter)
		{
			await this.actionWorkflowService.LogSearchAction(": всички");
			return Ok(await personLotNewSearchService.GetAll(filter));
		}

		[HttpPost("NewCount")]
		public async Task<ActionResult<int>> GetNewCount([FromBody] PersonLotNewFilterDto filter)
		{
			return Ok(await personLotNewSearchService.GetCount(filter));
		}

		[HttpPost("Approval")]
		public async Task<ActionResult<SearchResultDto<PersonApprovalSearchDto>>> GetAllForApproval([FromBody] PersonApprovalFilterDto filter)
		{
			await this.actionWorkflowService.LogSearchAction(": за одобрение");
			permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
			return Ok(await personSearchApprovalService.GetAll(filter));
		}

		[HttpPost("ApprovalCount")]
		public async Task<ActionResult<int>> GetAllForApprovalCount([FromBody] PersonApprovalFilterDto filter)
		{
			permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
			return Ok(await personSearchApprovalService.GetCount(filter));
		}

		[HttpPost("Excel")]
		public async Task<FileStreamResult> ExportExcel([FromBody] PersonLotNewFilterDto filter)
		{
			await actionWorkflowService.LogCustomAction("Експортиране на физически лица в ексел");
			var excelStream = await personLotNewSearchService.ExportExcel(filter);

			return new FileStreamResult(excelStream, "application/vnd.ms-excel");
		}

		[HttpPost("Uan")]
		public async Task<ActionResult<SearchResultDto<PersonUanExportDto>>> GetAllUanExport([FromBody] PersonUanExportFilterDto filter)
		{
			await actionWorkflowService.LogCustomAction("Експортиране на ЕАН");
			return Ok(await personUanExportSearchService.GetAllUanExport(filter));
		}

		[HttpPost("UanCount")]
		public async Task<ActionResult<int>> GetAllUanExportCount([FromBody] PersonUanExportFilterDto filter)
		{
			return Ok(await personUanExportSearchService.GetCount(filter));
		}

		[HttpPost("ExcelUan")]
		public async Task<FileStreamResult> ExportExcelUan([FromBody] PersonUanExportFilterDto filter)
		{
			await actionWorkflowService.LogCustomAction("Експортиране на ЕАН в ексел");
			var excelStream = await personUanExportSearchService.ExportExcel(filter);

			return new FileStreamResult(excelStream, "application/vnd.ms-excel");
		}
	}
}
