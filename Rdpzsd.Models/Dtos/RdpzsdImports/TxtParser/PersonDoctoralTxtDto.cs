using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.RdpzsdImport;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    // This is Doctoral txt parsed to Dto with types
    public class PersonDoctoralTxtDto : BasePersonStudentDoctoralTxtDto
    {
        public string Uan { get; set; }
        public DateTime BirthDate { get; set; }
        public int DoctoralProgrammeId { get; set; }
        public string ProtocolNumber { get; set; }
        public DateTime ProtocolDate { get; set; }
        public YearType YearType { get; set; }

        public AttestationType? Atestation { get; set; }
        public int? EducationFeeTypeId { get; set; }
        public int? AdmissionReasonId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

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

        public PersonDoctoral ToPersonDoctoral(int lotId, int institutionId, int? subordinateId, int? relocatedFromPartId)
        {
            var personDoctoral = new PersonDoctoral
            {
                AdmissionReasonId = AdmissionReasonId.Value,
                EndDate = EndDate,
                InstitutionId = institutionId,
                InstitutionSpecialityId = DoctoralProgrammeId,
                LotId = lotId,
                PeHighSchoolType = PeHighSchoolType,
                PeResearchAreaId = PeResearchAreaId,
                PeType = PreviousEducationType.HighSchool,
                StartDate = StartDate.Value,
                State = PartState.Actual,
                StudentEventId = StudentEventId,
                StudentStatusId = StudentStatusId,
                SubordinateId = subordinateId,
                Semesters = new List<PersonDoctoralSemester>
                {
                    new PersonDoctoralSemester
                    {
                        AttestationType = Atestation,
                        ProtocolDate = ProtocolDate,
                        ProtocolNumber = ProtocolNumber,
                        YearType = YearType,
                        EducationFeeTypeId = EducationFeeTypeId,
                        HasScholarship = HasScholarship,
                        Note = Note,
                        ParticipatedIntPrograms = ParticipatedIntPrograms,
                        RelocatedFromPartId = relocatedFromPartId,
                        SemesterRelocatedDate = SemesterRelocatedDate,
                        SemesterRelocatedNumber = SemesterRelocatedNumber,
                        StudentEventId = StudentEventId,
                        StudentStatusId = StudentStatusId,
                        UseHolidayBase = UseHolidayBase,
                        UseHostel = UseHostel
                    }
                },
                PartInfo = new PersonDoctoralInfo
                {
                    ActionDate = CreateDate,
                    InstitutionId = CreateInstitutionId,
                    SubordinateId = CreateSubordinateId,
                    UserFullname = UserFullname,
                    UserId = UserId
                }
            };

            return personDoctoral;
        }

        public PersonDoctoralSemester ToPersonDoctoralSemester(int personDoctoralId, PersonDoctoralSemester lastSemester)
        {
            var personDoctoralSemester = new PersonDoctoralSemester
            {
                AttestationType = Atestation,
                EducationFeeTypeId = EducationFeeTypeId,
                HasScholarship = HasScholarship,
                Note = Note,
                ParticipatedIntPrograms = ParticipatedIntPrograms,
                PartId = personDoctoralId,
                ProtocolDate = ProtocolDate,
                ProtocolNumber = ProtocolNumber,
                RelocatedFromPartId = null,
                SemesterRelocatedDate = SemesterRelocatedDate,
                SemesterRelocatedNumber = SemesterRelocatedNumber,
                StudentEventId = StudentEventId,
                StudentStatusId = StudentStatusId,
                UseHolidayBase = UseHolidayBase,
                UseHostel = UseHostel,
                YearType = YearType
            };

            return personDoctoralSemester;
        }
    }
}
