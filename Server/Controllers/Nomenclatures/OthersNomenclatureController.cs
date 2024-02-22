using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.User;
using Logs.Dtos;
using Logs.Dtos.AwNomenclatureDtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Dtos.Nomenclatures.Others;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Services.Nomenclatures;
using Rdpzsd.Services.Permissions;
using Server.Controllers.Nomenclatures.Base;
using System.Threading.Tasks;

namespace Server.Controllers.Nomenclatures
{
    [ApiController]
	[Route("api/Nomenclature/Period")]
	public class PeriodController : BaseEditableNomenclatureController<Period, PeriodFilterDto, PeriodNomenclatureService, AwPeriodNomenclatureDto>
	{
		public PeriodController(
			PeriodNomenclatureService periodNomenclatureService,
			UserContext userContext,
			PermissionService permissionService,
			RdpzsdDbContext context,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwPeriodNomenclatureDto> actionWorkflowService
			)
			: base(periodNomenclatureService, userContext, permissionService, context, domainValidatorService, actionWorkflowService)
		{
		}

		public async override Task<ActionResult<Period>> Create([FromBody] Period entity)
		{
            if (await context.Periods.AsNoTracking().AnyAsync(e => e.Year == entity.Year && e.Semester == entity.Semester))
            {
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.Period_Exists);
			}

			return await base.Create(entity);
		}

		[HttpGet("Latest")]
		public async Task<ActionResult<Period>> GetLatestPeriod()
		{
			return await nomenclatureService.GetLatestPeriod();
		}


		[HttpGet("Next")]
		public async Task<ActionResult<Period>> GetNextPeriod([FromQuery] int year, [FromQuery] Semester semester)
		{
			if (year < 1 || (semester != Semester.First && semester != Semester.Second))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.Period_NotValidPeriod);
			}

			return await nomenclatureService.GetNextPeriod(year, semester);
		}

		[HttpGet("Previous")]
		public async Task<ActionResult<Period>> GetPreviousPeriod([FromQuery] int year, [FromQuery] Semester semester)
		{
			if (year < 1 || (semester != Semester.First && semester != Semester.Second))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.Period_NotValidPeriod);
			}

			return await nomenclatureService.GetPreviousPeriod(year, semester);
		}
	}

	[ApiController]
	[Route("api/Nomenclature/AdmissionReason")]
	public class AdmissionReasonController: BaseEditableNomenclatureController<AdmissionReason, AdmissionReasonFilterDto, AdmissionReasonNomenclatureService, AwAdmissionReasonNomenclatureDto>
	{
		public AdmissionReasonController(
			AdmissionReasonNomenclatureService admissionReasonNomenclatureService,
			UserContext userContext,
			PermissionService permissionService,
			RdpzsdDbContext context,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwAdmissionReasonNomenclatureDto> actionWorkflowService
			)
			: base(admissionReasonNomenclatureService, userContext, permissionService, context, domainValidatorService, actionWorkflowService)
		{
		}
    }

	[ApiController]
	[Route("api/Nomenclature/EducationFeeType")]
	public class EducationFeeTypeController: BaseNomenclatureController<EducationFeeType, EducationFeeTypeFilterDto, EducationFeeTypeNomenclatureService, AwEducationFeeTypeNomenclatureDto>
    {
		public EducationFeeTypeController(
			EducationFeeTypeNomenclatureService educationFeeTypeNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwEducationFeeTypeNomenclatureDto> actionWorkflowService
			)
			: base(educationFeeTypeNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/School")]
	public class SchoolController : BaseEditableNomenclatureController<School, SchoolFilterDto, SchoolNomenclatureService, AwSchoolNomenclatureDto>
	{
		public SchoolController(
			SchoolNomenclatureService schoolNomenclatureService,
			UserContext userContext,
			PermissionService permissionService,
			RdpzsdDbContext context,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwSchoolNomenclatureDto> actionWorkflowService
			)
			: base(schoolNomenclatureService, userContext, permissionService, context, domainValidatorService, actionWorkflowService)
		{
		}
    }
}
