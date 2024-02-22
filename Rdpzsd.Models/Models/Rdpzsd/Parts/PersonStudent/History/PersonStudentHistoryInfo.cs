using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentHistoryInfo : PartInfo
    {
        [Skip]
        public PersonStudentHistory PersonStudentHistory { get; set; }
    }

    public class PersonStudentHistoryInfoConfiguration : IEntityTypeConfiguration<PersonStudentHistoryInfo>
    {
        public void Configure(EntityTypeBuilder<PersonStudentHistoryInfo> builder)
        {
            builder.HasOne(p => p.PersonStudentHistory)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonStudentHistoryInfo>(p => p.Id);
        }
    }
}
