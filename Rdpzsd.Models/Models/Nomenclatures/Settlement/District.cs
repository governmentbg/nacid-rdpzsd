using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class District : NomenclatureCode, IIncludeAll<District>
	{
		public string Code2 { get; set; }
		public string SecondLevelRegionCode { get; set; }
		public string MainSettlementCode { get; set; }

		public IQueryable<District> IncludeAll(IQueryable<District> query)
		{
			return query;
		}
	}
}
