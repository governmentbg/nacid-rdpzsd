using Logs.Interfaces;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts
{
    public abstract class BaseSinglePartController<TPart, TPartInfo, THistory, THistoryInfo, TLot, TService, TActionWorkflowOperation> : ControllerBase
        where TPart : Part<TPartInfo>, ISinglePart<TPart, TLot, THistory>, new()
        where TPartInfo : PartInfo, new()
        where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
        where THistoryInfo: PartInfo, new()
        where TLot : EntityVersion
        where TService : BaseSinglePartService<TPart, TPartInfo, THistory, THistoryInfo, TLot>
        where TActionWorkflowOperation : IActionWorkflowOperation, new()
    {
		protected readonly TService singlePartService;
        protected readonly PermissionService permissionService;
        protected readonly PersonLotValidationService personLotValidationService;
        protected readonly ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService;

        public BaseSinglePartController(
			TService singlePartService,
            PermissionService permissionService,
            PersonLotValidationService personLotValidationService,
            ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService
            )
		{
            this.singlePartService = singlePartService;
            this.permissionService = permissionService;
            this.personLotValidationService = personLotValidationService;
            this.actionWorkflowService = actionWorkflowService;
        }

		[HttpGet]
        [Route("{lotId:int}")]
        public async virtual Task<ActionResult<TPart>> Get([FromRoute] int lotId)
        {
            await actionWorkflowService.LogGetAction($"номер на партида: {lotId}");
            return Ok(await singlePartService.Get(lotId));
        }

        [HttpPut]
        [Route("Update")]
        public async virtual Task<ActionResult<TPart>> Put([FromBody] TPart part)
        {
            await actionWorkflowService.LogUpdateAction($"номер на партида: {part.Id}");
            permissionService.VerifyEditPermissions();
            return Ok(await singlePartService.Put(part, 0));
        }

        [HttpPost]
        [Route("Create")]
        public async virtual Task<ActionResult<TPart>> Post([FromBody] TPart part)
        {
            await actionWorkflowService.LogCreateAction($"номер на партида: {part.Id}");
            permissionService.VerifyEditPermissions();
            return Ok(await singlePartService.Post(part, 0));
        }

        [HttpGet]
        [Route("{lotId:int}/History")]
        public async virtual Task<ActionResult<PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>>> GetHistory([FromRoute] int lotId)
        {
            return Ok(await singlePartService.GetHistory(lotId));
        }
    }
}
