using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    public class SpecialityTxtDto
    {
        public string Uan { get; set; }
        public string BirthDate { get; set; }
        public string SpecialityId { get; set; }
        public string PeriodYear { get; set; }
        public string PeriodSemester { get; set; }
        public string StudentEvent { get; set; }
        public string CourseType { get; set; }
        public string StudentSemester { get; set; }
        public string EducationFeeType { get; set; }
        public string AdmissionReason { get; set; }
        public string FacultyNumber { get; set; }
        public string PeType { get; set; }
        public string PeHighSchoolType { get; set; }
        public string PeResearchArea { get; set; }
        public string Note { get; set; }
        public string RelocatedFromInstitution { get; set; }
        public string SemesterRelocatedNumber { get; set; }
        public string SemesterRelocatedDate { get; set; }
        public string HasScholarship { get; set; }
        public string UseHostel { get; set; }
        public string UseHolidayBase { get; set; }
        public string ParticipatedIntPrograms { get; set; }
        public string ActionState { get; set; }

        public int ErrorRow { get; set; }
        public string ErrorCodes { get; set; }

        public SpecialityTxtDto(LineDto<TxtSpecValidationErrorCode> lineDto)
        {
            Uan = lineDto.Columns.SingleOrDefault(e => e.Index == 0)?.Value;
            BirthDate = lineDto.Columns.SingleOrDefault(e => e.Index == 1)?.Value;
            SpecialityId = lineDto.Columns.SingleOrDefault(e => e.Index == 2)?.Value;
            PeriodYear = lineDto.Columns.SingleOrDefault(e => e.Index == 3)?.Value;
            PeriodSemester = lineDto.Columns.SingleOrDefault(e => e.Index == 4)?.Value;
            StudentEvent = lineDto.Columns.SingleOrDefault(e => e.Index == 5)?.Value;
            CourseType = lineDto.Columns.SingleOrDefault(e => e.Index == 6)?.Value;
            StudentSemester = lineDto.Columns.SingleOrDefault(e => e.Index == 7)?.Value;
            EducationFeeType = lineDto.Columns.SingleOrDefault(e => e.Index == 8)?.Value;
            AdmissionReason = lineDto.Columns.SingleOrDefault(e => e.Index == 9)?.Value;
            FacultyNumber = lineDto.Columns.SingleOrDefault(e => e.Index == 10)?.Value;
            PeType = lineDto.Columns.SingleOrDefault(e => e.Index == 11)?.Value;
            PeHighSchoolType = lineDto.Columns.SingleOrDefault(e => e.Index == 12)?.Value;
            PeResearchArea = lineDto.Columns.SingleOrDefault(e => e.Index == 13)?.Value;
            Note = lineDto.Columns.SingleOrDefault(e => e.Index == 14)?.Value;
            RelocatedFromInstitution = lineDto.Columns.SingleOrDefault(e => e.Index == 15)?.Value;
            SemesterRelocatedNumber = lineDto.Columns.SingleOrDefault(e => e.Index == 16)?.Value;
            SemesterRelocatedDate = lineDto.Columns.SingleOrDefault(e => e.Index == 17)?.Value;
            HasScholarship = lineDto.Columns.SingleOrDefault(e => e.Index == 18)?.Value;
            UseHostel = lineDto.Columns.SingleOrDefault(e => e.Index == 19)?.Value;
            UseHolidayBase = lineDto.Columns.SingleOrDefault(e => e.Index == 20)?.Value;
            ParticipatedIntPrograms = lineDto.Columns.SingleOrDefault(e => e.Index == 21)?.Value;
            ActionState = lineDto.Columns.SingleOrDefault(e => e.Index == 22)?.Value;

            ErrorRow = lineDto.RowIndex;
            ErrorCodes = $"{string.Join("; ", lineDto.ErrorCodes.Select(e => (int)e))}";
        }
    }

    public static class SpecialityTxtDtoExtensions
    {
        public static List<SpecialityTxtDto> ToSpecialityTxtDto(this List<LineDto<TxtSpecValidationErrorCode>> lineDtos)
        {
            return lineDtos.Select(e => new SpecialityTxtDto(e)).ToList();
        }
    }
}
