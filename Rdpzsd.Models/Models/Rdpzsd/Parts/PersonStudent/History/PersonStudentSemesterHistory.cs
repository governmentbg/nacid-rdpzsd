using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentSemesterHistory : BasePersonSemester
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
        public PersonStudentSemesterRelocatedFileHistory SemesterRelocatedFile { get; set; }

        public CourseType? IndividualPlanCourse { get; set; }
        public Semester? IndividualPlanSemester { get; set; }

        [Skip]
        public PersonStudent RelocatedFromPart { get; set; }
        public int SemesterInstitutionId { get; set; }
        [Skip]
        public Institution SemesterInstitution { get; set; }
    }

    public class PersonStudentSemesterHistoryConfiguration : IEntityTypeConfiguration<PersonStudentSemesterHistory>
    {
        public void Configure(EntityTypeBuilder<PersonStudentSemesterHistory> builder)
        {
            builder.Property(e => e.Note)
                .HasMaxLength(500);
        }
    }
}
