using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class ResearchArea : NomenclatureHierarchy, IIncludeAll<ResearchArea>
	{
		public string CodeNumber { get; set; }

		public IQueryable<ResearchArea> IncludeAll(IQueryable<ResearchArea> query)
		{
			return query;
		}
	}
}
