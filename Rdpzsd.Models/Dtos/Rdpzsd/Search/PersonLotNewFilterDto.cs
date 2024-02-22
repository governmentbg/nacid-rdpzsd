using Infrastructure.Constants;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Search;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonLotNewFilterDto : FilterDto, IWhere<PersonLot>
    {
        public PersonLotNewFilterType FilterType { get; set; } = PersonLotNewFilterType.IdentificationNumber;
        // Get's Persons created by AR organization and not mapped (without speciality and doctoral parts)
        public bool ShowNotMapped { get; set; } = true;

        public string Uan { get; set; }
        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }

        public int? BirthCountryId { get; set; }
        public DateTime? BirthDate { get; set; }

        public IQueryable<PersonLot> WhereBuilder(IQueryable<PersonLot> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = query.Where(e => e.State == LotState.Actual 
                    || e.State == LotState.Erased
                    || (userContext.UserType == UserType.Rsd && (e.State == LotState.PendingApproval || e.State == LotState.CancelApproval || e.State == LotState.MissingPassportCopy)));

            if (FilterType == PersonLotNewFilterType.IdentificationNumber)
            {
                if (ShowNotMapped)
                {
                    query = ConstructNotMappedPersons(query, userContext);
                }
                else if (!string.IsNullOrWhiteSpace(Uan) || !string.IsNullOrWhiteSpace(Uin) || !string.IsNullOrWhiteSpace(ForeignerNumber))
                {
                    if (!string.IsNullOrWhiteSpace(Uan))
                    {
                        query = query.Where(e => e.Uan == Uan.Trim().ToUpper());
                    }

                    if (!string.IsNullOrWhiteSpace(Uin))
                    {
                        query = query.Where(e => e.PersonBasic.Uin == Uin.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(ForeignerNumber))
                    {
                        query = query.Where(e => e.PersonBasic.ForeignerNumber == ForeignerNumber.Trim());
                    }
                }
                else
                {
                    query = query.Where(e => false);
                }
            }
            else
            {
                if (ShowNotMapped)
                {
                    query = ConstructNotMappedPersons(query, userContext);
                }
                else
                {
                    query = query.Where(e => e.PersonBasic.BirthDate.Date == BirthDate.Value.Date && e.PersonBasic.BirthCountryId == BirthCountryId);
                }
            }

            return query;
        }

        private IQueryable<PersonLot> ConstructNotMappedPersons(IQueryable<PersonLot> query, UserContext userContext)
        {
            query = query.Where(e => e.State != LotState.Erased);

            query = query.Where(e => !e.PersonStudents.Any(s => s.State != PartState.Erased) && !e.PersonDoctorals.Any(s => s.State != PartState.Erased)
                && (FilterType == PersonLotNewFilterType.BirthPlace 
                    ? (e.PersonBasic.Uin == null && e.PersonBasic.ForeignerNumber == null && e.PersonBasic.IdnNumber != null)
                    : (e.PersonBasic.Uin != null || e.PersonBasic.ForeignerNumber != null)));

            if (userContext.UserType == UserType.Rsd)
            {
                if (userContext.Institution.ChildInstitutions.Count == 1)
                {
                    var userSubordinateId = userContext.Institution.ChildInstitutions.First().Id;

                    query = query.Where(e => (e.CreateSubordinateId.HasValue && userSubordinateId == e.CreateSubordinateId.Value)
                        || e.PersonBasic.PartInfo.SubordinateId == userSubordinateId);
                }
                else
                {
                    query = query.Where(e => (e.CreateInstitutionId.HasValue && userContext.Institution.Id == e.CreateInstitutionId.Value) 
                        || e.PersonBasic.PartInfo.InstitutionId == userContext.Institution.Id);
                }
            }

            return query;
        }
    }
}
