using Infrastructure.Constants;
using Infrastructure.User;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Parts
{
    public class PersonStudentFilterDto : FilterDto, IWhere<PersonStudent>
    {
        public int? LotId { get; set; }
        public int? ExcludeId { get; set; }
        public string StudentStatusAlias { get; set; }
        public string StudentEventAlias { get; set; }
        public bool OnlyMasters { get; set; } = false;

        public IQueryable<PersonStudent> WhereBuilder(IQueryable<PersonStudent> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = query.Where(e => e.State != PartState.Erased);

            if (LotId.HasValue)
            {
                query = query.Where(e => e.LotId == LotId);
            }

            if (ExcludeId.HasValue)
            {
                query = query.Where(e => e.Id != ExcludeId);
            }

            if (!string.IsNullOrWhiteSpace(StudentStatusAlias))
            {
                query = query.Where(e => e.StudentStatus.Alias == StudentStatusAlias);
            }

            if (!string.IsNullOrWhiteSpace(StudentEventAlias))
            {
                query = query.Where(e => e.StudentEvent.Alias == StudentEventAlias);
            }

            if (OnlyMasters)
            {
                query = query.Where(e => e.InstitutionSpeciality.Speciality.EducationalQualification.Alias == EducationalQualificationConstants.MasterSecondary
                || e.InstitutionSpeciality.Speciality.EducationalQualification.Alias == EducationalQualificationConstants.MasterHigh);
            }

            return query;
        }
    }
}
