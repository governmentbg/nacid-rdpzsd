using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class PersonImportErrorFile : RdpzsdAttachedFile
    {
        [Skip]
        public PersonImport PersonImport { get; set; }
    }

    public class PersonImportErrorFileConfiguration : IEntityTypeConfiguration<PersonImportErrorFile>
    {
        public void Configure(EntityTypeBuilder<PersonImportErrorFile> builder)
        {
            builder.HasOne(p => p.PersonImport)
                   .WithOne(a => a.ErrorFile)
                   .HasForeignKey<PersonImportErrorFile>(p => p.Id);
        }
    }
}
