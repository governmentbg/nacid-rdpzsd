using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Nomenclature;
using Rdpzsd.Models.Enums.Nomenclature.School;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class School : Nomenclature, IIncludeAll<School>, IValidate
    {
        public SchoolState State { get; set; }
        public SchoolType Type { get; set; }
        public SchoolOwnershipType OwnershipType { get; set; }

        public int SettlementId { get; set; }
        [Skip]
        public Settlement Settlement { get; set; }
        public int MunicipalityId { get; set; }
        [Skip]
        public Municipality Municipality { get; set; }
        public int DistrictId { get; set; }
        [Skip]
        public District District { get; set; }
        public int? MigrationId { get; set; }
        public int? ParentId { get; set; }
        [Skip]
        public School Parent { get; set; }

        public IQueryable<School> IncludeAll(IQueryable<School> query)
        {
            return query
                .Include(e => e.District)
                .Include(e => e.Municipality)
                .Include(e => e.Settlement);
        }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            Name = Name?.Trim();

            if (!string.IsNullOrWhiteSpace(Name)
                && !ValidatePropertiesStatic.IsValidWithoutLatin(Name))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.School_NameCyrilic);
            }

            if (State == SchoolState.Renamed && !ParentId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.School_SchoolParentRequired);
            }
        }
    }
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.HasOne(e => e.Parent)
                .WithMany()
                .HasForeignKey(e => e.ParentId);

            builder.Property(e => e.Name)
             .HasMaxLength(255);
        }
    }
}
