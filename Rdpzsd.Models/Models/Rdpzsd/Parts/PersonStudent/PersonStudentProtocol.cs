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
    public class PersonStudentProtocol : EntityVersion, IValidate
    {
        public int PartId { get; set; }
        public StudentProtocolType ProtocolType { get; set; } = StudentProtocolType.StateExamination;
        public string ProtocolNumber { get; set; }
        public DateTime ProtocolDate { get; set; }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (string.IsNullOrWhiteSpace(ProtocolNumber))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidProtocolNumber);
            }

            if (ProtocolDate.Year < 1970 || ProtocolDate.Year > DateTime.Now.Year)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidProtocolDate);
            }

            if (ProtocolType == 0)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidProtocolType);
            }
        }
    }

    public class PersonStudentProtocolConfiguration : IEntityTypeConfiguration<PersonStudentProtocol>
    {
        public void Configure(EntityTypeBuilder<PersonStudentProtocol> builder)
        {
            builder.Property(e => e.ProtocolNumber)
                .HasMaxLength(30);
        }
    }
}
