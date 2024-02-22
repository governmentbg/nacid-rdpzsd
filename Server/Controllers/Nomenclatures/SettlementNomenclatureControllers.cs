using Infrastructure.User;
using Logs.Dtos.AwNomenclatureDtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Dtos.Nomenclatures.Settlements;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.Nomenclatures;
using Server.Controllers.Nomenclatures.Base;

namespace Server.Controllers.Nomenclatures
{
	[ApiController]
	[Route("api/Nomenclature/Country")]
	public class CountryController : BaseNomenclatureCodeController<Country, NomenclatureCodeFilterDto<Country>, CountryNomenclatureService, AwCountryNomenclatureDto>
	{
		public CountryController(
			CountryNomenclatureService countryNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwCountryNomenclatureDto> actionWorkflowService
			)
			: base(countryNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/District")]
	public class DistrictController : BaseNomenclatureCodeController<District, NomenclatureCodeFilterDto<District>, DistrictNomenclatureService, AwDistrictNomenclatureDto>
	{
		public DistrictController(
			DistrictNomenclatureService districtNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwDistrictNomenclatureDto> actionWorkflowService
			)
			: base(districtNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/Municipality")]
	public class MunicipalityController : BaseNomenclatureCodeController<Municipality, MunicipalityFilterDto, MunicipalityNomenclatureService, AwMunicipalityNomenclatureDto>
	{
		public MunicipalityController(
			MunicipalityNomenclatureService municipalityNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwMunicipalityNomenclatureDto> actionWorkflowService
			)
			: base(municipalityNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}

	[ApiController]
	[Route("api/Nomenclature/Settlement")]
	public class SettlementController : BaseNomenclatureCodeController<Settlement, SettlementFilterDto, SettlementNomenclatureService, AwSettlementNomenclatureDto>
	{
		public SettlementController(
			SettlementNomenclatureService settlementNomenclatureService,
			UserContext userContext,
			ActionWorkflowService<AwSettlementNomenclatureDto> actionWorkflowService
			)
			: base(settlementNomenclatureService, userContext, actionWorkflowService)
		{
		}
	}
}
