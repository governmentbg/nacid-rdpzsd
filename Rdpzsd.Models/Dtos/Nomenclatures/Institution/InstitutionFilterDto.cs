using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Models.Nomenclatures;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures
{
    public class InstitutionFilterDto : NomenclatureHierarchyFilterDto<Institution>
    {
        public string Uic { get; set; }
        public int? LotNumber { get; set; }
        public OrganizationType? OrganizationType { get; set; }
        public List<OrganizationType> OrganizationTypes { get; set; } = new List<OrganizationType>();
        public OwnershipType? OwnershipType { get; set; }
        public int? SettlementId { get; set; }
        public int? MunicipalityId { get; set; }
        public int? DistrictId { get; set; }

		// When RSD user it will filter institutions
		public bool GetInstitutionsByPermissions { get; set; } = false;
		
		//Filtering for joint specialities
		public int? InstitutionSpecialityId { get; set; }
		public bool GetJointSpecialityInstitutions { get; set; } = false;
		public List<int?> InstitutionIds { get; set; } = new List<int?>();

		public override IQueryable<Institution> WhereBuilder(IQueryable<Institution> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
			if (GetJointSpecialityInstitutions && InstitutionSpecialityId.HasValue)
			{
				var institutionSpeciality = rdpzsdDbContext
					.InstitutionSpecialities
					.AsNoTracking()
					.Include(e => e.InstitutionSpecialityJointSpecialities)
					.Include(e => e.Institution)
					.Single(e => e.Id == InstitutionSpecialityId);

				InstitutionIds = institutionSpeciality.InstitutionSpecialityJointSpecialities.Select(e => e.InstitutionId).Append(institutionSpeciality.Institution.RootId).Distinct().ToList();
			}

			if (!string.IsNullOrWhiteSpace(Uic))
			{
				var textFilter = $"%{Uic.Trim().ToLower()}%";
				query = query.Where(e => EF.Functions.ILike(e.Uic, textFilter));
			}

			if (LotNumber.HasValue)
			{
				query = query.Where(e => e.LotNumber == LotNumber);
			}

			if (OrganizationType.HasValue)
			{
				query = query.Where(e => e.OrganizationType == OrganizationType);
			}

			if (OrganizationTypes != null && OrganizationTypes.Any())
			{
				query = query.Where(e => OrganizationTypes.Contains(e.OrganizationType.Value));
			}

			if (OwnershipType.HasValue)
			{
				query = query.Where(e => e.OwnershipType == OwnershipType);
			}

			if (SettlementId.HasValue)
			{
				query = query.Where(e => e.SettlementId == SettlementId);
			}

			if (MunicipalityId.HasValue)
			{
				query = query.Where(e => e.MunicipalityId == MunicipalityId);
			}

			if (DistrictId.HasValue)
			{
				query = query.Where(e => e.DistrictId == DistrictId);
			}

            if (GetInstitutionsByPermissions && userContext.UserType == UserType.Rsd)
            {
				RootId = userContext.Institution.Id;

                if (Level == Enums.Level.Second && userContext.Institution.ChildInstitutions.Any())
                {
					var subordinateIds = userContext.Institution.ChildInstitutions.Select(e => e.Id).ToList();

					query = query.Where(e => subordinateIds.Contains(e.Id));
				}
            }
			if (InstitutionIds.Any())
			{
				query = query.Where(e => InstitutionIds.Contains(e.Id));
			}
			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}

		public override IQueryable<Institution> ConstructTextFilter(IQueryable<Institution> query)
		{
			if (!string.IsNullOrWhiteSpace(TextFilter))
			{
				var textFilter = $"{TextFilter.Trim().ToLower()}";
				query = query.Where(e => (e.Code.Trim().ToLower() + " " + e.Name.Trim().ToLower()).Contains(textFilter)
					|| (e.Code.Trim().ToLower() + " " + e.NameAlt.Trim().ToLower()).Contains(textFilter)
					|| e.ShortName.Trim().ToLower().Contains(textFilter));
			}

			return query;
		}
	}
}
