using Dapper;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;

namespace Rdpzsd.Reports.StickerReports.Dtos
{
    public class StickerReportFilterDto
    {
        public int? InstitutionId { get; set; }
        public StudentStickerState StickerState { get; set; } = StudentStickerState.StickerForPrint;
        public int? StickerYear { get; set; }

        // Server only
        public bool IsOriginal { get; set; } = true;

        public void WhereBuilder(SqlBuilder sqlBuilder)
        {
            if (InstitutionId.HasValue)
            {
                sqlBuilder.Where("ps.institutionid = @InstitutionId", new { InstitutionId });
            }

            if (IsOriginal)
            {
                sqlBuilder.Where("ps.stickerstate = @StickerState", new { StickerState });

                if (StickerYear.HasValue)
                {
                    sqlBuilder.Where("ps.stickeryear = @StickerYear", new { StickerYear });
                }
            }
            else
            {
                sqlBuilder.Where("psdd.duplicatestickerstate = @StickerState", new { StickerState });

                if (StickerYear.HasValue)
                {
                    sqlBuilder.Where("psdd.duplicatestickeryear = @StickerYear", new { StickerYear });
                }
            }
        }
    }
}
