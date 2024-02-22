using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Nomenclatures.StudentStatus
{
    public class StudentEventQualification : EntityVersion
    {
        public int StudentEventId { get; set; }
        public int EducationalQualificationId { get; set; }
        [Skip]
        public EducationalQualification EducationalQualification { get; set; }
    }
}
