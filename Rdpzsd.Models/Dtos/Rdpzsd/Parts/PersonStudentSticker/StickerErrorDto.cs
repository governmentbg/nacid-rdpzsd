using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Parts.PersonStudentSticker
{
    public class StickerErrorDto
    {
        public bool HasError { get; set; } = false;

        public List<MissingStudentSemesterDto> MissingStudentSemesters { get; set; } = new List<MissingStudentSemesterDto>();
        public List<CustomStickerErrorDto> OtherErrors { get; set; } = new List<CustomStickerErrorDto>();
    }

    public class CustomStickerErrorDto
    {
        public string Error { get; set; }
        public string ErrorAlt { get; set; }
    }

    public class MissingStudentSemesterDto
    {
        public CourseType Course { get; set; }
        public Semester StudentSemester { get; set; }
    }
}
