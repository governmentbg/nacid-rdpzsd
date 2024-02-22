using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PassportCopyHistory : RdpzsdAttachedFile
    {
        [Skip]
        public PersonBasicHistory PersonBasicHistory { get; set; }
    }

    public class PersonCopyHistoryConfiguration : IEntityTypeConfiguration<PassportCopyHistory>
    {
        public void Configure(EntityTypeBuilder<PassportCopyHistory> builder)
        {
            builder.HasOne(p => p.PersonBasicHistory)
                   .WithOne(a => a.PassportCopy)
                   .HasForeignKey<PassportCopyHistory>(p => p.Id);
        }
    }
}
