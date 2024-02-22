using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentDuplicateDiplomaHistory : EntityVersion
    {
        public int PartId { get; set; }

        public StudentStickerState DuplicateStickerState { get; set; } = StudentStickerState.None;
        public int DuplicateStickerYear { get; set; }
        public string DuplicateDiplomaNumber { get; set; }
        public string DuplicateRegistrationDiplomaNumber { get; set; }
        public DateTime? DuplicateDiplomaDate { get; set; }

        public PersonStudentDuplicateDiplomaFileHistory File { get; set; }

        public bool IsValid { get; set; }
    }

    public class PersonStudentDuplicateDiplomaHistoryConfiguration : IEntityTypeConfiguration<PersonStudentDuplicateDiplomaHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDuplicateDiplomaHistory> builder)
        {
            builder.Property(e => e.DuplicateDiplomaNumber)
                .HasMaxLength(25);

            builder.Property(e => e.DuplicateRegistrationDiplomaNumber)
                .HasMaxLength(25);
        }
    }
}
