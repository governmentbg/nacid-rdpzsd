using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Services.Permissions;
using Rdpzsd.Services.Rdpzsd.Parts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers.Rdpzsd.Parts
{
    [ApiController]
    [Route("api/PersonLot/[controller]")]
    public class PersonBasicController : BaseSinglePartController<PersonBasic, PersonBasicInfo, PersonBasicHistory, PersonBasicHistoryInfo, PersonLot, PersonBasicService, AwPersonBasicDto>
    {
		private readonly DomainValidatorService domainValidatorService;

		public PersonBasicController(
			PersonBasicService personBasicPartService,
			PermissionService permissionService,
			PersonLotValidationService personLotValidationService,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwPersonBasicDto> actionWorkflowService
			)
			: base(personBasicPartService, permissionService, personLotValidationService, actionWorkflowService)
		{
			this.domainValidatorService = domainValidatorService;
		}

		[HttpPost]
		[Route("Create")]
		public async override Task<ActionResult<PersonBasic>> Post([FromBody] PersonBasic part)
		{
			domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityNotSupported);
			await Task.CompletedTask;
			return null;
		}

		[HttpPut]
		[Route("Update")]
		public async override Task<ActionResult<PersonBasic>> Put([FromBody] PersonBasic part)
		{
			domainValidatorService.ThrowFunctionalityNotSupportedError();
			await actionWorkflowService.LogUpdateAction($"номер на партида: {part.Id}");
			permissionService.VerifyEditPermissions();
			await personLotValidationService.VerifyPersonBasicEditPermissions(part.Id);
			await personLotValidationService.ValidateUniquePersonLot(part.Uin, part.ForeignerNumber, part.Id);
			await personLotValidationService.ValidateUniquePersonEmail(part.Email, part.Id);
			return Ok(await singlePartService.Put(part, PersonLotActionType.PersonBasicEdit));
		}

		[HttpGet]
		[Route("{lotId:int}/HistoryImages")]
		public async virtual Task<ActionResult<List<string>>> GetHistoryImages([FromRoute] int lotId)
		{
			await actionWorkflowService.LogCustomAction($"Преглед на снимки на ФЛ с номер: {lotId}");
			return Ok(await singlePartService.GetHistoryImages(lotId));
		}
	}
}
