using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures.StudentStatus
{
    public class StudentEvent : Nomenclature, IIncludeAll<StudentEvent>
    {
        public int StudentStatusId { get; set; }
        [Skip]
        public StudentStatus StudentStatus { get; set; }

        public List<StudentEventQualification> StudentEventQualifications { get; set; } = new List<StudentEventQualification>();

        public IQueryable<StudentEvent> IncludeAll(IQueryable<StudentEvent> query)
        {
            return query
                .Include(e => e.StudentEventQualifications)
                    .ThenInclude(s => s.EducationalQualification)
                .Include(e => e.StudentStatus);
        }
    }
}
