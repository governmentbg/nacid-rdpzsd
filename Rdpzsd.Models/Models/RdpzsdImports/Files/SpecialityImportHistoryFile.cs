using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class SpecialityImportHistoryFile : RdpzsdAttachedFile
    {
        [Skip]
        public SpecialityImportHistory SpecialityImportHistory { get; set; }
    }

    public class SpecialityImportHistoryFileConfiguration : IEntityTypeConfiguration<SpecialityImportHistoryFile>
    {
        public void Configure(EntityTypeBuilder<SpecialityImportHistoryFile> builder)
        {
            builder.HasOne(p => p.SpecialityImportHistory)
                   .WithOne(a => a.ImportFile)
                   .HasForeignKey<SpecialityImportHistoryFile>(p => p.Id);
        }
    }
}
