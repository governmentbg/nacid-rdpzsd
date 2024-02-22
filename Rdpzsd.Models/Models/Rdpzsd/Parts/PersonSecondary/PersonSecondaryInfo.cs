using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary
{
    public class PersonSecondaryInfo: PartInfo
    {
        [Skip]
        public PersonSecondary PersonSecondary { get; set; }
    }

    public class PersonSecondaryInfoConfiguration : IEntityTypeConfiguration<PersonSecondaryInfo>
    {
        public void Configure(EntityTypeBuilder<PersonSecondaryInfo> builder)
        {
            builder.HasOne(p => p.PersonSecondary)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonSecondaryInfo>(p => p.Id);
        }
    }
}
