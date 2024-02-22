using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class Municipality : NomenclatureCode, IIncludeAll<Municipality>
	{
		public int DistrictId { get; set; }
		[Skip]
		public District District { get; set; }
		public string Code2 { get; set; }
		public string MainSettlementCode { get; set; }
		public string Category { get; set; }

		public IQueryable<Municipality> IncludeAll(IQueryable<Municipality> query)
		{
			return query
				.Include(e => e.District);
		}
	}
}
