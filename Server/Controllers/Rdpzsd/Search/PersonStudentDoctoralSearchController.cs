using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentDoctoral;
using Rdpzsd.Services.Rdpzsd.Search;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Search
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonStudentSearchController : ControllerBase
    {
        private readonly PersonStudentSearchService personStudentSearchService;
		private readonly ActionWorkflowService<AwPersonStudentSearchDto> actionWorkflowService;

		public PersonStudentSearchController(
			PersonStudentSearchService personStudentSearchService,
			ActionWorkflowService<AwPersonStudentSearchDto> actionWorkflowService
		)
		{
			this.personStudentSearchService = personStudentSearchService;
			this.actionWorkflowService = actionWorkflowService;
		}

		[HttpPost]
		public async Task<ActionResult<SearchResultDto<PersonStudentDoctoralSearchDto>>> GetAll([FromBody] PersonStudentSearchFilterDto filter)
		{
			await actionWorkflowService.LogSearchAction();
			return Ok(await personStudentSearchService.GetAll(filter));
		}

		[HttpPost("Count")]
		public async Task<ActionResult<int>> GetCount([FromBody] PersonStudentSearchFilterDto filter)
		{
			return Ok(await personStudentSearchService.GetCount(filter));
		}
	}

	[ApiController]
	[Route("api/[controller]")]
	public class PersonDoctoralSearchController : ControllerBase
	{
		private readonly PersonDoctoralSearchService personDoctoralSearchService;
		private readonly ActionWorkflowService<AwPersonDoctoralSearchDto> actionWorkflowService;

		public PersonDoctoralSearchController(
			PersonDoctoralSearchService personDoctoralSearchService,
			ActionWorkflowService<AwPersonDoctoralSearchDto> actionWorkflowService
		)
		{
			this.personDoctoralSearchService = personDoctoralSearchService;
			this.actionWorkflowService = actionWorkflowService;
		}

		[HttpPost]
		public async Task<ActionResult<SearchResultDto<PersonStudentDoctoralSearchDto>>> GetAll([FromBody] PersonDoctoralSearchFilterDto filter)
		{
			await actionWorkflowService.LogSearchAction();
			return Ok(await personDoctoralSearchService.GetAll(filter));
		}

		[HttpPost("Count")]
		public async Task<ActionResult<int>> GetCount([FromBody] PersonDoctoralSearchFilterDto filter)
		{
			return Ok(await personDoctoralSearchService.GetCount(filter));
		}
	}
}
