using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class EducationalForm : Nomenclature, IIncludeAll<EducationalForm>
	{
		public int? DataUniExternalId { get; set; }

		public IQueryable<EducationalForm> IncludeAll(IQueryable<EducationalForm> query)
		{
			return query;
		}
	}
}
