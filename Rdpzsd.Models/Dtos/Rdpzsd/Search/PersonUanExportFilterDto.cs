using Infrastructure.User;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Search;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonUanExportFilterDto : FilterDto, IWhere<PersonLot>
    {
        public PersonLotNewFilterType FilterType { get; set; } = PersonLotNewFilterType.IdentificationNumber;
        public string SearchType { get; set; }
        public string Uan { get; set; }
        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public int? BirthCountryId { get; set; }
        public DateTime? BirthDate { get; set; }

        public IQueryable<PersonLot> WhereBuilder(IQueryable<PersonLot> query, UserContext userContext, RdpzsdDbContext context)
        {
            query = ConstructSearchFilterType(query, userContext, SearchType);

            if (FilterType == PersonLotNewFilterType.IdentificationNumber)
            {
                if (!string.IsNullOrWhiteSpace(Uan) || !string.IsNullOrWhiteSpace(Uin) || !string.IsNullOrWhiteSpace(ForeignerNumber))
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
                    query = query.Where(e => !string.IsNullOrWhiteSpace(e.PersonBasic.Uin) || !string.IsNullOrWhiteSpace(e.PersonBasic.ForeignerNumber));
                }
            }
            else
            {
                if (BirthDate.HasValue || BirthCountryId.HasValue)
                {
                    if (BirthDate.HasValue)
                    {
                        query = query.Where(e => e.PersonBasic.BirthDate.Date == BirthDate.Value.Date);
                    }

                    if (BirthCountryId.HasValue)
                    {
                        query = query.Where(e => e.PersonBasic.BirthCountryId == BirthCountryId);
                    }
                }
                else
                {
                    query = query.Where(e => string.IsNullOrWhiteSpace(e.PersonBasic.Uin) && string.IsNullOrWhiteSpace(e.PersonBasic.ForeignerNumber));
                }
            }

            return query;
        }

        private IQueryable<PersonLot> ConstructSearchFilterType(IQueryable<PersonLot> query, UserContext userContext, string searchType)
        {
            if (userContext.Institution.ChildInstitutions.Count == 1)
            {
                var childInstitutionId = userContext.Institution.ChildInstitutions.Select(e => e.Id).First();

                query = query.Where(e => e.State == LotState.Actual &&
                                (searchType == "Students" ?
                                (e.PersonStudents.Any(s => s.SubordinateId.HasValue && childInstitutionId == s.SubordinateId.Value) || e.PersonLotIdNumbers.Any(e => e.SubordinateId == childInstitutionId)) :
                                (e.PersonDoctorals.Any(s => s.SubordinateId.HasValue && childInstitutionId == s.SubordinateId.Value) || e.PersonLotIdNumbers.Any(e => e.SubordinateId == childInstitutionId))));
            }
            else
            {
                query = query.Where(e => e.State == LotState.Actual &&
                                    (searchType == "Students" ?
                                      (e.PersonStudents.Any(s => s.InstitutionId == userContext.Institution.Id) || e.PersonLotIdNumbers.Any(e => e.InstitutionLotId == userContext.Institution.Id)) :
                                      (e.PersonDoctorals.Any(e => e.InstitutionId == userContext.Institution.Id) || e.PersonLotIdNumbers.Any(e => e.InstitutionLotId == userContext.Institution.Id))));
            }

            return query;
        }
    }
}
