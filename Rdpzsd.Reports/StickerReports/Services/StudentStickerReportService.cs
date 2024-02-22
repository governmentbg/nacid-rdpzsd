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
    public class StudentStickerReportService
    {
        private readonly UserContext userContext;
        private readonly ExcelProcessorService excelProcessorService;

        public StudentStickerReportService(
            UserContext userContext,
            ExcelProcessorService excelProcessorService
            )
        {
            this.userContext = userContext;
            this.excelProcessorService = excelProcessorService;
        }

        public async Task<MemoryStream> StudentStickerReportExcel(StickerReportFilterDto filterDto)
        {
            var result = await GetStudentStickerReport(filterDto);

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
                new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Стикери - студенти",
                    Items = result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).StickerYear, ColumnName = "Година на стикера" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).StickerState, ColumnName = "Статус на стикера" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).Uan, ColumnName = "ЕАН" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).Institution, ColumnName = "ВУ" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).FirstName, ColumnName = "Име" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).MiddleName, ColumnName = "Презиме" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).LastName, ColumnName = "Фамилия" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).OtherNames, ColumnName = "Други имена" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).Uin, ColumnName = "ЕГН" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).ForeignerNumber, ColumnName = "ЛНЧ" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).IdnNumber, ColumnName = "ИДН" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).FacultyNumber, ColumnName = "Факултетен номер" },
                        e => new ExcelTableTuple { CellItem = ((StudentStickerReportDto)e).IsOriginal, ColumnName = "За оригинал" }
                    }
                }
            };

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }

        public async Task<IEnumerable<StudentStickerReportDto>> GetStudentStickerReport(StickerReportFilterDto filterDto)
        {
            var originalStickersReport = await GetStudentOriginalStickerReport(filterDto);
            var duplicateStickersReport = await GetStudentDuplicateStickerReport(filterDto);

            var result = originalStickersReport.Concat(duplicateStickersReport)
                .OrderBy(e => e.StickerYear)
                .ThenBy(e => e.Uan)
                .AsEnumerable();

            return result;
        }

        private async Task<IEnumerable<StudentStickerReportDto>> GetStudentOriginalStickerReport(StickerReportFilterDto filterDto)
        {
            filterDto.IsOriginal = true;

            if (userContext.UserType == UserType.Rsd)
            {
                filterDto.InstitutionId = userContext.Institution.Id;
            }

            using IDbConnection dbConnection = new NpgsqlConnection(AppSettingsProvider.MainDbConnectionString);

            var sqlBuilder = new SqlBuilder();
            var builderTemplate = sqlBuilder.AddTemplate(RawStudentStickerTemplate());

            sqlBuilder.Where(ConstantStickerWhere());
            sqlBuilder.Where($"psd.id is null and (ps.stickerstate = {(int)StudentStickerState.StickerForPrint} or ps.stickerstate = {(int)StudentStickerState.Recieved})");
            filterDto.WhereBuilder(sqlBuilder);

            var result = await dbConnection.QueryAsync<StudentStickerReportDto>(builderTemplate.RawSql, builderTemplate.Parameters);

            return result;
        }

        private async Task<IEnumerable<StudentStickerReportDto>> GetStudentDuplicateStickerReport(StickerReportFilterDto filterDto)
        {
            filterDto.IsOriginal = false;

            if (userContext.UserType == UserType.Rsd)
            {
                filterDto.InstitutionId = userContext.Institution.Id;
            }

            using IDbConnection dbConnection = new NpgsqlConnection(AppSettingsProvider.MainDbConnectionString);

            var sqlBuilder = new SqlBuilder();
            var builderTemplate = sqlBuilder.AddTemplate(RawStudentDuplicateStickerTemplate());

            sqlBuilder.Where(ConstantStickerWhere());
            sqlBuilder.Where($"psdd.isvalid and (psdd.duplicatestickerstate = {(int)StudentStickerState.StickerForPrint} or (psdd.duplicatestickerstate = {(int)StudentStickerState.Recieved} and psddf is null))");
            filterDto.WhereBuilder(sqlBuilder);

            var result = await dbConnection.QueryAsync<StudentStickerReportDto>(builderTemplate.RawSql, builderTemplate.Parameters);

            return result;
        }

        private static string RawStudentStickerTemplate()
        {
            var template = @$"select ps.stickeryear as stickeryear, ps.stickerstate as stickerstate, pl.uan as uan, inst.name as institution, inst.namealt as institutionalt,
                pb.firstname, pb.firstnamealt, pb.middlename, pb.middlenamealt, pb.lastname, pb.lastnamealt, pb.othernames, pb.othernamesalt,
                pb.uin as uin, pb.foreignernumber as foreignernumber, pb.idnnumber as idnnumber, ps.facultynumber as facultynumber, true as isoriginal
                from PersonStudent as ps
                join PersonLot as pl on pl.id = ps.lotid
                join PersonBasic as pb on pb.id = pl.id
                join Institution as inst on inst.id = ps.institutionid
                left join PersonStudentDiploma as psd on psd.id = ps.id
                /**where**/";

            return template;
        }

        private static string RawStudentDuplicateStickerTemplate()
        {
            var template = @$"select psdd.duplicatestickeryear as stickeryear, psdd.duplicatestickerstate as stickerstate, pl.uan as uan, inst.name as institution, inst.namealt as institutionalt,
                pb.firstname, pb.firstnamealt, pb.middlename, pb.middlenamealt, pb.lastname, pb.lastnamealt, pb.othernames, pb.othernamesalt,
                pb.uin as uin, pb.foreignernumber as foreignernumber, pb.idnnumber as idnnumber, ps.facultynumber as facultynumber, false as isoriginal
                from PersonStudent as ps
                join PersonLot as pl on pl.id = ps.lotid
                join PersonBasic as pb on pb.id = pl.id
                join Institution as inst on inst.id = ps.institutionid
                join PersonStudentDiploma as psd on psd.id = ps.id
                join PersonStudentDuplicateDiploma as psdd on psdd.partid = ps.id
                left join PersonStudentDuplicateDiplomaFile as psddf on psddf.id = psdd.id
                /**where**/";

            return template;
        }

        private static string ConstantStickerWhere()
        {
            return @$"pl.state = {(int)LotState.Actual} 
                        and ps.state <> {(int)PartState.Erased}";
        }
    }
}
