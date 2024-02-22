using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class PersonImportFile : RdpzsdAttachedFile
    {
        [Skip]
        public PersonImport PersonImport { get; set; }
    }

    public class PersonImportFileConfiguration : IEntityTypeConfiguration<PersonImportFile>
    {
        public void Configure(EntityTypeBuilder<PersonImportFile> builder)
        {
            builder.HasOne(p => p.PersonImport)
                   .WithOne(a => a.ImportFile)
                   .HasForeignKey<PersonImportFile>(p => p.Id);
        }
    }
}
