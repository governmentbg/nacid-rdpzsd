using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class SpecialityImportHistoryErrorFile : RdpzsdAttachedFile
    {
        [Skip]
        public SpecialityImportHistory SpecialityImportHistory { get; set; }
    }

    public class SpecialityImportHistoryErrorFileConfiguration : IEntityTypeConfiguration<SpecialityImportHistoryErrorFile>
    {
        public void Configure(EntityTypeBuilder<SpecialityImportHistoryErrorFile> builder)
        {
            builder.HasOne(p => p.SpecialityImportHistory)
                   .WithOne(a => a.ErrorFile)
                   .HasForeignKey<SpecialityImportHistoryErrorFile>(p => p.Id);
        }
    }
}
