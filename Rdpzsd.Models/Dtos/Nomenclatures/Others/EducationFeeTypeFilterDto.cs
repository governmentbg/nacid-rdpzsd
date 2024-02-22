using Infrastructure.User;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.Others
{
    public class EducationFeeTypeFilterDto: NomenclatureFilterDto<EducationFeeType>
    {
        public int? AdmissionReasonId { get; set; }

        public override IQueryable<EducationFeeType> WhereBuilder(IQueryable<EducationFeeType> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
            if (AdmissionReasonId.HasValue)
            {
                query = query.Where(e => e.AdmissionReasonEducationFees.Any(e => e.AdmissionReasonId == AdmissionReasonId));
            }

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}

        public override IQueryable<EducationFeeType> OrderBuilder(IQueryable<EducationFeeType> query)
        {
            return query
                .OrderBy(e => e.Id)
                .ThenBy(e => e.ViewOrder)
                .ThenBy(e => e.Name);
        }
    }
}
