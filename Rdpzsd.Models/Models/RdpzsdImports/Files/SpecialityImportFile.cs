using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class SpecialityImportFile : RdpzsdAttachedFile
    {
        [Skip]
        public SpecialityImport SpecialityImport { get; set; }
    }

    public class SpecialityImportFileConfiguration : IEntityTypeConfiguration<SpecialityImportFile>
    {
        public void Configure(EntityTypeBuilder<SpecialityImportFile> builder)
        {
            builder.HasOne(p => p.SpecialityImport)
                   .WithOne(a => a.ImportFile)
                   .HasForeignKey<SpecialityImportFile>(p => p.Id);
        }
    }
}
