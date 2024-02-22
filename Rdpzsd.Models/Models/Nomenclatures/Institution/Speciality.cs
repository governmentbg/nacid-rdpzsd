using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Nomenclatures.Base;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class Speciality : NomenclatureCode
	{
		public int? ResearchAreaId { get; set; }
		[Skip]
		public ResearchArea ResearchArea { get; set; }

		public int EducationalQualificationId { get; set; }
		[Skip]
		public EducationalQualification EducationalQualification { get; set; }

		public bool IsRegulated { get; set; }
	}
}
