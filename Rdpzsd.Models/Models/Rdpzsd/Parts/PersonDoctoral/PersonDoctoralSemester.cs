using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonDoctoralSemester : BasePersonSemester, IIncludeAll<PersonDoctoralSemester>, IValidate
    {
        public DateTime ProtocolDate { get; set; }
        public string ProtocolNumber { get; set; }
        public YearType YearType { get; set; }
        public AttestationType? AttestationType { get; set; }
        [Skip]
        public PersonDoctoral RelocatedFromPart { get; set; }
        public string SemesterRelocatedNumber { get; set; }
        public DateTime? SemesterRelocatedDate { get; set; }
        public PersonDoctoralSemesterRelocatedFile SemesterRelocatedFile { get; set; }

		public IQueryable<PersonDoctoralSemester> IncludeAll(IQueryable<PersonDoctoralSemester> query)
        {
            return query
               .Include(e => e.EducationFeeType)
               .Include(e => e.StudentEvent)
               .Include(e => e.StudentStatus)
               .Include(e => e.RelocatedFromPart.StudentEvent)
               .Include(e => e.RelocatedFromPart.Institution)
               .Include(e => e.RelocatedFromPart.Subordinate)
               .Include(e => e.RelocatedFromPart.InstitutionSpeciality.Speciality)
               .Include(e => e.SemesterRelocatedFile);
        }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if ((StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocation || StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocationAbroad) && SemesterRelocatedFile == null)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_RelocatedFileMissing);
            }

            if (!(StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocation || StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocationAbroad))
			{
                SemesterRelocatedFile = null;
			}

            if (ProtocolDate.Year < 1970 || string.IsNullOrWhiteSpace(ProtocolNumber))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidProtocolDateNumber);
            }

            if (!Enum.IsDefined(typeof(YearType), YearType))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidYearType);
            }

            ValidateSemester(domainValidatorService);
        }
    }

    public class PersonDoctoralSemesterConfiguration : IEntityTypeConfiguration<PersonDoctoralSemester>
    {
        public void Configure(EntityTypeBuilder<PersonDoctoralSemester> builder)
        {
            builder.Property(e => e.Note)
                .HasMaxLength(500);

            builder.Property(e => e.ProtocolNumber)
                .HasMaxLength(15);
        }
    }
}
