using Infrastructure.User;
using Rdpzsd.Models.Models.Nomenclatures;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.Settlements
{
	public class SettlementFilterDto : NomenclatureCodeFilterDto<Settlement>
	{
		public int? DistrictId { get; set; }
		public int? MunicipalityId { get; set; }

		public override IQueryable<Settlement> WhereBuilder(IQueryable<Settlement> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (DistrictId.HasValue)
			{
				query = query.Where(e => e.DistrictId == DistrictId);
			}

			if (MunicipalityId.HasValue)
			{
				query = query.Where(e => e.MunicipalityId == MunicipalityId);
			}

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}
	}
}
