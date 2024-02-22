using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    // This is not needed to have PersonStudentStickerNoteHistory table
    public class PersonStudentStickerNote : EntityVersion
    {
        public int PartId { get; set; }

        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public DateTime ActionDate { get; set; }
        public int? InstitutionId { get; set; }
        [Skip]
        public Institution Institution { get; set; }
        public int? SubordinateId { get; set; }
        [Skip]
        public Institution Subordinate { get; set; }
        public string Note { get; set; }
    }

    public class PersonStudentStickerNoteConfiguration : IEntityTypeConfiguration<PersonStudentStickerNote>
    {
        public void Configure(EntityTypeBuilder<PersonStudentStickerNote> builder)
        {
            builder.Property(e => e.Note)
                .HasMaxLength(250);
        }
    }
}
