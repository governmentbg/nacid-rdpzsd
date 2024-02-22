using Logs.Interfaces;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts.Base
{
    public abstract class BaseMultiPartController<TPart, TPartInfo, THistory, THistoryInfo, TLot, TFilter, TService, TActionWorkflowOperation> : ControllerBase
        where TPart : Part<TPartInfo>, IMultiPart<TPart, TLot, THistory>, new()
        where TPartInfo : PartInfo, new()
        where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
        where THistoryInfo : PartInfo, new()
        where TLot : EntityVersion
        where TFilter : FilterDto, IWhere<TPart>, new()
        where TService : BaseMultiPartService<TPart, TPartInfo, THistory, THistoryInfo, TLot, TFilter>
        where TActionWorkflowOperation : IActionWorkflowOperation, new()
    {
        protected readonly TService multiPartService;
        protected readonly PermissionService permissionService;
        protected readonly PersonLotValidationService personLotValidationService;
        protected readonly ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService;

        public BaseMultiPartController(
            TService multiPartService,
            PermissionService permissionService,
            PersonLotValidationService personLotValidationService,
            ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService
            )
        {
            this.multiPartService = multiPartService;
            this.permissionService = permissionService;
            this.personLotValidationService = personLotValidationService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpGet]
        [Route("{lotId:int}")]
        public async virtual Task<ActionResult<List<TPart>>> GetAll([FromRoute] int lotId)
        {
            await actionWorkflowService.LogGetAction($"номер на партида - ${lotId}");
            return Ok(await multiPartService.GetAll(lotId));
        }

        [HttpGet]
        [Route("{id:int}/History")]
        public async virtual Task<ActionResult<PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>>> GetHistory([FromRoute] int id)
        {
            return Ok(await multiPartService.GetHistory(id));
        }

        [HttpPost]
        [Route("Create")]
        public async virtual Task<ActionResult<TPart>> Post([FromBody] TPart part)
        {
            await actionWorkflowService.LogCreateAction($"номер на партида - {part.Id}");
            permissionService.VerifyEditPermissions();
            return Ok(await multiPartService.Post(part, 0, null));
        }

        [HttpPut]
        [Route("Update")]
        public async virtual Task<ActionResult<TPart>> Put([FromBody] TPart part)
        {
            await actionWorkflowService.LogUpdateAction($"номер на партида - {part.Id}");
            permissionService.VerifyEditPermissions();
            var actualPart = await multiPartService.Get(part.Id);
            return Ok(await multiPartService.Put(actualPart, part, 0, null));
        }

        [HttpDelete]
        [Route("Erase/{partId:int}")]
        public async virtual Task<ActionResult<TPart>> Erase([FromRoute] int partId)
        {
            await actionWorkflowService.LogDeleteAction($"номер на партида - {partId}");
            permissionService.VerifyEditPermissions();
            var actualPart = await multiPartService.Get(partId);
            return Ok(await multiPartService.Erase(actualPart, 0, null));
        }

        [HttpPost]
        [Route("SearchDto")]
        public async Task<ActionResult<SearchResultDto<TPart>>> GetSearchDto([FromBody] TFilter filter)
        {
            return Ok(await multiPartService.GetSearchDto(filter));
        }
    }
}
