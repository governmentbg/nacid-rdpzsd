using Infrastructure.User;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonApprovalFilterDto : FilterDto, IWhere<PersonLot>
    {
        public int? CreateInstitutionId { get; set; }
        public int? CreateSubordinateId { get; set; }

        public ApprovalState? State { get; set; }

        public IQueryable<PersonLot> WhereBuilder(IQueryable<PersonLot> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = query.Where(e => e.State == LotState.PendingApproval || e.State == LotState.CancelApproval || e.State == LotState.MissingPassportCopy);

            if (CreateInstitutionId.HasValue)
            {
                query = query.Where(e => e.CreateInstitutionId == CreateInstitutionId);
            }

            if (CreateSubordinateId.HasValue)
            {
                query = query.Where(e => e.CreateSubordinateId == CreateSubordinateId);
            }

            if (State.HasValue)
            {
                query = query.Where(e => State == ApprovalState.Pending 
                    ? e.State == LotState.PendingApproval 
                    : State == ApprovalState.MissingPassportCopy
                        ? e.State == LotState.MissingPassportCopy
                        : e.State == LotState.CancelApproval);
            }

            return query;
        }
    }
}
