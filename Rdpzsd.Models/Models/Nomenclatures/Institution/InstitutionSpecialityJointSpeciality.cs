using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class InstitutionSpecialityJointSpeciality : EntityVersion
	{
		public int InstitutionSpecialityId { get; set; }
		public OrganizationSpecialityJointSpecialityLocation Location { get; set; }
		public int? InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }
		public int? InstitutionByParentId { get; set; }
		[Skip]
		public Institution InstitutionByParent { get; set; }
		public string ForeignInstitutionName { get; set; }
		public string ForeignInstitutionByParentName { get; set; }
	}
	public enum OrganizationSpecialityJointSpecialityLocation
	{
		Bulgaria = 1,
		Abroad = 2
	}

}
