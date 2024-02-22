using Infrastructure.User;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.StudentStatus
{
    public class StudentEventFilterDto : NomenclatureFilterDto<StudentEvent>
    {
        public int? StudentStatusId { get; set; }
        public int? EducationalQualificationId { get; set; }

		public List<string> StudentEventAliases { get; set; } = new List<string>();

        public override IQueryable<StudentEvent> WhereBuilder(IQueryable<StudentEvent> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (StudentStatusId.HasValue)
			{
				query = query.Where(e => e.StudentStatusId == StudentStatusId);
			}

			if (EducationalQualificationId.HasValue)
			{
				query = query.Where(e => e.StudentEventQualifications.Any(s => s.EducationalQualificationId == EducationalQualificationId));
			}

            if (StudentEventAliases.Any())
            {
				query = query.Where(e => StudentEventAliases.Contains(e.Alias));
            }

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}
	}
}
