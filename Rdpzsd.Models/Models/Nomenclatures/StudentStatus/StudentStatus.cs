using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures.StudentStatus
{
    public class StudentStatus : Nomenclature, IIncludeAll<StudentStatus>
    {
        public IQueryable<StudentStatus> IncludeAll(IQueryable<StudentStatus> query)
        {
            return query;
        }
    }
}
