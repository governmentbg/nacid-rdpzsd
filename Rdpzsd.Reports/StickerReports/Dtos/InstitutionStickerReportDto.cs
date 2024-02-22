using System.Collections.Generic;

namespace Rdpzsd.Reports.StickerReports.Dtos
{
    public class InstitutionStickerReportDto
    {
        public int StickerYear { get; set; }

        public int InstitutionId { get; set; }
        public string Institution { get; set; }
        public string InstitutionAlt { get; set; }
        public string InstitutionShort { get; set; }
        public string InstitutionShortAlt { get; set; }

        public int StudentStickersCount { get; set; }
    }

    public class InstitutionStickerYearReportDto
    {
        public int StickerYear { get; set; }

        public List<InstitutionStickerReportDto> InstitutionStickerReportDtos { get; set; } = new List<InstitutionStickerReportDto>();
    }
}
