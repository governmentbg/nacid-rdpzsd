using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonImage : RdpzsdAttachedFile
    {
        [Skip]
        public PersonBasic PersonBasic { get; set; }
    }

    public class PersonImageConfiguration : IEntityTypeConfiguration<PersonImage>
    {
        public void Configure(EntityTypeBuilder<PersonImage> builder)
        {
            builder.HasOne(p => p.PersonBasic)
                   .WithOne(a => a.PersonImage)
                   .HasForeignKey<PersonImage>(p => p.Id);
        }
    }
}
