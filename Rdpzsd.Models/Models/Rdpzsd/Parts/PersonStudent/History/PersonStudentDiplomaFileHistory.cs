using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentDiplomaFileHistory : RdpzsdAttachedFile
    {
        [Skip]
        public PersonStudentDiplomaHistory Diploma { get; set; }
    }

    public class PersonStudentDiplomaFileHistoryConfiguration : IEntityTypeConfiguration<PersonStudentDiplomaFileHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDiplomaFileHistory> builder)
        {
            builder.HasOne(p => p.Diploma)
                   .WithOne(a => a.File)
                   .HasForeignKey<PersonStudentDiplomaFileHistory>(p => p.Id);
        }
    }
}
