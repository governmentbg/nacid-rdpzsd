using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Nomenclature.Institution;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
    public class Institution : NomenclatureHierarchy, IIncludeAll<Institution>
	{
		public int LotNumber { get; set; }
		public InstitutionCommitState? State { get; set; }

		[Skip]
		public Institution Parent { get; set; }
		[Skip]
		public Institution Root { get; set; }

		public string Uic { get; set; }
		public string ShortName { get; set; }
		public string ShortNameAlt { get; set; }

		public OrganizationType? OrganizationType { get; set; }
		public OwnershipType? OwnershipType { get; set; }

		public int? SettlementId { get; set; }
		[Skip]
		public Settlement Settlement { get; set; }
		public int? MunicipalityId { get; set; }
		[Skip]
		public Municipality Municipality { get; set; }
		public int? DistrictId { get; set; }
		[Skip]
		public District District { get; set; }

		public bool IsResearchUniversity { get; set; }

		public List<InstitutionSpeciality> InstitutionSpecialities { get; set; } = new List<InstitutionSpeciality>();

		public IQueryable<Institution> IncludeAll(IQueryable<Institution> query)
		{
			return query
				.Include(e => e.District)
				.Include(e => e.Municipality)
				.Include(e => e.Settlement)
				.Include(e => e.Parent)
				.Include(e => e.Root);
		}
	}

	public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
	{
		public void Configure(EntityTypeBuilder<Institution> builder)
		{
			builder.HasOne(e => e.Parent)
				.WithMany()
				.HasForeignKey(e => e.ParentId);

			builder.HasOne(e => e.Root)
				.WithMany()
				.HasForeignKey(e => e.RootId);
		}
	}
}
