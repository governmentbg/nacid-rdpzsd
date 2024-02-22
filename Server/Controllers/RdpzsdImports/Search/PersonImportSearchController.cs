using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Search;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdImports.Search
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonImportSearchController : ControllerBase
    {
        private readonly PersonImportSearchService personImportSearchService;
		private readonly ActionWorkflowService<AwPersonImportDto> actionWorkflowService;
		public PersonImportSearchController(
			PersonImportSearchService personImportSearchService,
			ActionWorkflowService<AwPersonImportDto> actionWorkflowService
		)
		{
			this.personImportSearchService = personImportSearchService;
			this.actionWorkflowService = actionWorkflowService;
		}

		[HttpPost]
		public async Task<ActionResult<SearchResultDto<PersonImport>>> GetAll([FromBody] PersonImportFilterDto filter)
		{
			await actionWorkflowService.LogGetAction();
			await actionWorkflowService.LogSearchAction();
			return Ok(await personImportSearchService.GetAll(filter));
		}

		[HttpPost("Count")]
		public async Task<ActionResult<int>> GetCount([FromBody] PersonImportFilterDto filter)
		{
			return Ok(await personImportSearchService.GetCount(filter));
		}
	}
}
