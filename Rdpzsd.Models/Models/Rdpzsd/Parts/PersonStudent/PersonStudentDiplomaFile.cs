using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonStudentDiplomaFile : RdpzsdAttachedFile
    {
        [Skip]
        public PersonStudentDiploma Diploma { get; set; }
    }

    public class PersonStudentDiplomaFileConfiguration : IEntityTypeConfiguration<PersonStudentDiplomaFile>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDiplomaFile> builder)
        {
            builder.HasOne(p => p.Diploma)
                   .WithOne(a => a.File)
                   .HasForeignKey<PersonStudentDiplomaFile>(p => p.Id);
        }
    }
}
