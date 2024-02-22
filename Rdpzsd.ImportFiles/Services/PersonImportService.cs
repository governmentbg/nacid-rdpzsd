using FileStorageNetCore.Api;
using Infrastructure.DomainValidation;
using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.Integrations.EmsIntegration;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Import.Services.Base;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Search;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Services
{
    public class PersonImportService : RdpzsdImportService<PersonImport, PersonImportFile, PersonImportErrorFile, PersonImportHistory, PersonImportHistoryFile, PersonImportHistoryErrorFile>
    {
		public PersonImportService(
			RdpzsdDbContext context,
			UserContext userContext,
			DomainValidatorService domainValidatorService,
			BlobStorageService blobStorageService,
			EmsIntegrationService emsIntegrationService,
			ExcelProcessorService excelProcessorService
		) : base(context, userContext, domainValidatorService, blobStorageService, emsIntegrationService, excelProcessorService)
		{
		}

		public async Task<MemoryStream> ExportUanExcel(int personImportId)
		{
			var personImportUans = await context.PersonImportUans
				.AsNoTracking()
				.Where(e => e.PersonImportId == personImportId)
				.Select(e => e.Uan)
				.ToListAsync();

			var result = await context.PersonLots
				.AsNoTracking()
				.Include(e => e.PersonBasic.BirthCountry)
				.Include(e => e.PersonBasic.BirthSettlement)
				.Include(e => e.PersonBasic.BirthMunicipality)
				.Include(e => e.PersonBasic.BirthDistrict)
				.Include(e => e.PersonBasic.ResidenceCountry)
				.Include(e => e.PersonBasic.ResidenceSettlement)
				.Include(e => e.PersonBasic.ResidenceMunicipality)
				.Include(e => e.PersonBasic.ResidenceDistrict)
				.Include(e => e.PersonBasic.Citizenship)
				.Include(e => e.PersonBasic.SecondCitizenship)
                  .Include(e => e.PersonDoctorals)
                .ThenInclude(e => e.StudentStatus)
                .Include(e => e.PersonStudents)
                .ThenInclude(e => e.StudentStatus)
				.Include(e => e.PersonSecondary)
				.OrderByDescending(e => e.Id)
				.Where(e => personImportUans.Contains(e.Uan))
				.ToPersonExportDto()
				.ToListAsync();

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

            sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
            {
                SheetName = "Основни данни",
                Items = result.Cast<object>().ToList(),
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
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).ResidenceAddress, ColumnName = "Постоянно местоживеене - адрес" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).CitizenshipCode, ColumnName = "Гражданство" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).SecondCitizenshipCode, ColumnName = "Второ гражданство" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).SecondaryInfo, ColumnName = "Средно образование" },
                        e => new ExcelTableTuple { CellItem = ((PersonExportDto)e).GraduatedSpecialities, ColumnName = "ID на завършено висше" },
                    }
            });

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }
	}
}
