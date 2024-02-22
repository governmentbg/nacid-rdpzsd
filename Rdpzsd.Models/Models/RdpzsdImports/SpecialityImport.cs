using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.RdpzsdImports.Base;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using System.Linq;

namespace Rdpzsd.Models.Models.RdpzsdImports
{
    public class SpecialityImport : RdpzsdImport<SpecialityImportFile, SpecialityImportErrorFile, SpecialityImportHistory, SpecialityImportHistoryFile, SpecialityImportHistoryErrorFile>, IIncludeAll<SpecialityImport>
    {
        public IQueryable<SpecialityImport> IncludeAll(IQueryable<SpecialityImport> query)
        {
            return query
                .Include(e => e.ImportFile)
                .Include(e => e.Institution)
                .Include(e => e.Subordinate)
                .Include(e => e.ErrorFile)
                .Include(e => e.ImportHistories)
                    .ThenInclude(s => s.ImportFile)
                .Include(e => e.ImportHistories)
                    .ThenInclude(s => s.ErrorFile);
        }
    }

    public class SpecialityImportConfiguration : IEntityTypeConfiguration<SpecialityImport>
    {
        public void Configure(EntityTypeBuilder<SpecialityImport> builder)
        {
            builder.HasMany(e => e.ImportHistories)
                .WithOne()
                .HasForeignKey(e => e.RdpzsdImportId);
        }
    }
}
