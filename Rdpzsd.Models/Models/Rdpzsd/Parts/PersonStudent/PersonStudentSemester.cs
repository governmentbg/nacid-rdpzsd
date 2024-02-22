using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonStudentSemester : BasePersonSemester, IIncludeAll<PersonStudentSemester>, IValidate
    {
        public int PeriodId { get; set; }
        [Skip]
        public Period Period { get; set; }

        public CourseType Course { get; set; }
        public Semester StudentSemester { get; set; }
        // Specific field which can be true only if StudentEvent is TwoYearsForOne and its the second period from this event
        public bool SecondFromTwoYearsPlan { get; set; } = false;

        public string SemesterRelocatedNumber { get; set; }
        public DateTime? SemesterRelocatedDate { get; set; }
        public PersonStudentSemesterRelocatedFile SemesterRelocatedFile { get; set; }

        public CourseType? IndividualPlanCourse { get; set; }
        public Semester? IndividualPlanSemester { get; set; }

        [Skip]
        public PersonStudent RelocatedFromPart { get; set; }
		public int SemesterInstitutionId { get; set; }
        [Skip]
		public Institution SemesterInstitution { get; set; }

		public IQueryable<PersonStudentSemester> IncludeAll(IQueryable<PersonStudentSemester> query)
        {
            return query
               .Include(e => e.EducationFeeType)
               .Include(e => e.Period)
               .Include(e => e.StudentEvent)
               .Include(e => e.StudentStatus)
               .Include(e => e.RelocatedFromPart.StudentEvent)
               .Include(e => e.RelocatedFromPart.Institution)
               .Include(e => e.RelocatedFromPart.Subordinate)
               .Include(e => e.RelocatedFromPart.InstitutionSpeciality.Speciality)
               .Include(e => e.SemesterRelocatedFile)
               .Include(e => e.SemesterInstitution);
        }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (Course == 0)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidCourse);
            }

            if (StudentSemester == 0)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidStudentSemester);
            }

            if (StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoYears && Course == CourseType.First)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidIndividualPlanTwoYears);
            }

            if (StudentEvent?.Alias != StudentEventConstants.IndividualPlanTwoYears && StudentEvent?.Alias != StudentEventConstants.IndividualPlanTwoSemesters)
            {
                IndividualPlanCourse = null;
                IndividualPlanSemester = null;
            }
            else if(StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoYears)
            {
                IndividualPlanCourse = Course + 1;
            }
            else if (StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoSemesters)
            {
                if (StudentSemester == Semester.Second)
                {
                    IndividualPlanCourse = Course + 1;
                    IndividualPlanSemester = Semester.First;
                }
                else if (StudentSemester == Semester.First)
                {
                    IndividualPlanCourse = Course;
                    IndividualPlanSemester = Semester.Second;
                }
            }

            if ((StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocation || StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocationAbroad) && SemesterRelocatedFile == null)
			{
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_RelocatedFileMissing);
			}

            if (!(StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocation || StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocationAbroad))
			{
                SemesterRelocatedFile = null;
			}

            if (PeriodId == 0)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidPeriod);
            }

            ValidateSemester(domainValidatorService);
        }
    }

    public class PersonStudentSemesterConfiguration : IEntityTypeConfiguration<PersonStudentSemester>
    {
        public void Configure(EntityTypeBuilder<PersonStudentSemester> builder)
        {
            builder.Property(e => e.Note)
                .HasMaxLength(500);
        }
    }
}
