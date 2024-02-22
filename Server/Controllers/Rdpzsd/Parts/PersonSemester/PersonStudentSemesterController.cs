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
using Rdpzsd.Services.Rdpzsd.Parts.PersonSemester;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts.PersonSemester
{
    [ApiController]
    [Route("api/PersonLot/[controller]")]
    public class PersonStudentSemesterController : ControllerBase
    {
        private readonly PersonStudentSemesterService personStudentSemesterService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly PersonStickerValidationService personStickerValidationService;
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<AwPersonStudentSemesterDto> actionWorkflowService;

        public PersonStudentSemesterController(
            PersonStudentSemesterService personStudentSemesterService,
            PersonStudentService personStudentService,
            PersonLotValidationService personLotValidationService,
            PersonStickerValidationService personStickerValidationService,
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwPersonStudentSemesterDto> actionWorkflowService
            )
        {
            this.personStudentSemesterService = personStudentSemesterService;
            this.personStudentService = personStudentService;
            this.personLotValidationService = personLotValidationService;
            this.personStickerValidationService = personStickerValidationService;
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<PersonStudentSemester>> Create([FromBody] PersonStudentSemester newSemester)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"номер на семестър - {newSemester.Id}, курс - {newSemester.Course}, семестър - {newSemester.StudentSemester}, към партида - {newSemester.PartId}");
            var actualPart = await personStudentService.Get(newSemester.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_SemesterError, StudentStickerState.ReturnedForEdit, StudentStickerState.None);
            personLotValidationService.VerifyUniquePeriod(actualPart.Semesters.ToList(), newSemester);

            return Ok(await personStudentSemesterService.Create(newSemester, actualPart));
        }

        [HttpPut]
        public async Task<ActionResult<PersonStudentSemester>> Update([FromBody] PersonStudentSemester updateSemester)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"номер на семестър - {updateSemester.Id}, курс - {updateSemester.Course}, семестър - {updateSemester.StudentSemester}, към партида - {updateSemester.PartId}");
            var actualPart = await personStudentService.Get(updateSemester.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_SemesterError, StudentStickerState.ReturnedForEdit, StudentStickerState.None);
            personLotValidationService.VerifyUniquePeriod(actualPart.Semesters.ToList(), updateSemester);

            return Ok(await personStudentSemesterService.Update(updateSemester, actualPart));
        }

        [HttpDelete]
        [Route("{semesterId:int}/{partId:int}")]
        public async Task<ActionResult<PersonStudentSemester>> Delete([FromRoute] int semesterId, [FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogDeleteAction($"номер на семестър - {semesterId}, към партида - {partId}");
            var semesterForDelete = await new PersonStudentSemester().IncludeAll(context.Set<PersonStudentSemester>().AsQueryable())
                        .SingleAsync(e => e.Id == semesterId && e.PartId == partId);

            var actualPart = await personStudentService.Get(semesterForDelete.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonStudent, PersonStudentInfo, PersonStudentSemester>(actualPart);
            personStickerValidationService.VerifyStickerState(actualPart.StickerState, SystemErrorCode.PersonStudentSticker_SemesterError, StudentStickerState.ReturnedForEdit, StudentStickerState.None);
            personLotValidationService.VerifyAtLeastOneSemester(actualPart.Semesters.ToList());
            await personLotValidationService.VerifyNotSelectedAsRelocated(partId, semesterForDelete);

            await personStudentSemesterService.Delete(semesterForDelete, actualPart);
            return Ok();
        }
    }
}
