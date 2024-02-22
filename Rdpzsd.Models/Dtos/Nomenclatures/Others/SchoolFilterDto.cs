using Infrastructure.User;
using Rdpzsd.Models.Enums.Nomenclature;
using Rdpzsd.Models.Enums.Nomenclature.School;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.Others
{
    public class SchoolFilterDto : NomenclatureFilterDto<School>
    {
        public SchoolState? State { get; set; }
		public SchoolType? Type { get; set; }
		public SchoolOwnershipType? OwnershipType { get; set; }

		public int? SettlementId { get; set; }
		public int? MunicipalityId { get; set; }
		public int? DistrictId { get; set; }

		public override IQueryable<School> WhereBuilder(IQueryable<School> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (State.HasValue)
			{
				query = query.Where(e => e.State == State);
			}

			if (Type.HasValue)
			{
				query = query.Where(e => e.Type == Type);
			}

			if (OwnershipType.HasValue)
			{
				query = query.Where(e => e.OwnershipType == OwnershipType);
			}

			if (DistrictId.HasValue)
			{
				query = query.Where(e => e.DistrictId == DistrictId);
			}

			if (MunicipalityId.HasValue)
			{
				query = query.Where(e => e.MunicipalityId == MunicipalityId);
			}

			if (SettlementId.HasValue)
			{
				query = query.Where(e => e.SettlementId == SettlementId);
			}

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}

        public override IQueryable<School> OrderBuilder(IQueryable<School> query)
        {
			return query
			  .OrderBy(e => e.Id)
			  .ThenBy(e => e.ViewOrder)
			  .ThenBy(e => e.Name);
		}
    }
}
