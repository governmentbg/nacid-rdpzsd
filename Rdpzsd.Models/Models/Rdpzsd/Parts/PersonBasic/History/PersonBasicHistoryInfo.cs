using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonBasicHistoryInfo : PartInfo
    {
        [Skip]
        public PersonBasicHistory PersonBasicHistory { get; set; }
    }

    public class PersonBasicHistoryInfoConfiguration : IEntityTypeConfiguration<PersonBasicHistoryInfo>
    {
        public void Configure(EntityTypeBuilder<PersonBasicHistoryInfo> builder)
        {
            builder.HasOne(p => p.PersonBasicHistory)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonBasicHistoryInfo>(p => p.Id);
        }
    }
}
