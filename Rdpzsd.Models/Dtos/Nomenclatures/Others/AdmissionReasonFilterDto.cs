using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Enums.Nomenclature.AdmissionReason;
using Rdpzsd.Models.Enums.Nomenclature.Country;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures.Others
{
    public class AdmissionReasonFilterDto : NomenclatureFilterDto<AdmissionReason>
    {
        public string OldCode { get; set; }
        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }
        public bool? EuCountry { get; set; }
        public bool? EeaCountry { get; set; }

        public int? CitizenshipId { get; set; }
        public int? SecondCitizenshipId { get; set; }

        public AdmissionReasonStudentType? StudentType { get; set; }

        public override IQueryable<AdmissionReason> WhereBuilder(IQueryable<AdmissionReason> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            if (!string.IsNullOrWhiteSpace(OldCode))
            {
                var textFilter = $"%{OldCode}%";
                query = query.Where(e => EF.Functions.ILike(e.OldCode, textFilter));
            }

            if (!string.IsNullOrWhiteSpace(ShortName))
            {
                var textFilter = $"%{ShortName.Trim().ToLower()}%";
                query = query.Where(e => EF.Functions.ILike(e.ShortName.Trim().ToLower(), textFilter));
            }

            if (!string.IsNullOrWhiteSpace(ShortNameAlt))
            {
                var textFilter = $"%{ShortNameAlt.Trim().ToLower()}%";
                query = query.Where(e => EF.Functions.ILike(e.ShortNameAlt.Trim().ToLower(), textFilter));
            }

            if (StudentType.HasValue)
            {
                if (StudentType == AdmissionReasonStudentType.Students)
                {
                    query = query.Where(e => e.AdmissionReasonStudentType != AdmissionReasonStudentType.Doctorals);
                }

                else if (StudentType == AdmissionReasonStudentType.Doctorals)
                {
                    query = query.Where(e => e.AdmissionReasonStudentType != AdmissionReasonStudentType.Students);
                }
            }

            if (EuCountry.HasValue && EeaCountry.HasValue)
            {
                //TODO Optimize query add Construct method
                if (EeaCountry.Value)
                {
                    query = query
                        .Where(e => e.CountryUnion == CountryUnion.EuAndEea ||
                                    (e.CountryUnion == CountryUnion.EuAndEea &&
                                    e.AdmissionReasonCitizenships.Any(e => !e.ExcludeCountry.Value && (e.CountryId == CitizenshipId || e.CountryId == SecondCitizenshipId))) ||
                                    (e.CountryUnion == null &&
                                    (e.AdmissionReasonCitizenships.Any(e => !e.ExcludeCountry.Value && (e.CountryId == CitizenshipId || e.CountryId == SecondCitizenshipId)) ||
                                    !e.AdmissionReasonCitizenships.Any())));
                }
                else if (!EuCountry.Value && !EeaCountry.Value)
                {
                    query = query
                        .Where(e => e.CountryUnion == CountryUnion.OtherCountries ||
                                   (e.CountryUnion == CountryUnion.OtherCountries &&
                                   (e.AdmissionReasonCitizenships.Any(e => !e.ExcludeCountry.Value && (e.CountryId == CitizenshipId || e.CountryId == SecondCitizenshipId)) ||
                                    e.AdmissionReasonCitizenships.Any(e => e.ExcludeCountry.Value && e.CountryId != CitizenshipId && e.CountryId != SecondCitizenshipId))) ||
                                    e.CountryUnion == null &&
                                    (e.AdmissionReasonCitizenships.Any(e => !e.ExcludeCountry.Value && (e.CountryId == CitizenshipId || e.CountryId == SecondCitizenshipId)) ||
                                    e.AdmissionReasonCitizenships.All(e => e.ExcludeCountry.Value && e.CountryId != CitizenshipId && e.CountryId != SecondCitizenshipId) ||
                                    !e.AdmissionReasonCitizenships.Any()));
                }
            }

            return base.WhereBuilder(query, userContext, rdpzsdDbContext);
        }

        public override IQueryable<AdmissionReason> ConstructTextFilter(IQueryable<AdmissionReason> query)
        {
            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => textFilter.Length < 3 
                    ? e.Id.ToString() == textFilter
                    : (e.Id + " - " + e.Name.Trim().ToLower()).Contains(textFilter)
                        || (e.Id + " - " + e.NameAlt.Trim().ToLower()).Contains(textFilter));
            }

            return query;
        }

        public override IQueryable<AdmissionReason> OrderBuilder(IQueryable<AdmissionReason> query)
        {
            return query
                .OrderByDescending(e => e.IsActive)
                .ThenBy(e => e.Id)
                .ThenBy(e => e.ViewOrder)
                .ThenBy(e => e.Name);
        }
    }
}
