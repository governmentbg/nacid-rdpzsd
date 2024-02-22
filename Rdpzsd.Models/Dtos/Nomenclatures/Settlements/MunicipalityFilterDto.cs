using Infrastructure.User;
using Rdpzsd.Models.Models.Nomenclatures;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.Settlements
{
	public class MunicipalityFilterDto : NomenclatureCodeFilterDto<Municipality>
	{
		public int? DistrictId { get; set; }

		public override IQueryable<Municipality> WhereBuilder(IQueryable<Municipality> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (DistrictId.HasValue)
			{
				query = query.Where(e => e.DistrictId == DistrictId);
			}

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}
	}
}
