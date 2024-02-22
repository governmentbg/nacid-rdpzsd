using Dapper;
using Infrastructure.AppSettings;
using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Npgsql;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Reports.StickerReports.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Reports.StickerReports.Services
{
    public class InstitutionStickerReportService
    {
        private readonly UserContext userContext;
        private readonly ExcelProcessorService excelProcessorService;

        public InstitutionStickerReportService(
            UserContext userContext,
            ExcelProcessorService excelProcessorService
            )
        {
            this.userContext = userContext;
            this.excelProcessorService = excelProcessorService;
        }

        public async Task<MemoryStream> InstitutionStickerReportExcel(StickerReportFilterDto filterDto)
        {
            var result = await GetInstitutionStickerReport(filterDto);

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

            if (result.Any())
            {
                foreach (var item in result)
                {
                    var sheet = new ExcelMultiSheet<ExcelTableTuple>
                    {
                        SheetName = item.StickerYear.ToString(),
                        Items = item.InstitutionStickerReportDtos.Cast<object>().ToList(),
                        Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                            e => new ExcelTableTuple { CellItem = ((InstitutionStickerReportDto)e).Institution, ColumnName = "Висше училище" },
                            e => new ExcelTableTuple { CellItem = ((InstitutionStickerReportDto)e).InstitutionShort, ColumnName = "Съкратено наименование на ВУ" },
                            e => new ExcelTableTuple { CellItem = ((InstitutionStickerReportDto)e).StudentStickersCount, ColumnName = "Общ брой студенти" }
                        }
                    };

                    sheets.Add(sheet);
                }
            }
            else
            {
                var sheet = new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Няма данни",
                    Items = new List<object>(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>>()
                };

                sheets.Add(sheet);
            }

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }

        public async Task<IEnumerable<InstitutionStickerYearReportDto>> GetInstitutionStickerReport(StickerReportFilterDto filterDto)
        {
            var originalStickersReport = await GetInstitutionOriginalStickerReport(filterDto);
            var duplicateStickersReport = await GetInstitutionDuplicateStickerReport(filterDto);

            var concatedReport = originalStickersReport.Concat(duplicateStickersReport)
                .GroupBy(e => new { e.StickerYear, e.InstitutionId, e.Institution, e.InstitutionAlt, e.InstitutionShort, e.InstitutionShortAlt })
                .Select(e => new InstitutionStickerReportDto
                {
                    StickerYear = e.Key.StickerYear,
                    InstitutionId = e.Key.InstitutionId,
                    Institution = e.Key.Institution,
                    InstitutionAlt = e.Key.InstitutionAlt,
                    InstitutionShort = e.Key.InstitutionShort,
                    InstitutionShortAlt = e.Key.InstitutionShortAlt,
                    StudentStickersCount = e.Sum(t => t.StudentStickersCount)
                })
                .OrderBy(e => e.StickerYear)
                .ThenBy(e => e.Institution)
                .AsEnumerable();

            var result = concatedReport.GroupBy(e => e.StickerYear)
                .Select(e => new InstitutionStickerYearReportDto
                {
                    StickerYear = e.Key,
                    InstitutionStickerReportDtos = e.ToList()
                });

            return result;
        }

        private async Task<IEnumerable<InstitutionStickerReportDto>> GetInstitutionOriginalStickerReport(StickerReportFilterDto filterDto)
        {
            filterDto.IsOriginal = true;

            if (userContext.UserType == UserType.Rsd)
            {
                filterDto.InstitutionId = userContext.Institution.Id;
            }

            using IDbConnection dbConnection = new NpgsqlConnection(AppSettingsProvider.MainDbConnectionString);

            var sqlBuilder = new SqlBuilder();
            var builderTemplate = sqlBuilder.AddTemplate(RawInstitutionStickerTemplate());

            sqlBuilder.Where(ConstantStickerWhere());
            sqlBuilder.Where($"psd.id is null and (ps.stickerstate = {(int)StudentStickerState.StickerForPrint} or ps.stickerstate = {(int)StudentStickerState.Recieved})");
            filterDto.WhereBuilder(sqlBuilder);

            var result = await dbConnection.QueryAsync<InstitutionStickerReportDto>(builderTemplate.RawSql, builderTemplate.Parameters);

            return result;
        }

        private async Task<IEnumerable<InstitutionStickerReportDto>> GetInstitutionDuplicateStickerReport(StickerReportFilterDto filterDto)
        {
            filterDto.IsOriginal = false;

            if (userContext.UserType == UserType.Rsd)
            {
                filterDto.InstitutionId = userContext.Institution.Id;
            }

            using IDbConnection dbConnection = new NpgsqlConnection(AppSettingsProvider.MainDbConnectionString);

            var sqlBuilder = new SqlBuilder();
            var builderTemplate = sqlBuilder.AddTemplate(RawInstitutionDuplicateStickerTemplate());

            sqlBuilder.Where(ConstantStickerWhere());
            sqlBuilder.Where($"psdd.isvalid and (psdd.duplicatestickerstate = {(int)StudentStickerState.StickerForPrint} or (psdd.duplicatestickerstate = {(int)StudentStickerState.Recieved} and psddf is null))");
            filterDto.WhereBuilder(sqlBuilder);

            var result = await dbConnection.QueryAsync<InstitutionStickerReportDto>(builderTemplate.RawSql, builderTemplate.Parameters);

            return result;
        }

        private static string RawInstitutionStickerTemplate()
        {
            var template = @$"select ps.stickeryear as stickeryear, inst.id as institutionid, inst.name as institution, inst.namealt as institutionalt,
                inst.shortname as institutionshort, inst.shortnamealt as institutionshortalt, count(*) as studentstickerscount
                from PersonStudent as ps
                join PersonLot as pl on pl.id = ps.lotid
                join Institution as inst on inst.id = ps.institutionid
                left join PersonStudentDiploma as psd on psd.id = ps.id
                /**where**/
                group by ps.stickeryear, inst.id, inst.name, inst.namealt, inst.shortname, inst.shortnamealt";

            return template;
        }

        private static string RawInstitutionDuplicateStickerTemplate()
        {
            var template = @$"select psdd.duplicatestickeryear as stickeryear, inst.id as institutionid, inst.name as institution, inst.namealt as institutionalt,
                inst.shortname as institutionshort, inst.shortnamealt as institutionshortalt, count(*) as studentstickerscount
                from PersonStudent as ps
                join PersonLot as pl on pl.id = ps.lotid
                join Institution as inst on inst.id = ps.institutionid
                join PersonStudentDiploma as psd on psd.id = ps.id
                join PersonStudentDuplicateDiploma as psdd on psdd.partid = ps.id
                left join PersonStudentDuplicateDiplomaFile as psddf on psddf.id = psdd.id
                /**where**/
                group by psdd.duplicatestickeryear, inst.id, inst.name, inst.namealt, inst.shortname, inst.shortnamealt";

            return template;
        }

        private static string ConstantStickerWhere()
        {
            return @$"pl.state = {(int)LotState.Actual} 
                        and ps.state <> {(int)PartState.Erased}";
        }
    }
}
