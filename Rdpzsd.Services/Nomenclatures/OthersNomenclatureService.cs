using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Nomenclatures;
using Rdpzsd.Models.Dtos.Nomenclatures.Others;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Services.Nomenclatures.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Nomenclatures
{
    public class PeriodNomenclatureService : BaseEditableNomenclatureService<Period, PeriodFilterDto>
    {
        public PeriodNomenclatureService(
            RdpzsdDbContext context,
            ExcelProcessorService excelProcessorService,
            UserContext userContext,
            DomainValidatorService domainValidatorService
        ) : base(context, excelProcessorService, userContext, domainValidatorService)
        {
        }

        public async override Task<Period> Update(Period entity)
        {
            var original = await context.Periods
                .SingleAsync(e => e.Id == entity.Id);

            original.IsActive = entity.IsActive;

            await context.SaveChangesAsync();

            return original;
        }

        public async Task<Period> GetLatestPeriod()
        {
            var latestPeriod = await context.Periods
                .AsNoTracking()
                .OrderByDescending(e => e.Year)
                .ThenByDescending(e => e.Semester)
                .FirstOrDefaultAsync();

            return latestPeriod;
        }

        public async Task<Period> GetNextPeriod(int year, Semester semester)
        {
            int nextYear = year;
            Semester nextSemester = Semester.First;

            if (semester == Semester.Second)
            {
                nextYear += 1;
            }
            else
            {
                nextSemester = Semester.Second;
            }

            var nextPeriod = await context.Periods
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Year == nextYear && e.Semester == nextSemester);

            if (nextPeriod == null)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.Period_NextPeriodNotStarted);
            }

            return nextPeriod;
        }

        public async Task<Period> GetPreviousPeriod(int year, Semester semester)
        {
            int previousYear = year;
            Semester previousSemester = Semester.Second;

            if (semester == Semester.First)
            {
                previousYear -= 1;
            }
            else
            {
                previousSemester = Semester.First;
            }

            var previousPeriod = await context.Periods
                .AsNoTracking()
                .SingleAsync(e => e.Year == previousYear && e.Semester == previousSemester);

            return previousPeriod;
        }
    }

    public class AdmissionReasonNomenclatureService : BaseEditableNomenclatureService<AdmissionReason, AdmissionReasonFilterDto>
    {
        public AdmissionReasonNomenclatureService(
            RdpzsdDbContext context,
            ExcelProcessorService excelProcessorService,
            UserContext userContext,
            DomainValidatorService domainValidatorService
        ) : base(context, excelProcessorService, userContext, domainValidatorService)
        {
        }

        public async override Task<AdmissionReason> Update(AdmissionReason entity)
        {
            var admissionReasonHistory = new AdmissionReasonHistory
            {
                AdmissionReasonId = entity.Id,
                ChangeDate = DateTime.Now,
                UserId = (int)userContext.UserId,
                UserFullName = userContext.UserFullname,
                Description = entity.Description,
                Name = entity.Name,
                NameAlt = entity.NameAlt,
                ShortName = entity.ShortName,
                ShortNameAlt = entity.ShortNameAlt,
                IsActive = entity.IsActive,
            };

            foreach (var admissionReasonEducationFee in entity.AdmissionReasonEducationFees)
            {
                var admissionReasonEducationFeeHistory = new AdmissionReasonEducationFeeHistory
                {
                    AdmissionReasonId = entity.Id,
                    EducationFeeTypeId = admissionReasonEducationFee.EducationFeeTypeId,
                    EducationFeeTypeName = admissionReasonEducationFee.EducationFeeType.Name
                };

                admissionReasonHistory.AdmissionReasonEducationFeeHistories.Add(admissionReasonEducationFeeHistory);
            }

            entity.AdmissionReasonHistories.Add(admissionReasonHistory);

            return await base.Update(entity);
        }

        public async override Task<MemoryStream> ExportExcel(AdmissionReasonFilterDto filter)
        {
            filter.GetAllData = true;
            SearchResultDto<AdmissionReason> result = await GetAll(filter);

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
                new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Основания за прием",
                    Items = result.Result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).Id, ColumnName = "Номер" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).ShortName, ColumnName = "Кратко наименование" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).ShortNameAlt, ColumnName = "Кратко наименование - на английски" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).Name, ColumnName = "Нормативно основание" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).NameAlt, ColumnName = "Нормативно основание - на английски" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).Description, ColumnName = "Пояснение" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).IsActive, ColumnName = "Активен" },
                        e => new ExcelTableTuple { CellItem = ((AdmissionReason)e).OldCode, ColumnName = "Стар код" },
                    }
                }
            };

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }
    }

    public class EducationFeeTypeNomenclatureService : BaseNomenclatureService<EducationFeeType, EducationFeeTypeFilterDto>
    {
        public EducationFeeTypeNomenclatureService(
            RdpzsdDbContext context,
            ExcelProcessorService excelProcessorService,
            UserContext userContext
        ) : base(context, excelProcessorService, userContext)
        {
        }

        public async override Task<MemoryStream> ExportExcel(EducationFeeTypeFilterDto filter)
        {
            filter.GetAllData = true;
            SearchResultDto<EducationFeeType> result = await GetAll(filter);

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
                new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Вид на таксата за обучение",
                    Items = result.Result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((EducationFeeType)e).Id, ColumnName = "Номер" },
                        e => new ExcelTableTuple { CellItem = ((EducationFeeType)e).Name, ColumnName = "Наименование" },
                        e => new ExcelTableTuple { CellItem = ((EducationFeeType)e).NameAlt, ColumnName = "Наименование - на английски" },
                        e => new ExcelTableTuple { CellItem = ((EducationFeeType)e).IsActive, ColumnName = "Активен" }
                    }
                }
            };

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }
    }

    public class SchoolNomenclatureService : BaseEditableNomenclatureService<School, SchoolFilterDto>
    {
        public SchoolNomenclatureService(
            RdpzsdDbContext context,
            ExcelProcessorService excelProcessorService,
            UserContext userContext,
            DomainValidatorService domainValidatorService
        ) : base(context, excelProcessorService, userContext, domainValidatorService)
        {
        }

        public async Task<School> GetSchoolByMigrationId(int migrationId)
        {
            var school = await context
                .Schools
                .AsNoTracking()
                .Include(e => e.Settlement)
                .SingleOrDefaultAsync(e => e.MigrationId == migrationId);

            return school;
        }

        public async override Task<MemoryStream> ExportExcel(SchoolFilterDto filter)
        {
            filter.GetAllData = true;
            SearchResultDto<School> result = await GetAll(filter);

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>> {
                new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Училища",
                    Items = result.Result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                        e => new ExcelTableTuple { CellItem = ((School)e).Id, ColumnName = "Номер" },
                        e => new ExcelTableTuple { CellItem = ((School)e).Name, ColumnName = "Наименование" },
                        e => new ExcelTableTuple { CellItem = ((School)e).State, ColumnName = "Статус" },
                        e => new ExcelTableTuple { CellItem = ((School)e).Type, ColumnName = "Вид (чл. 24-27/37-41)" },
                        e => new ExcelTableTuple { CellItem = ((School)e).OwnershipType, ColumnName = "Вид (чл. 35-36)" },
                        e => new ExcelTableTuple { CellItem = ((School)e).Settlement.Name, ColumnName = "Населено място" },
                        e => new ExcelTableTuple { CellItem = ((School)e).District.Name, ColumnName = "Област" },
                        e => new ExcelTableTuple { CellItem = ((School)e).Municipality.Name, ColumnName = "Община" },
                        e => new ExcelTableTuple { CellItem = ((School)e).IsActive, ColumnName = "Активен" }
                    }
                }
            };

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }
    }
}
