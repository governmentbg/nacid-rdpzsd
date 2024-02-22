using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
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
    public class PersonStudentDiplomaController : ControllerBase
    {
        private readonly PersonStudentDiplomaService personStudentDiplomaService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly PersonStickerValidationService personStickerValidationService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<AwPersonStudentDiplomaDto> actionWorkflowService;

        public PersonStudentDiplomaController(
            PersonStudentDiplomaService personStudentDiplomaService,
            PersonStudentService personStudentService,
            PersonLotValidationService personLotValidationService,
            PersonStickerValidationService personStickerValidationService,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwPersonStudentDiplomaDto> actionWorkflowService
            )
        {
            this.personStudentDiplomaService = personStudentDiplomaService;
            this.personStudentService = personStudentService;
            this.personLotValidationService = personLotValidationService;
            this.personStickerValidationService = personStickerValidationService;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<PersonStudentDiploma>> Create([FromBody] PersonStudentDiploma newDiploma)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"ид на диплома - {newDiploma.Id}");
            var actualPart = await personStudentService.Get(newDiploma.Id);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyUniquePersonDiploma(actualPart);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithoutDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_DiplomaError, StudentStickerState.Recieved);

            return Ok(await personStudentDiplomaService.Create(newDiploma, actualPart));
        }

        [HttpPut]
        public async Task<ActionResult<PersonStudentDiploma>> Update([FromBody] PersonStudentDiploma updateDiploma)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"ид на диплома - {updateDiploma.Id}");
            var actualPart = await personStudentService.Get(updateDiploma.Id);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_DiplomaError, StudentStickerState.Recieved);

            return Ok(await personStudentDiplomaService.Update(updateDiploma, actualPart));
        }

        [HttpPut("{partId:int}/Invalid")]
        public async Task<ActionResult<PersonStudentDiploma>> Invalid([FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Инвалидиране на диплома с номер на партида - {partId}");
            var actualPart = await personStudentService.Get(partId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_DiplomaError, StudentStickerState.Recieved);

            return Ok(await personStudentDiplomaService.Invalid(actualPart));
        }
    }
}
