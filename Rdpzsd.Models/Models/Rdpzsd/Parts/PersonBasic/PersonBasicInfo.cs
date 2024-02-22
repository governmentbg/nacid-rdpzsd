using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonBasicInfo : PartInfo
    {
        [Skip]
        public PersonBasic PersonBasic { get; set; }
    }

    public class PersonBasicInfoConfiguration : IEntityTypeConfiguration<PersonBasicInfo>
    {
        public void Configure(EntityTypeBuilder<PersonBasicInfo> builder)
        {
            builder.HasOne(p => p.PersonBasic)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonBasicInfo>(p => p.Id);
        }
    }
}
