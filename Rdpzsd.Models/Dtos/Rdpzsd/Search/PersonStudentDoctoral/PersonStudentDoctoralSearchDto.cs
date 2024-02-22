using Infrastructure.Constants;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentDoctoral
{
    public class PersonStudentDoctoralSearchDto : PersonSearchDto
    {
        public string FacultyNumber { get; set; }

        protected override void SetPersonStudentsDoctorals(PersonLot personLot)
        {
            if (personLot.PersonStudents.Any())
            {
                foreach (var personStudent in personLot.PersonStudents.Where(e => e.State != PartState.Erased))
                {
                    FacultyNumber = personStudent.FacultyNumber;

                    var latestSemester = personStudent.Semesters
                        .OrderByDescending(e => e.Period.Year)
                        .ThenByDescending(e => e.Period.Semester)
                        .ThenByDescending(e => e.Id)
                        .FirstOrDefault();

                    PersonSemesters.Add(new PersonSemesterSearchDto
                    {
                        Course = latestSemester.IndividualPlanCourse ?? latestSemester.Course,
                        InstitutionSpeciality = personStudent.InstitutionSpeciality,
                        Period = latestSemester.Period,
                        StudentSemester = latestSemester.IndividualPlanSemester ?? latestSemester.StudentSemester,
                        StudentStatus = personStudent.StudentStatus?.Alias == StudentStatusConstants.Graduated ? personStudent.StudentStatus : latestSemester.StudentStatus,
                        StudentEvent = personStudent.StudentStatus?.Alias == StudentStatusConstants.Graduated ? personStudent.StudentEvent : latestSemester.StudentEvent,
                        Institution = personStudent.Institution,
                        DiplomaYear = personStudent.Diploma != null && personStudent.Diploma.DiplomaDate.HasValue ? personStudent.Diploma.DiplomaDate.Value.Year : null
                    });
                }
            }
            else if (personLot.PersonDoctorals.Any())
            {
                foreach (var personDoctoral in personLot.PersonDoctorals.Where(e => e.State != PartState.Erased))
                {
                    var latestSemester = personDoctoral.Semesters
                        .OrderByDescending(e => e.ProtocolDate.Date)
                        .ThenByDescending(e => e.Id)
                        .FirstOrDefault();

                    PersonSemesters.Add(new PersonSemesterSearchDto
                    {
                        InstitutionSpeciality = personDoctoral.InstitutionSpeciality,
                        ProtocolDate = latestSemester.ProtocolDate,
                        ProtocolNumber = latestSemester.ProtocolNumber,
                        StudentStatus = personDoctoral.StudentStatus?.Alias == StudentStatusConstants.Graduated ? personDoctoral.StudentStatus : latestSemester.StudentStatus,
                        StudentEvent = personDoctoral.StudentStatus?.Alias == StudentStatusConstants.Graduated ? personDoctoral.StudentEvent : latestSemester.StudentEvent,
                        Institution = personDoctoral.Institution,
                        YearType = latestSemester.YearType
                    });
                }
            }
        }

        public PersonStudentDoctoralSearchDto(PersonLot personLot) : base(personLot)
        {
        }
    }

    public class PersonSemesterSearchDto
    {
        public DateTime? ProtocolDate { get; set; }
        public string ProtocolNumber { get; set; }
        public Institution Institution { get; set; }
        public InstitutionSpeciality InstitutionSpeciality { get; set; }
        public Period Period { get; set; }
        public CourseType? Course { get; set; }
        public Semester? StudentSemester { get; set; }
        public StudentStatus StudentStatus { get; set; }
        public StudentEvent StudentEvent { get; set; }
        public int? DiplomaYear { get; set; }
        public YearType YearType { get; set; }
    }

    public static class PersonStudentDoctoralSearchDtoExtensions
    {
        public static IQueryable<PersonStudentDoctoralSearchDto> ToPersonStudentDoctoralSearchDto(this IQueryable<PersonLot> personLots)
        {
            return personLots.Select(e => new PersonStudentDoctoralSearchDto(e));
        }
    }
}
