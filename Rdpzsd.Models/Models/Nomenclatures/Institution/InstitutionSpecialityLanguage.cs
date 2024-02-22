using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class InstitutionSpecialityLanguage : EntityVersion
	{
		public int InstitutionSpecialityId { get; set; }
		public int? LanguageId { get; set; }
		[Skip]
		public Language Language { get; set; }
	}
}
