using Infrastructure.Constants;
using Infrastructure.User;
using Rdpzsd.Models.Models.Nomenclatures;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures
{
	public class EducationalQualificationFilterDto : NomenclatureFilterDto<EducationalQualification>
	{
		public bool OnlyMasters { get; set; } = false;

        public override IQueryable<EducationalQualification> WhereBuilder(IQueryable<EducationalQualification> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
            if (OnlyMasters)
            {
				query = query.Where(e => e.Alias == EducationalQualificationConstants.MasterSecondary || e.Alias == EducationalQualificationConstants.MasterHigh);
            }

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}
	}
}
