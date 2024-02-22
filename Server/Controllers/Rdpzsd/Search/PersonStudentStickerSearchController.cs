using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentSticker;
using Rdpzsd.Services.Rdpzsd.Search;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Search
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonStudentStickerSearchController : ControllerBase
    {
        private readonly PersonStudentStickerSearchService personStudentStickerSearchService;
        private readonly ActionWorkflowService<AwPersonStudentStickerSearchDto> actionWorkflowService;

        public PersonStudentStickerSearchController(
            PersonStudentStickerSearchService personStudentStickerSearchService,
            ActionWorkflowService<AwPersonStudentStickerSearchDto> actionWorkflowService
        )
        {
            this.personStudentStickerSearchService = personStudentStickerSearchService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<SearchResultDto<PersonStudentStickerSearchDto>>> GetAll([FromBody] PersonStudentStickerFilterDto filter)
        {
            await actionWorkflowService.LogSearchAction();
            return Ok(await personStudentStickerSearchService.GetAll(filter));
        }

        [HttpPost("Count")]
        public async Task<ActionResult<int>> GetCount([FromBody] PersonStudentStickerFilterDto filter)
        {
            return Ok(await personStudentStickerSearchService.GetCount(filter));
        }
    }
}
