using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class SpecialityImportErrorFile : RdpzsdAttachedFile
    {
        [Skip]
        public SpecialityImport SpecialityImport { get; set; }
    }

    public class SpecialityImportErrorFileConfiguration : IEntityTypeConfiguration<SpecialityImportErrorFile>
    {
        public void Configure(EntityTypeBuilder<SpecialityImportErrorFile> builder)
        {
            builder.HasOne(p => p.SpecialityImport)
                   .WithOne(a => a.ErrorFile)
                   .HasForeignKey<SpecialityImportErrorFile>(p => p.Id);
        }
    }
}
