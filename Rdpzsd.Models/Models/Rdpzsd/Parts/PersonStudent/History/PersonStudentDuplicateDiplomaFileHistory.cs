using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentDuplicateDiplomaFileHistory : RdpzsdAttachedFile
    {
        [Skip]
        public PersonStudentDuplicateDiplomaHistory Diploma { get; set; }
    }

    public class PersonStudentDuplicateDiplomaFileHistoryConfiguration : IEntityTypeConfiguration<PersonStudentDuplicateDiplomaFileHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDuplicateDiplomaFileHistory> builder)
        {
            builder.HasOne(p => p.Diploma)
                   .WithOne(a => a.File)
                   .HasForeignKey<PersonStudentDuplicateDiplomaFileHistory>(p => p.Id);
        }
    }
}
