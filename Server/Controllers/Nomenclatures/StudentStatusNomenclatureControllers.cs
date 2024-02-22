using Infrastructure.User;
using Logs.Dtos.AwNomenclatureDtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Dtos.Nomenclatures.StudentStatus;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using Rdpzsd.Services.Nomenclatures;
using Server.Controllers.Nomenclatures.Base;

namespace Server.Controllers.Nomenclatures
{
	[ApiController]
	[Route("api/Nomenclature/StudentStatus")]
	public class StudentStatusController : BaseNomenclatureController<StudentStatus, NomenclatureFilterDto<StudentStatus>, StudentStatusNomenclatureService, AwStudentStatusNomenclatureDto>
	{
		public StudentStatusController(
			StudentStatusNomenclatureService studentStatusNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwStudentStatusNomenclatureDto> actionWorkflowService
			)
			: base(studentStatusNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/StudentEvent")]
	public class StudentEventController : BaseNomenclatureController<StudentEvent, StudentEventFilterDto, StudentEventNomenclatureService, AwStudentEventNomenclatureDto>
	{
		public StudentEventController(
			StudentEventNomenclatureService studentEventNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwStudentEventNomenclatureDto> actionWorkflowService
			)
			: base(studentEventNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}
}
