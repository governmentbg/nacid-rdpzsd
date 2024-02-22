using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.Rdpzsd;
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
	public class PersonDoctoralController : BaseMultiPartController<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonLot, PersonDoctoralFilterDto, PersonDoctoralService, AwPersonDoctoralDto>
	{
		private readonly DomainValidatorService domainValidatorService;

		public PersonDoctoralController(
			PersonDoctoralService personDoctoralPartService,
			PermissionService permissionService,
			PersonLotValidationService personLotValidationService,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwPersonDoctoralDto> actionWorkflowService
			)
			: base(personDoctoralPartService, permissionService, personLotValidationService, actionWorkflowService)
		{
			this.domainValidatorService = domainValidatorService;
		}

		[HttpPost]
		[Route("Create")]
		public async override Task<ActionResult<PersonDoctoral>> Post([FromBody] PersonDoctoral part)
		{
			domainValidatorService.ThrowFunctionalityNotSupportedError();
			await actionWorkflowService.LogCreateAction($"номер на партида - {part.Id}");
			await personLotValidationService.VerifyLotState(part.LotId, LotState.Actual);
			personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(part);
			personLotValidationService.VerifyInitialSpecialitySemester<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(part);
			return Ok(await multiPartService.Post(part, PersonLotActionType.PersonDoctoralAdd, part.InstitutionSpeciality?.Speciality?.Code));
		}

		[HttpPut]
		[Route("Update")]
		public async override Task<ActionResult<PersonDoctoral>> Put([FromBody] PersonDoctoral part)
		{
			domainValidatorService.ThrowFunctionalityNotSupportedError();
			await actionWorkflowService.LogUpdateAction($"номер на партида - {part.Id}");
			await personLotValidationService.VerifyLotState(part.LotId, LotState.Actual);
			personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(part);

			var actualPart = await multiPartService.Get(part.Id);

			personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
			personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
			personLotValidationService.VerifyPersonStudentNotGraduated<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);

			return Ok(await multiPartService.Put(actualPart, part, PersonLotActionType.PersonDoctoralEdit, part.InstitutionSpeciality?.Speciality?.Code));
		}

		[HttpDelete]
		[Route("Erase/{partId:int}")]
		public async override Task<ActionResult<PersonDoctoral>> Erase([FromRoute] int partId)
		{
			domainValidatorService.ThrowFunctionalityNotSupportedError();
			await actionWorkflowService.LogDeleteAction($"номер на партида - {partId}");

			var actualPart = await multiPartService.Get(partId);

			personLotValidationService.VerifyPartState(actualPart.State, PartState.Actual);
			personLotValidationService.VerifyStudentDoctoralPartEditPermission<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
			personLotValidationService.VerifyPersonStudentNotGraduated<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester>(actualPart);
			personLotValidationService.VerifyOnlyOneSemester(actualPart.Semesters.ToList());

			return Ok(await multiPartService.Erase(actualPart, PersonLotActionType.PersonDoctoralErase, actualPart.InstitutionSpeciality?.Speciality?.Code));
		}
	}
}
