using Infrastructure.User;
using Logs.Dtos.AwNomenclatureDtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.Nomenclatures;
using Server.Controllers.Nomenclatures.Base;

namespace Server.Controllers.Nomenclatures
{
    [ApiController]
    [Route("api/Nomenclature/Institution")]
    public class InstitutionController : BaseNomenclatureCodeController<Institution, InstitutionFilterDto, InstitutionNomenclatureService, AwInstitutionNomenclatureDto>
    {
        public InstitutionController(
            InstitutionNomenclatureService institutionNomenclatureService,
            UserContext userContext,
			ActionWorkflowService<AwInstitutionNomenclatureDto> actionWorkflowService
            )
            : base(institutionNomenclatureService, userContext, actionWorkflowService)
        {
        }
    }

	[ApiController]
	[Route("api/Nomenclature/EducationalQualification")]
	public class EducationalQualificationController : BaseNomenclatureController<EducationalQualification, EducationalQualificationFilterDto, EducationalQualificationNomenclatureService, AwEducationalQualificationNomenclatureDto>
	{
		public EducationalQualificationController(
			EducationalQualificationNomenclatureService educationalQualificationNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwEducationalQualificationNomenclatureDto> actionWorkflowService
			)
			: base(educationalQualificationNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/EducationalForm")]
	public class EducationalFormController : BaseNomenclatureController<EducationalForm, NomenclatureFilterDto<EducationalForm>, EducationalFormNomenclatureService, AwEducationalFormNomenclatureDto>
	{
		public EducationalFormController(
			EducationalFormNomenclatureService educationalFormNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwEducationalFormNomenclatureDto> actionWorkflowService
			)
			: base(educationalFormNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/ResearchArea")]
	public class ResearchAreaController : BaseNomenclatureCodeController<ResearchArea, NomenclatureHierarchyFilterDto<ResearchArea>, ResearchAreaNomenclatureService, AwResearchAreaNomenclatureDto>
	{
		public ResearchAreaController(
			ResearchAreaNomenclatureService researchAreaNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwResearchAreaNomenclatureDto> actionWorkflowService
			)
			: base(researchAreaNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}
}
