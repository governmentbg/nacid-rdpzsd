using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Dtos.Nomenclatures.Settlements;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Services.Nomenclatures.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures
{
    public class CountryNomenclatureService : BaseNomenclatureCodeService<Country, NomenclatureCodeFilterDto<Country>>
    {
		public CountryNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}

		public async override Task<MemoryStream> ExportExcel(NomenclatureCodeFilterDto<Country> filter)
		{
			filter.GetAllData = true;
			SearchResultDto<Country> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Държави",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((Country)e).Code, ColumnName = "Код" },
						e => new ExcelTableTuple { CellItem = ((Country)e).Name, ColumnName = "Държава" },
						e => new ExcelTableTuple { CellItem = ((Country)e).NameAlt, ColumnName = "Държава - на английски" },
						e => new ExcelTableTuple { CellItem = ((Country)e).IsActive, ColumnName = "Активен" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}

	public class DistrictNomenclatureService : BaseNomenclatureCodeService<District, NomenclatureCodeFilterDto<District>>
	{
		public DistrictNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}

		public async override Task<MemoryStream> ExportExcel(NomenclatureCodeFilterDto<District> filter)
		{
			filter.GetAllData = true;
			SearchResultDto<District> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Области",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((District)e).Code, ColumnName = "Код" },
						e => new ExcelTableTuple { CellItem = ((District)e).Name, ColumnName = "Област" },
						e => new ExcelTableTuple { CellItem = ((District)e).NameAlt, ColumnName = "Област - на английски" },
						e => new ExcelTableTuple { CellItem = ((District)e).IsActive, ColumnName = "Активен" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}

	public class MunicipalityNomenclatureService : BaseNomenclatureCodeService<Municipality, MunicipalityFilterDto>
	{
		public MunicipalityNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}

		public async override Task<MemoryStream> ExportExcel(MunicipalityFilterDto filter)
		{
			filter.GetAllData = true;
			SearchResultDto<Municipality> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Общини",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((Municipality)e).Code, ColumnName = "Код" },
						e => new ExcelTableTuple { CellItem = ((Municipality)e).Name, ColumnName = "Община" },
						e => new ExcelTableTuple { CellItem = ((Municipality)e).NameAlt, ColumnName = "Община - на английски" },
						e => new ExcelTableTuple { CellItem = ((Municipality)e).IsActive, ColumnName = "Активен" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}

	public class SettlementNomenclatureService : BaseNomenclatureCodeService<Settlement, SettlementFilterDto>
	{
		public SettlementNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}

		public async override Task<MemoryStream> ExportExcel(SettlementFilterDto filter)
		{
			filter.GetAllData = true;
			SearchResultDto<Settlement> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Населени места",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((Settlement)e).Code, ColumnName = "Код ЕКАТТЕ" },
						e => new ExcelTableTuple { CellItem = ((Settlement)e).Name, ColumnName = "Населено място" },
						e => new ExcelTableTuple { CellItem = ((Settlement)e).NameAlt, ColumnName = "Населено място - на английски" },
						e => new ExcelTableTuple { CellItem = ((Settlement)e).IsActive, ColumnName = "Активен" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}
}
