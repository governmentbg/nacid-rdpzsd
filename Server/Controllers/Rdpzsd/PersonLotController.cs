using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Lot;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonLotController : ControllerBase
    {
        private readonly PersonLotService personlotService;
        private readonly PermissionService permissionService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly RdpzsdDbContext context;
        private readonly ActionWorkflowService<AwPersonLotDto> actionWorkflowService;

        public PersonLotController(
            PersonLotService personlotService,
            PermissionService permissionService,
            PersonLotValidationService personLotValidationService,
            DomainValidatorService domainValidatorService,
            RdpzsdDbContext context,
            ActionWorkflowService<AwPersonLotDto> actionWorkflowService
        )
        {
            this.personlotService = personlotService;
            this.permissionService = permissionService;
            this.personLotValidationService = personLotValidationService;
            this.domainValidatorService = domainValidatorService;
            this.context = context;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpGet]
        [Route("{uan}")]
        public async Task<ActionResult<PersonLotDto>> GetLot([FromRoute] string uan)
        {
            await actionWorkflowService.LogGetAction($"ЕАН - {uan}");
            return Ok(await personlotService.GetLot(uan));
        }

        [HttpGet]
        [Route("{lotId:int}/State")]
        public async Task<ActionResult<LotState>> GetLotState([FromRoute] int lotId)
        {
            return Ok(await personlotService.GetLotState(lotId));
        }

        [HttpPost("CreateLot")]
        public async Task<ActionResult<PersonLot>> CreateLot([FromBody] PersonBasic personBasic)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"номер на партида основни данни - {personBasic.Id}");
            permissionService.VerifyEditPermissions();
            await personLotValidationService.ValidateUniquePersonLot(personBasic.Uin, personBasic.ForeignerNumber, null);
            await personLotValidationService.ValidateUniquePersonEmail(personBasic.Email, null);

            return Ok(await personlotService.CreateLot(personBasic));
        }

        [HttpPost]
        [Route("{lotId:int}/CancelPendingApproval")]
        public async virtual Task<ActionResult<LotState>> CancelPendingApproval([FromRoute] int lotId, [FromBody] NoteDto model)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Отказ на изчакващо одобрение, номер на партида - {lotId}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
            return Ok(await personlotService.CancelPendingApproval(lotId, model));
        }

        [HttpPost]
        [Route("{lotId:int}/SendForApproval")]
        public async virtual Task<ActionResult<LotState>> SendForApproval([FromRoute] int lotId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Изпращане за одобрение, номер на партида - {lotId}");
            await personLotValidationService.VerifyLotStateRsdUser(lotId, LotState.CancelApproval, LotState.MissingPassportCopy);
            return Ok(await personlotService.SendForApproval(lotId));
        }

        [HttpPost]
        [Route("{lotId:int}/Approve")]
        public async virtual Task<ActionResult<LotState>> Approve([FromRoute] int lotId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Одобрение, номер на партида - {lotId}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
            return Ok(await personlotService.Approve(lotId));
        }

        [HttpPost]
        [Route("{lotId:int}/Erase")]
        public async virtual Task<ActionResult<LotState>> Erase([FromRoute] int lotId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();

            await actionWorkflowService.LogDeleteAction($"номер на партида - {lotId}");

            permissionService.VerifyEditPermissions();

            var personLot = await context.PersonLots
                .Include(e => e.PersonStudents)
                .Include(e => e.PersonDoctorals)
                .SingleAsync(e => e.Id == lotId && e.State == LotState.Actual);

            personLotValidationService.VerifyNotStudentOrDoctoral(personLot);

            return Ok(await personlotService.Erase(personLot));
        }

        [HttpGet]
        [Route("{lotId:int}/LotActions")]
        public async Task<ActionResult<List<PersonLotActionDto>>> GetLotActions([FromRoute] int lotId)
        {
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdEdit);
            return Ok(await personlotService.GetPersonLotActions(lotId));
        }

        [HttpGet]
        [Route("{lotId:int}/LatestPersonLotActionByType")]
        public async Task<ActionResult<PersonLotAction>> GetLatestPersonLotActionByType([FromRoute] int lotId, [FromQuery] PersonLotActionType actionType)
        {
            await personLotValidationService.VerifyPersonLotActionsPermissions(lotId);
            return Ok(await personlotService.GetLatestPersonLotActionByType(lotId, actionType));
        }
    }
}
