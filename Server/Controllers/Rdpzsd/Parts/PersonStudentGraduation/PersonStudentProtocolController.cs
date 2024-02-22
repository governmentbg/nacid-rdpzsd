using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts;
using Rdpzsd.Services.Rdpzsd.Parts.PersonStudentGraduation;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts.PersonStudentGraduation
{
    [ApiController]
    [Route("api/PersonLot/[controller]")]
    public class PersonStudentProtocolController : ControllerBase
    {
        private readonly PersonStudentProtocolService personStudentProtocolService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly PersonStickerValidationService personStickerValidationService;
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<AwPersonStudentProtocolDto> actionWorkflowService;

        public PersonStudentProtocolController(
            PersonStudentProtocolService personStudentProtocolService,
            PersonStudentService personStudentService,
            PersonLotValidationService personLotValidationService,
            PersonStickerValidationService personStickerValidationService,
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwPersonStudentProtocolDto> actionWorkflowService
            )
        {
            this.personStudentProtocolService = personStudentProtocolService;
            this.personStudentService = personStudentService;
            this.personLotValidationService = personLotValidationService;
            this.personStickerValidationService = personStickerValidationService;
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<PersonStudentProtocol>> Create([FromBody] PersonStudentProtocol newProtocol)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"номер на партидата - {newProtocol.PartId}");
            var actualPart = await personStudentService.Get(newProtocol.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatus(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_ProtocolsError, StudentStickerState.ReturnedForEdit, StudentStickerState.None);

            return Ok(await personStudentProtocolService.Create(newProtocol, actualPart));
        }

        [HttpPut]
        public async Task<ActionResult<PersonStudentProtocol>> Update([FromBody] PersonStudentProtocol updateProtocol)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"номер на партидата - {updateProtocol.PartId}");
            var actualPart = await personStudentService.Get(updateProtocol.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatus(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_ProtocolsError, StudentStickerState.ReturnedForEdit, StudentStickerState.None);

            return Ok(await personStudentProtocolService.Update(updateProtocol, actualPart));
        }

        [HttpDelete]
        [Route("{protocolId:int}/{partId:int}")]
        public async Task<ActionResult<PersonStudentProtocol>> Delete([FromRoute] int protocolId, [FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogDeleteAction($"ид на протокола - {protocolId}, номер на партидата - {partId}");

            var protocolForDelete = await context.Set<PersonStudentProtocol>()
                        .SingleAsync(e => e.Id == protocolId && e.PartId == partId);

            var actualPart = await personStudentService.Get(protocolForDelete.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatus(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_ProtocolsError, StudentStickerState.ReturnedForEdit, StudentStickerState.None);

            await personStudentProtocolService.Delete(protocolForDelete, actualPart);
            return Ok();
        }
    }
}
