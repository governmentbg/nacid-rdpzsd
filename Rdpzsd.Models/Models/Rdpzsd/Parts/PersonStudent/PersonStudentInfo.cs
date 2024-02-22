using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonStudentInfo : PartInfo
    {
        [Skip]
        public PersonStudent PersonStudent { get; set; }
    }

    public class PersonStudentInfoConfiguration : IEntityTypeConfiguration<PersonStudentInfo>
    {
        public void Configure(EntityTypeBuilder<PersonStudentInfo> builder)
        {
            builder.HasOne(p => p.PersonStudent)
                   .WithOne(a => a.PartInfo)
                   .HasForeignKey<PersonStudentInfo>(p => p.Id);
        }
    }
}
