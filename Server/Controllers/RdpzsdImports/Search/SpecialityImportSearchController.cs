using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Services.Search;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdImports.Search
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialityImportSearchController : ControllerBase
    {
        private readonly SpecialityImportSearchService specialityImportSearchService;
        private readonly ActionWorkflowService<AwSpecialityImportDto> actionWorkflowService;
        public SpecialityImportSearchController(
            SpecialityImportSearchService specialityImportSearchService,
            ActionWorkflowService<AwSpecialityImportDto> actionWorkflowService
        )
        {
            this.specialityImportSearchService = specialityImportSearchService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<SearchResultDto<SpecialityImport>>> GetAll([FromBody] SpecialityImportFilterDto filter)
        {
            await actionWorkflowService.LogGetAction();
            return Ok(await specialityImportSearchService.GetAll(filter));
        }

        [HttpPost("Count")]
        public async Task<ActionResult<int>> GetCount([FromBody] SpecialityImportFilterDto filter)
        {
            return Ok(await specialityImportSearchService.GetCount(filter));
        }
    }
}
