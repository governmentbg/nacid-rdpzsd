using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
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
    public class PersonStudentDuplicateDiplomaController : ControllerBase
    {
        private readonly PersonStudentDuplicateDiplomaService personStudentDuplicateDiplomaService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly PersonStickerValidationService personStickerValidationService;
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<AwPersonStudentDuplicateDiplomaDto> actionWorkflowService;

        public PersonStudentDuplicateDiplomaController(
            PersonStudentDuplicateDiplomaService personStudentDuplicateDiplomaService,
            PersonStudentService personStudentService,
            PersonLotValidationService personLotValidationService,
            PersonStickerValidationService personStickerValidationService,
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwPersonStudentDuplicateDiplomaDto> actionWorkflowService
            )
        {
            this.personStudentDuplicateDiplomaService = personStudentDuplicateDiplomaService;
            this.personStudentService = personStudentService;
            this.personLotValidationService = personLotValidationService;
            this.personStickerValidationService = personStickerValidationService;
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<PersonStudentDuplicateDiploma>> Create([FromBody] PersonStudentDuplicateDiplomaCreateDto createDuplicateDiplomaDto)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"номер на партида - {createDuplicateDiplomaDto.NewDuplicateDiploma.PartId}");
            var actualPart = await personStudentService.Get(createDuplicateDiplomaDto.NewDuplicateDiploma.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerYear(createDuplicateDiplomaDto.NewDuplicateDiploma.DuplicateStickerYear);
            personLotValidationService.VerifyUniqueValidDuplicateDiploma(actualPart);

            return Ok(await personStudentDuplicateDiplomaService.Create(createDuplicateDiplomaDto, actualPart));
        }

        [HttpPut]
        public async Task<ActionResult<PersonStudentDuplicateDiploma>> Update([FromBody] PersonStudentDuplicateDiploma updateDuplicateDiploma)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"номер на партида - {updateDuplicateDiploma.PartId}");
            var actualPart = await personStudentService.Get(updateDuplicateDiploma.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(updateDuplicateDiploma.DuplicateStickerState, SystemErrorCode.PersonStudentSticker_DuplicateDiplomaError, StudentStickerState.Recieved, StudentStickerState.None);
            personStickerValidationService.VerifyStickerYear(updateDuplicateDiploma.DuplicateStickerYear);
            personLotValidationService.VerifyUniqueValidDuplicateDiploma(actualPart, updateDuplicateDiploma.Id);

            return Ok(await personStudentDuplicateDiplomaService.Update(updateDuplicateDiploma, actualPart));
        }

        [HttpPut("{duplicateDiplomaId:int}/Invalid")]
        public async Task<ActionResult<PersonStudentDuplicateDiploma>> Invalid([FromRoute] int duplicateDiplomaId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCustomAction($"Инвалидиране на дублирана диплома с ид - {duplicateDiplomaId}");

            var duplicateDiplomaForUpdate = await context.PersonStudentDuplicateDiplomas
                        .Include(e => e.File)
                        .SingleAsync(e => e.Id == duplicateDiplomaId
                            && e.IsValid
                            && e.File != null);

            var actualPart = await personStudentService.Get(duplicateDiplomaForUpdate.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentEvent(actualPart.StudentEvent?.Alias, StudentEventConstants.GraduatedWithDiploma);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(duplicateDiplomaForUpdate.DuplicateStickerState, SystemErrorCode.PersonStudentSticker_DiplomaError, StudentStickerState.Recieved);
            personLotValidationService.VerifyUniqueValidDuplicateDiploma(actualPart, duplicateDiplomaForUpdate.Id);

            return Ok(await personStudentDuplicateDiplomaService.Invalid(duplicateDiplomaForUpdate, actualPart));
        }
    }
}
