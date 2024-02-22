using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonStudentDuplicateDiplomaFile : RdpzsdAttachedFile
    {
        [Skip]
        public PersonStudentDuplicateDiploma DuplicateDiploma { get; set; }
    }

    public class PersonStudentDuplicateDiplomaFileConfiguration : IEntityTypeConfiguration<PersonStudentDuplicateDiplomaFile>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDuplicateDiplomaFile> builder)
        {
            builder.HasOne(p => p.DuplicateDiploma)
                   .WithOne(a => a.File)
                   .HasForeignKey<PersonStudentDuplicateDiplomaFile>(p => p.Id);
        }
    }
}
