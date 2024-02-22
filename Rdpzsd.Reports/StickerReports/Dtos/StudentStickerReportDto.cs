using Rdpzsd.Models.Enums.Rdpzsd.Parts;

namespace Rdpzsd.Reports.StickerReports.Dtos
{
    public class StudentStickerReportDto
    {
        public int StickerYear { get; set; }
        public StudentStickerState StickerState { get; set; }

        public string Uan { get; set; }

        public string Institution { get; set; }
        public string InstitutionAlt { get; set; }

        public string FirstName { get; set; }
        public string FirstNameAlt { get; set; }
        public string MiddleName { get; set; }
        public string MiddleNameAlt { get; set; }
        public string LastName { get; set; }
        public string LastNameAlt { get; set; }
        public string OtherNames { get; set; }
        public string OtherNamesAlt { get; set; }

        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string IdnNumber { get; set; }

        public string FacultyNumber { get; set; }
        public bool IsOriginal { get; set; }
    }
}
