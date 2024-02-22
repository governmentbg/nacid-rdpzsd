using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts;
using Server.Controllers.Rdpzsd.Parts.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts
{
    [ApiController]
    [Route("api/PersonLot/[controller]")]
    public class PersonStudentController : BaseMultiPartController<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonLot, PersonStudentFilterDto, PersonStudentService, AwPersonStudentDto>
    {
        private readonly DomainValidatorService domainValidatorService;
        private readonly PersonStickerValidationService personStickerValidationService;

        public PersonStudentController(
			PersonStudentService personStudentPartService,
			PermissionService permissionService,
			PersonLotValidationService personLotValidationService,
            DomainValidatorService domainValidatorService,
            PersonStickerValidationService personStickerValidationService,
            ActionWorkflowService<AwPersonStudentDto> actionWorkflowService
            )
            : base(personStudentPartService, permissionService, personLotValidationService, actionWorkflowService)
		{
            this.domainValidatorService = domainValidatorService;
            this.personStickerValidationService = personStickerValidationService;
        }

        [HttpPost]
        [Route("Create")]
        public async override Task<ActionResult<PersonStudent>> Post([FromBody] PersonStudent part)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"номер на партида - {part.Id}");
            await personLotValidationService.VerifyHasStudentSecondaryAndLotState(part.LotId, part.PeType, LotState.Actual);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(part);
            personLotValidationService.VerifyInitialSpecialitySemester<PersonStudent, PersonStudentInfo, PersonStudentSemester>(part);
            return Ok(await multiPartService.Post(part, PersonLotActionType.PersonStudentAdd, part.InstitutionSpeciality?.Speciality?.Code));
        }

        [HttpPut]
        [Route("Update")]
        public async override Task<ActionResult<PersonStudent>> Put([FromBody] PersonStudent part)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"номер на партида - {part.Id}");
            await personLotValidationService.VerifyHasStudentSecondaryAndLotState(part.LotId, part.PeType, LotState.Actual);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(part);
            personStickerValidationService.VerifyStickerState(part.StickerState, SystemErrorCode.PersonStudentSticker_EditNotAllowed, StudentStickerState.None, StudentStickerState.ReturnedForEdit);

            var actualPart = await multiPartService.Get(part.Id);

            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_EditNotAllowed, StudentStickerState.None, StudentStickerState.ReturnedForEdit);

            return Ok(await multiPartService.Put(actualPart, part, PersonLotActionType.PersonStudentEdit, part.InstitutionSpeciality?.Speciality?.Code));
        }

        [HttpDelete]
        [Route("Erase/{partId:int}")]
        public async override Task<ActionResult<PersonStudent>> Erase([FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();

            await actionWorkflowService.LogDeleteAction($"номер на партида - {partId}");

            var actualPart = await multiPartService.Get(partId);

            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personLotValidationService.VerifyOnlyOneSemester(actualPart.Semesters.ToList());

            return Ok(await multiPartService.Erase(actualPart, PersonLotActionType.PersonStudentErase, actualPart.InstitutionSpeciality?.Speciality?.Code));
        }
    }
}
