using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.Collections
{
    public class PassportCopy : RdpzsdAttachedFile
	{
        [Skip]
        public PersonBasic PersonBasic { get; set; }
    }

    public class PassportCopyConfiguration : IEntityTypeConfiguration<PassportCopy>
    {
        public void Configure(EntityTypeBuilder<PassportCopy> builder)
        {
            builder.HasOne(p => p.PersonBasic)
                   .WithOne(a => a.PassportCopy)
                   .HasForeignKey<PassportCopy>(p => p.Id);
        }
    }
}
