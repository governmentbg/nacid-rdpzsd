using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.RdpzsdImports.Base;
using Rdpzsd.Models.Models.RdpzsdImports.Collections;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.RdpzsdImports
{
    public class PersonImport : RdpzsdImport<PersonImportFile, PersonImportErrorFile, PersonImportHistory, PersonImportHistoryFile, PersonImportHistoryErrorFile>, IIncludeAll<PersonImport>
    {
        public List<PersonImportUan> PersonImportUans { get; set; } = new List<PersonImportUan>();

        public IQueryable<PersonImport> IncludeAll(IQueryable<PersonImport> query)
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

    public class PersonImportConfiguration : IEntityTypeConfiguration<PersonImport>
    {
        public void Configure(EntityTypeBuilder<PersonImport> builder)
        {
            builder.HasMany(e => e.ImportHistories)
                .WithOne()
                .HasForeignKey(e => e.RdpzsdImportId);

            builder.HasMany(e => e.PersonImportUans)
                .WithOne()
                .HasForeignKey(e => e.PersonImportId);
        }
    }
}
