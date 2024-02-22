using Infrastructure.Constants;
using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures
{
	public class InstitutionSpecialityNomenclatureService
	{
		protected readonly RdpzsdDbContext context;
		protected readonly ExcelProcessorService excelProcessorService;
		protected readonly UserContext userContext;

		public InstitutionSpecialityNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
			)
		{
			this.context = context;
			this.excelProcessorService = excelProcessorService;
			this.userContext = userContext;
		}

		public async virtual Task<SearchResultDto<InstitutionSpeciality>> GetAll(InstitutionSpecialityFilterDto filter)
		{
			if (filter == null)
			{
				filter = new InstitutionSpecialityFilterDto();
			}

			var query = new InstitutionSpeciality().IncludeAll(context.Set<InstitutionSpeciality>().AsNoTracking());
			query = filter.WhereBuilder(query, userContext, context);

			query = query
				.OrderBy(e => e.Institution.Name)
				.ThenBy(e => e.Speciality.Name)
				.ThenBy(e => e.Id);

			var result = new SearchResultDto<InstitutionSpeciality> {
				TotalCount = await query.CountAsync(),
				Result = filter.GetAllData
					? await query.ToListAsync()
					: await query.Skip(filter.Offset).Take(filter.Limit).ToListAsync()

			};

			return result;
		}

		public async Task<MemoryStream> ExportExcel(InstitutionSpecialityFilterDto filter)
		{
			filter.GetAllData = true;
			SearchResultDto<InstitutionSpeciality> result = await GetAll(filter);

			var sheetName = filter.EducationalQualificationAlias == EducationalQualificationConstants.Doctor ? "Докторски програми" : "Специалности";

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = sheetName,
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Id, ColumnName = "ID на специалност в НАЦИД" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Institution.LotNumber, ColumnName = "Номер на основно звено" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Institution.Name, ColumnName = "Основно звено" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Institution.Parent.LotNumber, ColumnName = "Номер на ВУ/НО" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Institution.Parent.Name, ColumnName = "ВУ/НО" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.Code, ColumnName = "Код на НАЦИД" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.Name, ColumnName = "Наименование на специалност" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.NameAlt, ColumnName = "Наименование на специалност - на английски" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.EducationalQualification.Name, ColumnName = "ОКС/ОНС" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.ResearchArea.Code, ColumnName = "Код на професионално направление" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.ResearchArea.Name, ColumnName = "Професионално направление" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).EducationalForm.Name, ColumnName = "Форма на обучение" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Duration, ColumnName = "Продължителност" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).NsiRegion.Code, ColumnName = "Код детайлна област КОО 2015" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).NsiRegion.Name, ColumnName = "Детайлна област КОО 2015" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).NationalStatisticalInstitute.Code, ColumnName = "Код КОО 2015" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).NationalStatisticalInstitute.Name, ColumnName = "КОО 2015" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).Speciality.IsRegulated, ColumnName = "Регулирана" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).IsForCadets, ColumnName = "За курсанти/офицери" },
						e => new ExcelTableTuple { CellItem = ((InstitutionSpeciality)e).IsActive, ColumnName = "Активна" }
					}
				}
			};

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}
}
