using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.RdpzsdImport;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{

    public abstract class BasePersonStudentDoctoralTxtDto
    {
        public int StudentEventId { get; set; }
        public int StudentStatusId { get; set; }

        // User information needed for PartInfo and LotActions
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public int CreateInstitutionId { get; set; }
        public int? CreateSubordinateId { get; set; }
    }

    // This is Speciality txt parsed to Dto with types
    public class PersonStudentTxtDto : BasePersonStudentDoctoralTxtDto
    {
        public string Uan { get; set; }
        public DateTime BirthDate { get; set; }
        public int InstitutionSpecialityId { get; set; }
        public int PeriodId { get; set; }
        public CourseType Course { get; set; }
        public Semester StudentSemester { get; set; }

        public int? EducationFeeTypeId { get; set; }
        public int? AdmissionReasonId { get; set; }
        public string FacultyNumber { get; set; }

        public PreviousEducationType? PeType { get; set; }
        public PreviousHighSchoolEducationType? PeHighSchoolType { get; set; }
        public int? PeResearchAreaId { get; set; }

        public string Note { get; set; }

        public int? RelocatedFromInstitutionSpecialityId { get; set; }

        public string SemesterRelocatedNumber { get; set; }
        public DateTime? SemesterRelocatedDate { get; set; }
        public bool HasScholarship { get; set; }
        public bool UseHostel { get; set; }
        public bool UseHolidayBase { get; set; }
        public bool ParticipatedIntPrograms { get; set; }

        public SpecialityImportAction ActionState { get; set; }

        public PersonStudent ToPersonStudent(int lotId, int institutionId, int? subordinateId, int? relocatedFromPartId)
        {
            var personStudent = new PersonStudent
            {
                AdmissionReasonId = AdmissionReasonId.Value,
                FacultyNumber = FacultyNumber,
                InstitutionId = institutionId,
                InstitutionSpecialityId = InstitutionSpecialityId,
                LotId = lotId,
                PeHighSchoolType = PeType == PreviousEducationType.HighSchool ? PeHighSchoolType : null,
                PeResearchAreaId = PeResearchAreaId,
                PeType = PeType.Value,
                State = PartState.Actual,
                StickerState = StudentStickerState.None,
                StudentEventId = StudentEventId,
                StudentStatusId = StudentStatusId,
                SubordinateId = subordinateId,
                Semesters = new List<PersonStudentSemester>
                {
                    new PersonStudentSemester
                    {
                        Course = Course,
                        EducationFeeTypeId = EducationFeeTypeId,
                        HasScholarship = HasScholarship,
                        Note = Note,
                        ParticipatedIntPrograms = ParticipatedIntPrograms,
                        PeriodId = PeriodId,
                        RelocatedFromPartId = relocatedFromPartId,
                        IndividualPlanCourse = null,
                        IndividualPlanSemester = null,
                        SemesterInstitutionId = institutionId,
                        SecondFromTwoYearsPlan = false,
                        SemesterRelocatedDate = SemesterRelocatedDate,
                        SemesterRelocatedNumber = SemesterRelocatedNumber,
                        StudentEventId = StudentEventId,
                        StudentSemester = StudentSemester,
                        StudentStatusId = StudentStatusId,
                        UseHolidayBase = UseHolidayBase,
                        UseHostel = UseHostel
                    }
                },
                PartInfo = new PersonStudentInfo
                {
                    ActionDate = CreateDate,
                    InstitutionId = CreateInstitutionId,
                    SubordinateId = CreateSubordinateId,
                    UserFullname = UserFullname,
                    UserId = UserId
                }
            };

            return personStudent;
        }

        public PersonStudentSemester ToPersonStudentSemester(int personStudentId, PersonStudentSemester lastSemester)
        {
            var (individualPlanCourse, individualPlanSemester) = ConstructIndividualPlanCourseSemester();

            var personStudentSemester = new PersonStudentSemester
            {
                Course = Course,
                EducationFeeTypeId = EducationFeeTypeId,
                HasScholarship = HasScholarship,
                IndividualPlanCourse = individualPlanCourse,
                IndividualPlanSemester = individualPlanSemester,
                Note = Note,
                ParticipatedIntPrograms = ParticipatedIntPrograms,
                PartId = personStudentId,
                PeriodId = PeriodId,
                RelocatedFromPartId = null,
                SecondFromTwoYearsPlan = lastSemester.StudentEventId == 110 && !lastSemester.SecondFromTwoYearsPlan,
                SemesterInstitutionId = CreateInstitutionId,
                SemesterRelocatedDate = SemesterRelocatedDate,
                SemesterRelocatedNumber = SemesterRelocatedNumber,
                StudentEventId = StudentEventId,
                StudentStatusId = StudentStatusId,
                UseHolidayBase = UseHolidayBase,
                UseHostel = UseHostel,
                StudentSemester = StudentSemester
            };

            return personStudentSemester;
        }

        private (CourseType?, Semester?) ConstructIndividualPlanCourseSemester()
        {
            if (StudentEventId == 110)
            {
                return (Course + 1, StudentSemester);
            }
            else if (StudentEventId == 111)
            {
                if (StudentSemester == Semester.First)
                {
                    return (Course, Semester.Second);
                }
                else
                {
                    return (Course + 1, Semester.First);
                }
            }
            else
            {
                return (null, null);
            }
        }
    }
}
