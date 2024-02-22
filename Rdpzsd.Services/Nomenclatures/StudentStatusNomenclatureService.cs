using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Dtos.Nomenclatures.StudentStatus;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using Rdpzsd.Services.Nomenclatures.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures
{
	public class StudentStatusNomenclatureService : BaseNomenclatureService<StudentStatus, NomenclatureFilterDto<StudentStatus>>
	{
		public StudentStatusNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}

		public async override Task<MemoryStream> ExportExcel(NomenclatureFilterDto<StudentStatus> filter)
		{
			filter.GetAllData = true;
			SearchResultDto<StudentStatus> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Статуси студенит/докторанти",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((StudentStatus)e).Id, ColumnName = "Номер" },
						e => new ExcelTableTuple { CellItem = ((StudentStatus)e).Name, ColumnName = "Статус" },
						e => new ExcelTableTuple { CellItem = ((StudentStatus)e).NameAlt, ColumnName = "Статус - на английски" },
						e => new ExcelTableTuple { CellItem = ((StudentStatus)e).IsActive, ColumnName = "Активен" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}

	public class StudentEventNomenclatureService : BaseNomenclatureService<StudentEvent, StudentEventFilterDto>
	{
		public StudentEventNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}

		public async override Task<MemoryStream> ExportExcel(StudentEventFilterDto filter)
		{
			filter.GetAllData = true;
			SearchResultDto<StudentEvent> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Събития студенти/докторанти",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((StudentEvent)e).Id, ColumnName = "Номер" },
						e => new ExcelTableTuple { CellItem = ((StudentEvent)e).Name, ColumnName = "Събитие" },
						e => new ExcelTableTuple { CellItem = ((StudentEvent)e).NameAlt, ColumnName = "Събитие - на английски" },
						e => new ExcelTableTuple { CellItem = ((StudentEvent)e).StudentStatus.Name, ColumnName = "Статус" },
						e => new ExcelTableTuple { CellItem = ((StudentEvent)e).IsActive, ColumnName = "Активен" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}
}
