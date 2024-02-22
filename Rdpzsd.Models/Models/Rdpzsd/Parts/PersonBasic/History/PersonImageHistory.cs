using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonImageHistory : RdpzsdAttachedFile
    {
        [Skip]
        public PersonBasicHistory PersonBasicHistory { get; set; }
    }

    public class PersonImageHistoryConfiguration : IEntityTypeConfiguration<PersonImageHistory>
    {
        public void Configure(EntityTypeBuilder<PersonImageHistory> builder)
        {
            builder.HasOne(p => p.PersonBasicHistory)
                   .WithOne(a => a.PersonImage)
                   .HasForeignKey<PersonImageHistory>(p => p.Id);
        }
    }
}
