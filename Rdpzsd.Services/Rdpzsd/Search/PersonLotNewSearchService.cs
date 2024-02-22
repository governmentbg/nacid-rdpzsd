using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search;
using Rdpzsd.Models.Enums.Rdpzsd.Search;
using Rdpzsd.Services.Rdpzsd.Search.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Search
{
    public class PersonLotNewSearchService : BasePersonLotSearchService<PersonLotNewFilterDto>
    {
        public PersonLotNewSearchService(
            RdpzsdDbContext context,
            UserContext userContext,
            ExcelProcessorService excelProcessorService

        ) : base(context, userContext, excelProcessorService)
        {
        }

        public override async Task<MemoryStream> ExportExcel(PersonLotNewFilterDto filter)
        {
            filter.GetAllData = true;
            SearchResultDto<PersonExportDto> result = await GetAllExport(filter);
            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

            sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
            {
                SheetName = "Основни данни",
                Items = result.Result.Cast<object>().ToList(),
                Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).Uan, ColumnName = "ЕАН" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).Uin, ColumnName = "ЕГН" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ForeignerNumber, ColumnName = "ЛНЧ" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).PersonIdn, ColumnName = "ИДН" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).BirthDate, ColumnName = "Дата на раждане" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).FirstName, ColumnName = "Първо име" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).MiddleName, ColumnName = "Бащино име" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).LastName, ColumnName = "Фамилия" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).OtherNames, ColumnName = "Други имена" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).FirstNameAlt, ColumnName = "Първо име на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).MiddleNameAlt, ColumnName = "Бащино име на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).LastNameAlt, ColumnName = "Фамилия на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).OtherNameAlt, ColumnName = "Други имена на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).Gender, ColumnName = "Пол" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).Email, ColumnName = "Електронна поща" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).PhoneNumber, ColumnName = "Телефон" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).BirthCountryCode, ColumnName = "Месторождение - държава" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).BirthSettlementCode, ColumnName = "Месторождение - населено място" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).BirthDistrictCode, ColumnName = "Месторождение - област" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).BirthMunicipalityCode, ColumnName = "Месторождение - община" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ForeignerBirthSettlement, ColumnName = "Месторождение - населено място в чужбина" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceCountryCode, ColumnName = "Постоянно местоживеене - държава" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceSettlementCode, ColumnName = "Постоянно местоживеене - населено място" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceDistrictCode, ColumnName = "Постоянно местоживеене - област" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceMunicipalityCode, ColumnName = "Постоянно местоживеене - община" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).PostCode, ColumnName = "Постоянно местоживеене - п.к." },
                        filter.FilterType == PersonLotNewFilterType.IdentificationNumber ?
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceAddress, ColumnName = "Постоянно местоживеене - адрес" } :
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceAddress, ColumnName = "Постоянно местоживеене - адрес в чужбина" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).CitizenshipCode, ColumnName = "Гражданство" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).SecondCitizenshipCode, ColumnName = "Второ гражданство" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).SecondaryInfo, ColumnName = "Средно образование" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).GraduatedSpecialities, ColumnName = "ID на завършено висше" }

                    }
            });

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }
    }

    public class PersonUanExportSearchService : BasePersonLotSearchService<PersonUanExportFilterDto>
    {
        public PersonUanExportSearchService(
            RdpzsdDbContext context,
            UserContext userContext,
            ExcelProcessorService excelProcessorService

        ) : base(context, userContext, excelProcessorService)
        {
        }

        public async virtual Task<SearchResultDto<PersonUanExportDto>> GetAllUanExport(PersonUanExportFilterDto filter)
        {
            var queryIds = GetInitialQuery(filter);

            var loadIds = await queryIds
                .Take(!filter.GetAllData ? filter.Limit : queryIds.Count())
                .ToListAsync();

            var result = await context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonLotIdNumbers)
                .Include(e => e.PersonBasic.BirthCountry)
                .OrderByDescending(e => e.Id)
                .Where(e => loadIds.Contains(e.Id))
                .ToPersonUanExportDto(userContext.Institution.Id)
                .ToListAsync();

            var searchResult = new SearchResultDto<PersonUanExportDto>
            {
                Result = result
            };

            return searchResult;
        }

        public override async Task<MemoryStream> ExportExcel(PersonUanExportFilterDto filter)
        {
            filter.GetAllData = true;
            SearchResultDto<PersonUanExportDto> result = await GetAllUanExport(filter);
            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

            if (filter.FilterType == PersonLotNewFilterType.IdentificationNumber)
            {
                sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "ЕАН",
                    Items = result.Result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).Uan, ColumnName = "ЕАН" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).Uin, ColumnName = "ЕГН" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).ForeignerNumber, ColumnName = "ЛНЧ" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).PersonIdn, ColumnName = "ИДН" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).NameInitials, ColumnName = "Инициали на лицето" }
                    }
                });
            }
            else
            {
                sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "ЕАН",
                    Items = result.Result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).Uan, ColumnName = "ЕАН" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).PersonIdn, ColumnName = "ИДН" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).ForeignerNumber, ColumnName = "ЛНЧ" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).FirstName, ColumnName = "Първо име" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).MiddleName, ColumnName = "Бащино име" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).LastName, ColumnName = "Фамилия" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).FirstNameAlt, ColumnName = "Първо име на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).MiddleNameAlt, ColumnName = "Башино име на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).LastNameAlt, ColumnName = "Фамилия на английски" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).Gender, ColumnName = "Пол" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).BirthDate, ColumnName = "Дата на раждане" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).BirthCountry.Code, ColumnName = "Месторождение - държава" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).ForeignerBirthSettlement, ColumnName = "Месторождение - населено място в чужбина" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).MigrationIdNumber, ColumnName = "IDNumber" },
                        e => new ExcelTableTuple { CellItem = ((PersonUanExportDto)e).MigrationUniId, ColumnName = "UniID" },
                    }
                });
            }

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }
    }
}
