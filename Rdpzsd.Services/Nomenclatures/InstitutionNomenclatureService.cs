using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Enums;
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
	public class InstitutionNomenclatureService : BaseNomenclatureCodeService<Institution, InstitutionFilterDto>
	{
		public InstitutionNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}
		public async override Task<SearchResultDto<Institution>> GetAll(InstitutionFilterDto filter)
		{
			if (filter.GetJointSpecialityInstitutions && filter.InstitutionSpecialityId.HasValue)
			{
				var institutionSpeciality = context.InstitutionSpecialities
					.Include(e => e.InstitutionSpecialityJointSpecialities)
					.Include(e => e.Institution)
					.Single(e => e.Id == filter.InstitutionSpecialityId);

				filter.InstitutionIds = institutionSpeciality.InstitutionSpecialityJointSpecialities.Select(e => e.InstitutionId).Append(institutionSpeciality.Institution.RootId).Distinct().ToList();
			}
				return await base.GetAll(filter);
		}

		public async override Task<MemoryStream> ExportExcel(InstitutionFilterDto filter)
		{
			filter.GetAllData = true;
			SearchResultDto<Institution> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

			if (filter.Level == Level.Second)
			{
				sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "ВУ/НО",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((Institution)e).LotNumber, ColumnName = "Номер" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).Name, ColumnName = "Наименование" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).OrganizationType, ColumnName = "Тип" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).Parent.Name, ColumnName = "ВУ/НО" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).Settlement.Name, ColumnName = "Населено място" }
					}
				});
			}
			else if (filter.Level == Level.First)
			{
				sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "ВУ/НО",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((Institution)e).LotNumber, ColumnName = "Номер" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).Name, ColumnName = "Наименование" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).OrganizationType, ColumnName = "Тип" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).OwnershipType, ColumnName = "Форма на собственост" },
						e => new ExcelTableTuple { CellItem = ((Institution)e).Settlement.Name, ColumnName = "Населено място" }
					}
				});
			}

			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}

	public class EducationalQualificationNomenclatureService : BaseNomenclatureService<EducationalQualification, EducationalQualificationFilterDto>
	{
		public EducationalQualificationNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}
	}

	public class EducationalFormNomenclatureService : BaseNomenclatureService<EducationalForm, NomenclatureFilterDto<EducationalForm>>
	{
		public EducationalFormNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}
	}

	public class ResearchAreaNomenclatureService : BaseNomenclatureCodeService<ResearchArea, NomenclatureHierarchyFilterDto<ResearchArea>>
	{
		public ResearchAreaNomenclatureService(
			RdpzsdDbContext context,
			ExcelProcessorService excelProcessorService,
			UserContext userContext
		) : base(context, excelProcessorService, userContext)
		{
		}
		public async override Task<MemoryStream> ExportExcel(NomenclatureHierarchyFilterDto<ResearchArea> filter)
		{
			filter.GetAllData = true;
			SearchResultDto<ResearchArea> result = await GetAll(filter);

			var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
				new ExcelMultiSheet<ExcelTableTuple>
				{
					SheetName = "Вид на таксата за обучение",
					Items = result.Result.Cast<object>().ToList(),
					Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
						e => new ExcelTableTuple { CellItem = ((ResearchArea)e).Code, ColumnName = "Код" },
						e => new ExcelTableTuple { CellItem = ((ResearchArea)e).Name, ColumnName = "Проф. направление" },
						e => new ExcelTableTuple { CellItem = ((ResearchArea)e).NameAlt, ColumnName = "Проф. направление - на английски" },
						e => new ExcelTableTuple { CellItem = ((ResearchArea)e).IsActive, ColumnName = "Активен" }
					}
				}
			};
			var excelStream = excelProcessorService.ExportMultiSheet(sheets);

			return excelStream;
		}
	}
}
