using Infrastructure.User;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.Others
{
    public class PeriodFilterDto : NomenclatureFilterDto<Period>
    {
        public int? Year { get; set; }
        public Semester? Semester { get; set; }

        public bool ExcludePeriodBefore { get; set; } = false;

        public override IQueryable<Period> WhereBuilder(IQueryable<Period> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            if (ExcludePeriodBefore && Year.HasValue && Semester.HasValue)
            {
                if (Semester == Enums.Semester.Second)
                {
                    query = query.Where(e => (e.Year == Year && e.Semester == Semester) || e.Year > Year);
                }
                else
                {
                    query = query.Where(e => e.Year == Year || e.Year > Year);
                }
            }
            else
            {
                if (Year.HasValue)
                {
                    query = query.Where(e => e.Year == Year);
                }

                if (Semester.HasValue)
                {
                    query = query.Where(e => e.Semester == Semester);
                }
            }

            return base.WhereBuilder(query, userContext, rdpzsdDbContext);
        }

        public override IQueryable<Period> OrderBuilder(IQueryable<Period> query)
        {
            return query
                .OrderByDescending(e => e.Year)
                .ThenByDescending(e => e.Semester)
                .ThenByDescending(e => e.Id);
        }
    }
}
