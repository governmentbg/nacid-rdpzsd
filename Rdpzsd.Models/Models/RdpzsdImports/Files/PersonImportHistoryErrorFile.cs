using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.RdpzsdImports.Files
{
    public class PersonImportHistoryErrorFile : RdpzsdAttachedFile
    {
        [Skip]
        public PersonImportHistory PersonImportHistory { get; set; }
    }

    public class PersonImportHistoryErrorFileConfiguration : IEntityTypeConfiguration<PersonImportHistoryErrorFile>
    {
        public void Configure(EntityTypeBuilder<PersonImportHistoryErrorFile> builder)
        {
            builder.HasOne(p => p.PersonImportHistory)
                   .WithOne(a => a.ErrorFile)
                   .HasForeignKey<PersonImportHistoryErrorFile>(p => p.Id);
        }
    }
}
