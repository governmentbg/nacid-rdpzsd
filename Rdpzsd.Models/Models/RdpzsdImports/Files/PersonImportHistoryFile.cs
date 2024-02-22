using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class PersonImportHistoryFile : RdpzsdAttachedFile
    {
        [Skip]
        public PersonImportHistory PersonImportHistory { get; set; }
    }

    public class PersonImportHistoryFileConfiguration : IEntityTypeConfiguration<PersonImportHistoryFile>
    {
        public void Configure(EntityTypeBuilder<PersonImportHistoryFile> builder)
        {
            builder.HasOne(p => p.PersonImportHistory)
                   .WithOne(a => a.ImportFile)
                   .HasForeignKey<PersonImportHistoryFile>(p => p.Id);
        }
    }
}
