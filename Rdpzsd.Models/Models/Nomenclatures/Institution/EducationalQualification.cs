using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class EducationalQualification : Nomenclature, IIncludeAll<EducationalQualification>
	{
		public int? DataUniExternalId { get; set; }

		public IQueryable<EducationalQualification> IncludeAll(IQueryable<EducationalQualification> query)
		{
			return query;
		}
	}
}
