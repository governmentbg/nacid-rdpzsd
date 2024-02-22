using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonStudentDuplicateDiploma : EntityVersion, IValidate
    {
        public int PartId { get; set; }

        public StudentStickerState DuplicateStickerState { get; set; } = StudentStickerState.None;
        public int DuplicateStickerYear { get; set; }
        public string DuplicateDiplomaNumber { get; set; }
        public string DuplicateRegistrationDiplomaNumber { get; set; }
        public DateTime? DuplicateDiplomaDate { get; set; }

        public PersonStudentDuplicateDiplomaFile File { get; set; }

        public bool IsValid { get; set; } = true;

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (string.IsNullOrWhiteSpace(DuplicateDiplomaNumber) || DuplicateDiplomaNumber.Length > 25)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidDuplicateDiplomaNumber);
            }

            if (DuplicateStickerYear < 2009 || DuplicateStickerYear > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidDuplicateStickerYear);
            }

            if (string.IsNullOrWhiteSpace(DuplicateRegistrationDiplomaNumber) || DuplicateRegistrationDiplomaNumber.Length > 25)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidDuplicateRegistrationDiplomaNumber);
            }

            if (!DuplicateDiplomaDate.HasValue || DuplicateDiplomaDate.Value.Year < 2009 || DuplicateDiplomaDate.Value.Year > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidDuplicateDiplomaDate);
            }
        }
    }

    public class PersonStudentDuplicateDiplomaConfiguration : IEntityTypeConfiguration<PersonStudentDuplicateDiploma>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDuplicateDiploma> builder)
        {
            builder.Property(e => e.DuplicateDiplomaNumber)
                .HasMaxLength(25);

            builder.Property(e => e.DuplicateRegistrationDiplomaNumber)
                .HasMaxLength(25);
        }
    }
}
