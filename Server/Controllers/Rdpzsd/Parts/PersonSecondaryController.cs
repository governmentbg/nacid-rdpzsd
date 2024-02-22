using Infrastructure.DomainValidation;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary;
using Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts
{
    [ApiController]
    [Route("api/PersonLot/[controller]")]
    public class PersonSecondaryController : BaseSinglePartController<PersonSecondary, PersonSecondaryInfo, PersonSecondaryHistory, PersonSecondaryHistoryInfo, PersonLot, PersonSecondaryService, AwPersonSecondaryDto>
    {
		private readonly DomainValidatorService domainValidatorService;

		public PersonSecondaryController(
			PersonSecondaryService personSecondaryService,
			PermissionService permissionService,
			PersonLotValidationService personLotValidationService,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwPersonSecondaryDto> actionWorkflowService
			)
			: base(personSecondaryService, permissionService, personLotValidationService, actionWorkflowService)
		{
			this.domainValidatorService = domainValidatorService;
		}

		[HttpPut]
		[Route("Update")]
		public async override Task<ActionResult<PersonSecondary>> Put([FromBody] PersonSecondary part)
		{
			domainValidatorService.ThrowFunctionalityNotSupportedError();
			await actionWorkflowService.LogUpdateAction($"номер на партида: {part.Id}");
			permissionService.VerifyRsdUser();
			return Ok(await singlePartService.Put(part, PersonLotActionType.PersonSecondaryEdit));
		}

		[HttpPost]
		[Route("Create")]
		public async override Task<ActionResult<PersonSecondary>> Post([FromBody] PersonSecondary part)
        {
			domainValidatorService.ThrowFunctionalityNotSupportedError();
			await actionWorkflowService.LogCreateAction($"номер на партида: {part.Id}");
			permissionService.VerifyRsdUser();
			return Ok(await singlePartService.Post(part, PersonLotActionType.PersonSecondaryAdd));
		}

		[HttpGet]
		[Route("{lotId:int}/GetFromRso")]
		public async virtual Task<ActionResult<PersonSecondary>> GetFromRso([FromRoute] int lotId)
		{
			await actionWorkflowService.LogCustomAction($"Извличане на данни от РСО за партида с номер: {lotId}");
			permissionService.VerifyRsdUser();
			return Ok(await singlePartService.GetFromRso(lotId));
		}

		[HttpGet]
		[Route("{rsoIntId:double}/GetImagesFromRso")]
		public async virtual Task<FileStreamResult> RsoImagesPdf([FromRoute] double? rsoIntId)
        {
			await actionWorkflowService.LogCustomAction($"Извличане на прикачени дипломи от РСО с външен номер: {rsoIntId}");
			permissionService.VerifyEditPermissions();
			var pdfStream = await singlePartService.GetImagesFromRso(rsoIntId);
			return new FileStreamResult(pdfStream, "application/pdf");
		}
	}
}
