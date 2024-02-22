using Infrastructure.User;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Parts
{
    public class PersonDoctoralFilterDto : FilterDto, IWhere<PersonDoctoral>
    {
        public int? LotId { get; set; }
        public int? ExcludeId { get; set; }
        public string StudentStatusAlias { get; set; }
        public string StudentEventAlias { get; set; }

        public IQueryable<PersonDoctoral> WhereBuilder(IQueryable<PersonDoctoral> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
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

            return query;
        }
    }
}
