using Infrastructure.ExcelProcessor.Models;
using Infrastructure.ExcelProcessor.Services;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtValidationErrorCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Rdpzsd.Import.Services.TxtProcessing
{
    public class TxtExcelService
    {
        private readonly ExcelProcessorService excelProcessorService;

        public TxtExcelService(ExcelProcessorService excelProcessorService)
        {
            this.excelProcessorService = excelProcessorService;
        }

        public MemoryStream GeneratePersonImportErrorExcel(List<LineDto<TxtValidationErrorCode>> errorLinesDto)
        {
            var result = errorLinesDto.ToPersonBasicTxtDto();

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

            sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
            {
                SheetName = "Открити грешки",
                Items = result.Cast<object>().ToList(),
                Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ErrorRow, ColumnName = "Ред в .txt файл" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ErrorCodes, ColumnName = "Номер на грешка" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).Uan, ColumnName = "ЕАН" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).Uin, ColumnName = "ЕГН" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ForeignerNumber, ColumnName = "ЛНЧ" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).IdnNumber, ColumnName = "ИДН" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).BirthDate, ColumnName = "Дата на раждане" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).FirstName, ColumnName = "Първо име" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).MiddleName, ColumnName = "Бащино име" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).LastName, ColumnName = "Фамилия" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).OtherNames, ColumnName = "Други имена" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).FirstNameAlt, ColumnName = "Първо име на английски" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).MiddleNameAlt, ColumnName = "Бащино име на английски" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).LastNameAlt, ColumnName = "Фамилия на английски" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).OtherNamesAlt, ColumnName = "Други имена на английски" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).Gender, ColumnName = "Пол" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).Email, ColumnName = "Лична електронна поща" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).PhoneNumber, ColumnName = "Телефон" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).BirthCountryCode, ColumnName = "Месторождение - държава" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).BirthSettlementCode, ColumnName = "Месторождение - населено място" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).BirthDistrictCode, ColumnName = "Месторождение - област" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).BirthMunicipalityCode, ColumnName = "Месторождение - община" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ForeignerBirthSettlement, ColumnName = "Месторождение - населено място в чужбина" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ResidenceCountryCode, ColumnName = "Постоянно местоживеене - държава" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ResidenceSettlementCode, ColumnName = "Постоянно местоживеене - населено място" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ResidenceDistrictCode, ColumnName = "Постоянно местоживеене - област" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ResidenceMunicipalityCode, ColumnName = "Постоянно местоживеене - община" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).PostCode, ColumnName = "Постоянно местоживеене - п.к." },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ResidenceAddress, ColumnName = "Постоянно местоживеене - адрес" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).ForeignerResidenceAddress, ColumnName = "Постоянно местоживеене - адрес в чужбина" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).CitizenshipCode, ColumnName = "Гражданство" },
                    e => new ExcelTableTuple { CellItem = ((PersonBasicTxtDto)e).SecondCitizenshipCode, ColumnName = "Второ гражданство" }
                }
            });

            sheets.Add(GenerateErrorCodesSheet<TxtValidationErrorCode>());

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }

        public MemoryStream GenerateSpecialityImportErrorExcel(List<LineDto<TxtSpecValidationErrorCode>> errorLinesDto)
        {
            var specialityResult = errorLinesDto.Where(e => e.LineType == LineDtoType.Speciality).ToList().ToSpecialityTxtDto();
            var doctoralResult = errorLinesDto.Where(e => e.LineType == LineDtoType.DoctoralProgramme).ToList().ToDoctoralTxtDto();

            var sheets = new List<ExcelMultiSheet<ExcelTableTuple>>();

            if (specialityResult.Any())
            {
                sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Открити грешки - студенти",
                    Items = specialityResult.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).ErrorRow, ColumnName = "Ред в .txt файл" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).ErrorCodes, ColumnName = "Номер на грешка" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).Uan, ColumnName = "ЕАН" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).BirthDate, ColumnName = "Дата на раждане" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).SpecialityId, ColumnName = "ID на специалност" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).PeriodYear, ColumnName = "СИ - Учебна година" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).PeriodSemester, ColumnName = "СИ - Учебен семестър" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).StudentEvent, ColumnName = "СИ - Семестриално събитие" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).CourseType, ColumnName = "СИ - Курс на студента" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).StudentSemester, ColumnName = "СИ - Семестър на студента" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).EducationFeeType, ColumnName = "СИ - Вид на таксата" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).AdmissionReason, ColumnName = "Основание за прием" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).FacultyNumber, ColumnName = "Факултетен номер" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).PeType, ColumnName = "ПО - Вид" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).PeHighSchoolType, ColumnName = "ПО - В България/В чужбина" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).PeResearchArea, ColumnName = "ПО (висше) - Професионално направление" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).Note, ColumnName = "СИ - Бележка" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).RelocatedFromInstitution, ColumnName = "СИ - Преместен от" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).SemesterRelocatedNumber, ColumnName = "СИ - Номер на документ за признаване" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).SemesterRelocatedDate, ColumnName = "СИ - Дата на документ за признаване" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).HasScholarship, ColumnName = "Стипендия" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).UseHostel, ColumnName = "Общежитие" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).UseHolidayBase, ColumnName = "Почивни бази" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).ParticipatedIntPrograms, ColumnName = "Межд. програми" },
                    e => new ExcelTableTuple { CellItem = ((SpecialityTxtDto)e).ActionState, ColumnName = "Действие" }
                }
                });
            }

            if (doctoralResult.Any())
            {
                sheets.Add(new ExcelMultiSheet<ExcelTableTuple>
                {
                    SheetName = "Открити грешки - докторанти",
                    Items = doctoralResult.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).ErrorRow, ColumnName = "Ред в .txt файл" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).ErrorCodes, ColumnName = "Номер на грешка" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).Uan, ColumnName = "ЕАН" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).BirthDate, ColumnName = "Дата на раждане" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).DoctoralProgrammeId, ColumnName = "ID на докторска програма" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).ProtocolNumber, ColumnName = "ИО - Номер на протокол/заповед" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).ProtocolDate, ColumnName = "ИО - Дата на протокол/заповед" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).StudentEvent, ColumnName = "ИО - Събитие" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).YearType, ColumnName = "ИО - Година на обучение" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).Atestation, ColumnName = "ИО - Оценка от атестация" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).EducationFeeType, ColumnName = "ИО - Вид на таксата" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).AdmissionReason, ColumnName = "Основание за прием" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).StartDate, ColumnName = "Дата на зачисляване" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).EndDate, ColumnName = "Очаквана дата на отчисляване" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).PeHighSchoolType, ColumnName = "ПО - В България/В чужбина" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).PeResearchArea, ColumnName = "ПО (висше) - Професионално направление" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).Note, ColumnName = "ИО - Бележка" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).RelocatedFromInstitution, ColumnName = "ИО - Преместен от" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).SemesterRelocatedNumber, ColumnName = "ИО - Номер на документ за признаване" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).SemesterRelocatedDate, ColumnName = "ИО - Дата на документ за признаване" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).HasScholarship, ColumnName = "Стипендия" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).UseHostel, ColumnName = "Общежитие" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).UseHolidayBase, ColumnName = "Почивни бази" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).ParticipatedIntPrograms, ColumnName = "Межд. програми" },
                    e => new ExcelTableTuple { CellItem = ((DoctoralTxtDto)e).ActionState, ColumnName = "Действие" }
                }
                });
            }

            sheets.Add(GenerateErrorCodesSheet<TxtSpecValidationErrorCode>());

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }

        private ExcelMultiSheet<ExcelTableTuple> GenerateErrorCodesSheet<TEnum>()
            where TEnum : struct, IConvertible
        {
            var errorCodesList = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new TxtValidationErrorCodeDto<TEnum>
                {
                    ErrorCodeNumber = Convert.ToInt32(Enum.Parse(typeof(TEnum), e.ToString()) as Enum),
                    ErrorCode = e
                });

            return new ExcelMultiSheet<ExcelTableTuple>
            {
                SheetName = "Кодове на грешки",
                Items = errorCodesList.Cast<object>().ToList(),
                Expressions = new List<Expression<Func<object, ExcelTableTuple>>> {
                    e => new ExcelTableTuple { CellItem = ((TxtValidationErrorCodeDto<TEnum>)e).ErrorCodeNumber, ColumnName = "Код" },
                    e => new ExcelTableTuple { CellItem = ((TxtValidationErrorCodeDto<TEnum>)e).ErrorCode, ColumnName = "Описание на грешката" }
                }
            };
        }
    }
}
