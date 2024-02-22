using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
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
    public class PersonDoctoralSemesterController : ControllerBase
    {
        private readonly PersonDoctoralSemesterService personDoctoralSemesterService;
        private readonly PersonDoctoralService personDoctoralService;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ActionWorkflowService<AwPersonDoctoralSemesterDto> actionWorkflowService;

        public PersonDoctoralSemesterController(
            PersonDoctoralSemesterService personDoctoralSemesterService,
            PersonDoctoralService personDoctoralService,
            PersonLotValidationService personLotValidationService,
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwPersonDoctoralSemesterDto> actionWorkflowService
            )
        {
            this.personDoctoralSemesterService = personDoctoralSemesterService;
            this.personDoctoralService = personDoctoralService;
            this.personLotValidationService = personLotValidationService;
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<PersonDoctoralSemester>> Create([FromBody] PersonDoctoralSemester newSemester)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"номер на атестация - {newSemester.Id}, към партида - {newSemester.PartId}");
            var actualPart = await personDoctoralService.Get(newSemester.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
            personLotValidationService.ValidateDoctoralSemesterProtocolDate(actualPart, newSemester);
            
            return Ok(await personDoctoralSemesterService.Create(newSemester, actualPart));
        }

        [HttpPut]
        public async Task<ActionResult<PersonDoctoralSemester>> Update([FromBody] PersonDoctoralSemester updateSemester)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"номер на атестация - {updateSemester.Id}, към партида - {updateSemester.PartId}");
            var actualPart = await personDoctoralService.Get(updateSemester.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
            
            return Ok(await personDoctoralSemesterService.Update(updateSemester, actualPart));
        }

        [HttpDelete]
        [Route("{semesterId:int}/{partId:int}")]
        public async Task<ActionResult<PersonDoctoralSemester>> Delete([FromRoute] int semesterId, [FromRoute] int partId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogDeleteAction($"номер на атестация - {semesterId}, към партида - {partId}");
            var semesterForDelete = await new PersonDoctoralSemester().IncludeAll(context.Set<PersonDoctoralSemester>().AsQueryable())
                        .SingleAsync(e => e.Id == semesterId && e.PartId == partId);

            var actualPart = await personDoctoralService.Get(semesterForDelete.PartId);
            await personLotValidationService.VerifyLotState(actualPart.LotId, LotState.Actual);
            personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
            personLotValidationService.VerifyPersonStudentNotGraduated<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
            personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
            personLotValidationService.VerifyAtLeastOneSemester(actualPart.Semesters.ToList());
            await personLotValidationService.VerifyNotSelectedAsRelocated(partId, semesterForDelete);

            await personDoctoralSemesterService.Delete(semesterForDelete, actualPart);
            return Ok();
        }
    }
}
