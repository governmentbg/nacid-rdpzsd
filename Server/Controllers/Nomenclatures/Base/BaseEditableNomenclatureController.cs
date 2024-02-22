using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.User;
using Logs.Interfaces;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using Rdpzsd.Services.Nomenclatures.Base;
using Rdpzsd.Services.Permissions;
using System.Threading.Tasks;

namespace Server.Controllers.Nomenclatures.Base
{
    public abstract class BaseEditableNomenclatureController<T, TFilter, TService, ТАctionWorkflowOperation> : BaseNomenclatureController<T, TFilter, TService, ТАctionWorkflowOperation>
        where T: Nomenclature, IIncludeAll<T>, IValidate, new()
		where TFilter : NomenclatureFilterDto<T>, new()
		where TService: BaseEditableNomenclatureService<T, TFilter>
        where ТАctionWorkflowOperation : IActionWorkflowOperation, new()
    {
        protected readonly PermissionService permissionService;
        protected readonly RdpzsdDbContext context;
        protected readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<ТАctionWorkflowOperation> actionWorkflowService;

        public BaseEditableNomenclatureController(
			TService nomenclatureEditableService,
			UserContext userContext,
            PermissionService permissionService,
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<ТАctionWorkflowOperation> actionWorkflowService
            )
                : base(nomenclatureEditableService, userContext, actionWorkflowService)
		{
            this.permissionService = permissionService;
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost("Delete")]
        public async virtual Task<ActionResult> Delete([FromBody] int id)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogDeleteAction($"id - {id}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
            await nomenclatureService.Delete(id);
            return Ok();
        }

        [HttpPut]
		public async virtual Task<ActionResult<T>> Update([FromBody] T entity)
        {
            await actionWorkflowService.LogUpdateAction($"id - {entity.Id}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
            entity.ValidateProperties(context, domainValidatorService);
            return Ok(await nomenclatureService.Update(entity));
        }

        [HttpPost("Create")]
        public async virtual Task<ActionResult<T>> Create([FromBody] T entity)
        {
            await actionWorkflowService.LogCreateAction($"id - {entity.Id}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
            entity.ValidateProperties(context, domainValidatorService);
            return Ok(await nomenclatureService.Create(entity));
        }
	}
}
