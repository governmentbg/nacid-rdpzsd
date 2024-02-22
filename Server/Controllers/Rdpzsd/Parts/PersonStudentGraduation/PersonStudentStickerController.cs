using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts.PersonStudentSticker;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentSticker;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts;
using Rdpzsd.Services.Rdpzsd.Parts.PersonStudentGraduation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts.PersonStudentGraduation
{
    [ApiController]
    [Route("api/PersonLot/[controller]")]
    public class PersonStudentStickerController : ControllerBase
    {
        private readonly PersonStudentStickerService personStudentStickerService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonStudentDuplicateDiplomaService personStudentDuplicateDiplomaService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly PersonStickerValidationService personStickerValidationService;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<AwPersonStudentStickerDto> actionWorkflowService;

        public PersonStudentStickerController(
            PersonStudentStickerService personStudentStickerService,
            PersonStudentService personStudentService,
            PersonStudentDuplicateDiplomaService personStudentDuplicateDiplomaService,
            PersonLotValidationService personLotValidationService,
            PersonStickerValidationService personStickerValidationService,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwPersonStudentStickerDto> actionWorkflowService
            )
        {
            this.personStudentStickerService = personStudentStickerService;
            this.personStudentService = personStudentService;
            this.personStudentDuplicateDiplomaService = personStudentDuplicateDiplomaService;
            this.personLotValidationService = personLotValidationService;
            this.personStickerValidationService = personStickerValidationService;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPut]
        [Route("{partId:int}/SendForSticker")]
        public async Task<ActionResult<StudentStickerState>> SendForSticker([FromRoute] int partId, [FromBody] StickerDto stickerDto)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Изпращане на диплома за стикер с номер на партида - {partId}");
            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatus(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_SendForSticker, StudentStickerState.ReturnedForEdit, StudentStickerState.None);
            personStickerValidationService.VerifyStickerYear(stickerDto.StickerYear);

            return Ok(await personStudentStickerService.SendForSticker(stickerDto, actualPart));
        }

        [HttpPut]
        [Route("{partId:int}/ReturnForEdit")]
        public async Task<ActionResult<StudentStickerState>> ReturnForEdit([FromRoute] int partId, [FromBody] StickerDto stickerDto)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Връщане за редакция за стикер с номер на партида - {partId}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatus(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_ReturnForEdit, StudentStickerState.SendForStickerDiscrepancy);

            return Ok(await personStudentStickerService.ReturnForEdit(stickerDto, actualPart));
        }

        [HttpPut]
        [Route("MarkedForPrint")]
        public async Task<ActionResult> MarkedForPrint([FromBody] List<int> personStudentIds)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Изпращане на стикери за печат, номер на партиди - {string.Join(',', personStudentIds)}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            if (personStudentIds != null && personStudentIds.Any()) {
                await personStudentStickerService.MarkedForPrint(personStudentIds);
            }

            return Ok();
        }

        [HttpPut]
        [Route("MarkedRecieved")]
        public async Task<ActionResult> MarkedRecieved([FromBody] List<int> personStudentIds)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Получаване на стикери, номер на партиди - {string.Join(',', personStudentIds)}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            if (personStudentIds != null && personStudentIds.Any())
            {
                await personStudentStickerService.MarkedRecieved(personStudentIds);
            }

            return Ok();
        }

        [HttpPut]
        [Route("{partId:int}/ForPrint")]
        public async Task<ActionResult<PersonStudentStickerSearchDto>> ForPrint([FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Изпращане на стикер за печат, номер на партида - {partId}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatusOrEvent(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation, actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithoutDiploma);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_ForPrint, StudentStickerState.SendForSticker, StudentStickerState.SendForStickerDiscrepancy, StudentStickerState.ReissueSticker);

            return Ok(await personStudentStickerService.ForPrint(actualPart));
        }

        [HttpPut]
        [Route("{duplicateDiplomaId:int}/ForPrintDuplicate")]
        public async Task<ActionResult<StudentStickerState>> ForPrintDuplicate([FromRoute] int duplicateDiplomaId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Изпращане на стикер за печат от дубликат на диплома, ид на дубликата на дипломата - {duplicateDiplomaId}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            var duplicateDiploma = await personStudentDuplicateDiplomaService.Get(duplicateDiplomaId);
            var actualPart = await personStudentService.Get(duplicateDiploma.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personStickerValidationService.VerifyStickerState(duplicateDiploma.DuplicateStickerState, SystemErrorCode.PersonStudentSticker_ForPrint, StudentStickerState.SendForSticker, StudentStickerState.ReissueSticker);
            personLotValidationService.VerifyUniqueValidDuplicateDiploma(actualPart, duplicateDiploma.Id);

            return Ok(await personStudentStickerService.ForPrintDuplicate(duplicateDiploma, actualPart));
        }

        [HttpPut]
        [Route("{partId:int}/Recieved")]
        public async Task<ActionResult<StudentStickerState>> Recieved([FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Получаване на стикер, номер на партидата - {partId}");
            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithoutDiploma);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_Recieved, StudentStickerState.StickerForPrint);

            return Ok(await personStudentStickerService.Recieved(actualPart));
        }

        [HttpPut]
        [Route("{duplicateDiplomaId:int}/RecievedDuplicate")]
        public async Task<ActionResult<StudentStickerState>> RecievedDuplicate([FromRoute] int duplicateDiplomaId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Получаване на стикер от дублукат на диплома, ид на дубликата на дипломата - {duplicateDiplomaId}");

            permissionService.VerifyInternalUser(PermissionConstants.RdpzsdStickers);

            var duplicateDiploma = await personStudentDuplicateDiplomaService.Get(duplicateDiplomaId);
            var actualPart = await personStudentService.Get(duplicateDiploma.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personStickerValidationService.VerifyStickerState(duplicateDiploma.DuplicateStickerState, SystemErrorCode.PersonStudentSticker_Recieved, StudentStickerState.StickerForPrint);
            personLotValidationService.VerifyUniqueValidDuplicateDiploma(actualPart, duplicateDiploma.Id);

            return Ok(await personStudentStickerService.RecievedDuplicate(duplicateDiploma, actualPart));
        }

        [HttpPut]
        [Route("{partId:int}/ReissueSticker")]
        public async Task<ActionResult<StudentStickerState>> ReissueSticker([FromRoute] int partId, [FromBody] StickerDto stickerDto)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Преиздаване на стикер, номер на партидата - {partId}");
            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithoutDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_ReissueSticker, StudentStickerState.Recieved);
            personStickerValidationService.VerifyStickerYear(stickerDto.StickerYear);

            return Ok(await personStudentStickerService.ReissueSticker(stickerDto, actualPart));
        }

        [HttpGet]
        [Route("{partId:int}/ValidatePersonStudentStickerInfo")]
        public async Task<ActionResult<StickerErrorDto>> ValidatePersonStudentStickerInfo([FromRoute] int partId)
        {
            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentStatus(actualPart.StudentStatus?.Alias, StudentStatusConstants.ProcessGraduation);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart, true, true);

            return Ok(await personStudentStickerService.ValidatePersonStudentStickerInfo(actualPart));
        }
    }
}
