using System.Collections.Generic;

namespace Infrastructure.Integrations.RndIntegration.Dtos
{
	public class InstitutionDto : BaseInstitutionDto
	{
		public string Logo { get; set; }

		public bool HasBachelor { get; set; } = false;
		public bool HasMaster { get; set; } = false;
		public bool HasDoctoral { get; set; } = false;

		public int? OrganizationType { get; set; }

		public List<ChildInstitutionDto> AcadChildOrganizationsDto { get; set; } = new List<ChildInstitutionDto>();

		// Use this because name convention
		public List<ChildInstitutionDto> ChildInstitutions { get { return AcadChildOrganizationsDto; } }
	}
}
