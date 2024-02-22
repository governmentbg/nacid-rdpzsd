using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonStudentDiploma : EntityVersion, IValidate
    {
        [Skip]
        public PersonStudent PersonStudent { get; set; }

        public string DiplomaNumber { get; set; }
        public string RegistrationDiplomaNumber { get; set; }
        public DateTime? DiplomaDate { get; set; }

        public PersonStudentDiplomaFile File { get; set; }

        public bool IsValid { get; set; } = true;

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (string.IsNullOrWhiteSpace(DiplomaNumber) || DiplomaNumber.Length > 25)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDiploma_InvalidDiplomaNumber);
            }

            if (string.IsNullOrWhiteSpace(RegistrationDiplomaNumber) || RegistrationDiplomaNumber.Length > 25)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDiploma_InvalidRegistrationDiplomaNumber);
            }

            if (!DiplomaDate.HasValue || DiplomaDate.Value.Year < 2009 || DiplomaDate.Value.Year > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDiploma_InvalidDiplomaDate);
            }

            if (File == null)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDiploma_MissingDiplomaFile);
            }
        }
    }

    public class PersonStudentDiplomaConfiguration : IEntityTypeConfiguration<PersonStudentDiploma>
    {
        public void Configure(EntityTypeBuilder<PersonStudentDiploma> builder)
        {
            builder.HasOne(p => p.PersonStudent)
                   .WithOne(a => a.Diploma)
                   .HasForeignKey<PersonStudentDiploma>(p => p.Id);

            builder.Property(e => e.DiplomaNumber)
                .HasMaxLength(30);

            builder.Property(e => e.RegistrationDiplomaNumber)
                .HasMaxLength(25);
        }
    }
}
