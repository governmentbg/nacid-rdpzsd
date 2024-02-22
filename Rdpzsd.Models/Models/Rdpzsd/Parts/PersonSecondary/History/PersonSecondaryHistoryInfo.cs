using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.PersonSecondary.History
{
    public class PersonSecondaryHistoryInfo: PartInfo
    {
        [Skip]
        public PersonSecondaryHistory PersonSecondaryHistory { get; set; }
    }

    public class PersonSecondaryHistoryInfoConfiguration : IEntityTypeConfiguration<PersonSecondaryHistoryInfo>
    {
        public void Configure(EntityTypeBuilder<PersonSecondaryHistoryInfo> builder)
        {
            builder.HasOne(p => p.PersonSecondaryHistory)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonSecondaryHistoryInfo>(p => p.Id);
        }
    }
}
