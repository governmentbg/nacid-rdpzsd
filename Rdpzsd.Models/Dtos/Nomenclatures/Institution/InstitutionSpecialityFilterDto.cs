using Infrastructure.Constants;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures
{
    public class InstitutionSpecialityFilterDto : FilterDto, IWhere<InstitutionSpeciality>
    {
        // Gets speciality in institution
        public int? InstitutionId { get; set; }
        // Gets specialities in institution and child subsructures
        public int? InstitutionRootId { get; set; }
        // If it has alias get all specialities with this EducationalQualification
        // If it is empty get all <> doctoral
        public string EducationalQualificationAlias { get; set; }
        // When RSD user it will filter speciality by permissions
        public bool GetInstitutionSpecialitiesByPermissions { get; set; } = false;
        public int? EducationalFormId { get; set; }
        public int? EducationalQualificationId { get; set; }
        public decimal? Duration { get; set; }

        public int? ResearchAreaId { get; set; }
        public bool? IsRegulated { get; set; }
        public bool? IsForCadets { get; set; }
        public string SpecialityName { get; set; }
        public string SpecialityCode { get; set; }
        public bool OnlyMasters { get; set; } = false;
        public bool ShowJointSpecialitiesOnly { get; set; } = false;
		public string SpecialityIdNumber { get; set; }

		public IQueryable<InstitutionSpeciality> WhereBuilder(IQueryable<InstitutionSpeciality> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            if (InstitutionId.HasValue)
            {
                query = query.Where(e => e.InstitutionId == InstitutionId);
            }

            if (EducationalFormId.HasValue)
            {
                query = query.Where(e => e.EducationalFormId == EducationalFormId);
            }

            if (EducationalQualificationId.HasValue)
            {
                query = query.Where(e => e.Speciality.EducationalQualificationId == EducationalQualificationId);
            }

            if (Duration.HasValue)
            {
                query = query.Where(e => e.Duration == Duration);
            }

            if (ResearchAreaId.HasValue)
            {
                query = query.Where(e => e.Speciality.ResearchAreaId == ResearchAreaId);
            }

            if (IsRegulated.HasValue)
            {
                query = query.Where(e => e.Speciality.IsRegulated == IsRegulated.Value);
            }

            if (IsForCadets.HasValue)
            {
                query = query.Where(e => e.IsForCadets == IsForCadets.Value);
            }

            if (!string.IsNullOrWhiteSpace(SpecialityCode))
            {
                var textFilter = $"%{SpecialityCode}%";
                query = query.Where(e => EF.Functions.ILike(e.Speciality.Code, textFilter));
            }

            if (!string.IsNullOrWhiteSpace(SpecialityName))
            {
                var textFilter = $"%{SpecialityName.Trim().ToLower()}%";
                query = query.Where(e => EF.Functions.ILike(e.Speciality.Name.Trim().ToLower(), textFilter));
            }

            if (!string.IsNullOrWhiteSpace(EducationalQualificationAlias))
            {
                query = query.Where(e => e.Speciality.EducationalQualification.Alias == EducationalQualificationAlias);
            }
            else
            {
                query = query.Where(e => e.Speciality.EducationalQualification.Alias != EducationalQualificationConstants.Doctor);
            }

            if (IsActive.HasValue)
            {
                if (IsActive.Value)
                {
                    query = query.Where(e => e.IsActive);
                }
                else
                {
                    query = query.Where(e => !e.IsActive);
                }
            }

            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => (e.Speciality.Code.Trim().ToLower() + " " + e.Speciality.Name.Trim().ToLower()).Contains(textFilter)
                    || (e.Speciality.Code.Trim().ToLower() + " " + e.Speciality.NameAlt.Trim().ToLower()).Contains(textFilter));
            }

            if (OnlyMasters)
            {
                query = query.Where(e => e.Speciality.EducationalQualification.Alias == EducationalQualificationConstants.MasterSecondary
                || e.Speciality.EducationalQualification.Alias == EducationalQualificationConstants.MasterHigh);
            }

            if (GetInstitutionSpecialitiesByPermissions && userContext.UserType == UserType.Rsd)
            {
                InstitutionRootId = userContext.Institution.Id;

                if (userContext.Institution.ChildInstitutions.Any())
                {
                    var subordinateIds = userContext.Institution.ChildInstitutions.Select(e => e.Id).ToList();

                    query = query.Where(e => subordinateIds.Contains(e.Institution.Id));
                }

                query = query.Where(e => userContext.UserEducationalQualificationPermissions.Contains(e.Speciality.EducationalQualification.Alias));
            }

            if (InstitutionRootId.HasValue)
            {
                query = query.Where(e => e.Institution.RootId == InstitutionRootId);
            }
            if (ShowJointSpecialitiesOnly)
			{
                query = query.Where(e => e.IsJointSpeciality.HasValue && e.IsJointSpeciality.Value && e.InstitutionSpecialityJointSpecialities.Any());
			}
            if (!string.IsNullOrWhiteSpace(SpecialityIdNumber))
			{
                int specialityId;
                if (int.TryParse(SpecialityIdNumber, out specialityId)) {
                    query = query.Where(e => e.Id == specialityId);
                }
            }
            return query;
        }
    }
}
