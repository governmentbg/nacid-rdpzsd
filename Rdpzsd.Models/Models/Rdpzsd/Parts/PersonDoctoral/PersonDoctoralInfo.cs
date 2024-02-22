using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonDoctoralInfo : PartInfo
    {
        [Skip]
        public PersonDoctoral PersonDoctoral { get; set; }
    }

    public class PersonDoctoralInfoConfiguration : IEntityTypeConfiguration<PersonDoctoralInfo>
    {
        public void Configure(EntityTypeBuilder<PersonDoctoralInfo> builder)
        {
            builder.HasOne(p => p.PersonDoctoral)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonDoctoralInfo>(p => p.Id);
        }
    }
}
