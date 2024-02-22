using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonDoctoralHistoryInfo : PartInfo
    {
        [Skip]
        public PersonDoctoralHistory PersonDoctoralHistory { get; set; }
    }

    public class PersonDoctoralHistoryInfoConfiguration : IEntityTypeConfiguration<PersonDoctoralHistoryInfo>
    {
        public void Configure(EntityTypeBuilder<PersonDoctoralHistoryInfo> builder)
        {
            builder.HasOne(p => p.PersonDoctoralHistory)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonDoctoralHistoryInfo>(p => p.Id);
        }
    }
}
