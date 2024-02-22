using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.Extensions;
using Rdpzsd.Import.Services.TxtValidation;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtValidationErrorCodes;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.RdpzsdImport;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.RdpzsdImports.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rdpzsd.Import.Services.TxtParser
{
    public class TxtParserService
    {
        private readonly NomenclatureDictionariesService nomenclatureDictionariesService;
        private Dictionary<string, int?> countriesDict;
        private Dictionary<string, Settlement> settlementsDict;
        private Dictionary<string, int?> districtDict;
        private Dictionary<string, int?> municipalityDict;
        private Dictionary<(int, Semester), int> periodIdDict;
        private Dictionary<int, int> studentStatusByEventDict;
        private Dictionary<string, int?> researchAreaByCodeDict;
        private HashSet<int> institutionSpecialityIdHashSet;
        private HashSet<int> doctoralProgrammeIdHashSet;
        private DomainValidatorService domainValidatorService;

        public TxtParserService(NomenclatureDictionariesService nomenclatureDictionariesService, DomainValidatorService domainValidatorService)
        {
            this.nomenclatureDictionariesService = nomenclatureDictionariesService;
            this.domainValidatorService = domainValidatorService;
        }

        public string ParseStreamToStringTxt(MemoryStream fileStream)
        {
            fileStream.Position = 0;
            using StreamReader reader = new StreamReader(fileStream);

            return reader.ReadToEnd();
        }

        public List<LineDto<TEnum>> ConstructLineDtoList<TEnum>(string parsedStringContent, LineDtoType lineDtoType)
            where TEnum : struct, IConvertible
        {
            var result = new List<LineDto<TEnum>>();
            var splitedLinesText = parsedStringContent.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

            int rowIndex = 1;

            foreach (var line in splitedLinesText)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var lineDto = new LineDto<TEnum>
                    {
                        LineType = lineDtoType,
                        RowIndex = rowIndex
                    };

                    var splitedLineColumnText = line.Split(new string[] { "|" }, StringSplitOptions.None).ToList();

                    int columnIndex = 0;
                    foreach (var column in splitedLineColumnText)
                    {
                        lineDto.Columns.Add(new LineColumnDto
                        {
                            Index = columnIndex,
                            Value = column.NullIfWhiteSpaceTrim()
                        }); ;

                        columnIndex++;
                    }

                    result.Add(lineDto);
                }

                rowIndex++;
            }

            return result;
        }

        public void ValidateTxtImportType(byte[] fileByteArray, ImportType importType)
        {
            using MemoryStream fileStream = new MemoryStream(fileByteArray);

            string parsedString = ParseStreamToStringTxt(fileStream);
            var linesDto = ConstructLineDtoList<TxtValidationErrorCode>(parsedString, importType == ImportType.PersonImport ? LineDtoType.NaturalPerson : LineDtoType.Speciality);
            var lineColumns = linesDto.FirstOrDefault(e => e.Columns.Any());

            if (lineColumns != null)
            {
                var firstColumnValue = lineColumns.Columns.Single(e => e.Index == 0).Value;
                var secondColumnValue = lineColumns.Columns.Single(e => e.Index == 1).Value;

                switch (importType)
                {
                    case ImportType.PersonImport:

                        if (!string.IsNullOrWhiteSpace(firstColumnValue) && ValidatePropertiesStatic.IsValidUan(firstColumnValue)
                         && !string.IsNullOrWhiteSpace(secondColumnValue) && ValidatePropertiesStatic.IsValidDateBetween(secondColumnValue))
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.RdpzsdImport_WrongPersonImportType);
                        }

                        break;
                    case ImportType.SpecialityImport:

                        var thirdColumnValue = lineColumns.Columns.Single(e => e.Index == 2).Value;
                        var fourthColumnValue = lineColumns.Columns.Single(e => e.Index == 3).Value;

                        if (string.IsNullOrWhiteSpace(firstColumnValue) || ValidatePropertiesStatic.IsValidUan(firstColumnValue) 
                          && ((!string.IsNullOrWhiteSpace(secondColumnValue) && ValidatePropertiesStatic.IsValidUinCheckSum(secondColumnValue))
                          || (!string.IsNullOrWhiteSpace(thirdColumnValue) && ValidatePropertiesStatic.IsValidForeignerNumberCheckSum(thirdColumnValue))
                          || (!string.IsNullOrWhiteSpace(fourthColumnValue) && fourthColumnValue.Length >= 6 && fourthColumnValue.Length <= 25 && ValidatePropertiesStatic.IsValidIdnNumber(fourthColumnValue))))
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.RdpzsdImport_WrongSpecialityImportType);
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        // Parse from PersonImport
        // This is used when TXT file is validated and we are sure data is OK
        public List<PersonLot> ParseToPersonLotList(string parsedStringContent, DateTime createDate, int userId, string userFullname, int institutionId, int? subordinateId)
        {
            countriesDict = nomenclatureDictionariesService.GetCountryDictAsync();
            settlementsDict = nomenclatureDictionariesService.GetSettlementDictAsync();
            districtDict = nomenclatureDictionariesService.GetDistrictDictAsync();
            municipalityDict = nomenclatureDictionariesService.GetMunicipalityDictAsync();

            var result = new List<PersonLot>();

            if (!string.IsNullOrWhiteSpace(parsedStringContent))
            {
                var splitedLinesText = parsedStringContent.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

                foreach (var lineText in splitedLinesText)
                {
                    if (!string.IsNullOrWhiteSpace(lineText))
                    {
                        var splitedLineColumnText = lineText.Split(new string[] { "|" }, StringSplitOptions.None).ToList();

                        if (splitedLineColumnText.Count == 30)
                        {
                            var personLot = ConstructPersonLot(splitedLineColumnText, createDate, userId, userFullname, institutionId, subordinateId);
                            result.Add(personLot);
                        }
                    }
                }
            }

            return result;
        }

        // Parse from SpecialityImport
        // This is used when TXT file is validated and we are sure data is OK
        public PersonStudentDoctoralTxtDto ParseToPersonStudentDoctoralTxtDtoList(string parsedStringContent, DateTime createDate, int userId, string userFullname, int institutionId, int? subordinateId)
        {
            periodIdDict = nomenclatureDictionariesService.GetPeriodIdDict();
            studentStatusByEventDict = nomenclatureDictionariesService.GetStudentStatusByEventDict();
            researchAreaByCodeDict = nomenclatureDictionariesService.GetResearchAreaByCodeDict();
            (institutionSpecialityIdHashSet, doctoralProgrammeIdHashSet) = nomenclatureDictionariesService.GetInstitutionSpecialitiesHashSet(subordinateId ?? institutionId, subordinateId.HasValue);

            var result = new PersonStudentDoctoralTxtDto();

            if (!string.IsNullOrWhiteSpace(parsedStringContent))
            {
                var splitedLinesText = parsedStringContent.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

                foreach (var lineText in splitedLinesText)
                {
                    if (!string.IsNullOrWhiteSpace(lineText))
                    {
                        var splitedLineColumnText = lineText.Split(new string[] { "|" }, StringSplitOptions.None).ToList();

                        if (splitedLineColumnText.Count == 23)
                        {
                            if (institutionSpecialityIdHashSet.Contains(int.Parse(splitedLineColumnText[2])))
                            {
                                var personStudentTxtDto = ConstructPersonStudentTxtDto(splitedLineColumnText, createDate, userId, userFullname, institutionId, subordinateId);
                                result.personStudentTxtDtos.Add(personStudentTxtDto);
                            }
                            else if (doctoralProgrammeIdHashSet.Contains(int.Parse(splitedLineColumnText[2])))
                            {
                                var personDoctoralTxtDto = ConstructPersonDoctoralTxtDto(splitedLineColumnText, createDate, userId, userFullname, institutionId, subordinateId);
                                result.personDoctoralTxtDtos.Add(personDoctoralTxtDto);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private PersonLot ConstructPersonLot(List<string> splitedLineColumnText, DateTime createDate, int userId, string userFullname, int institutionId, int? subordinateId)
        {
            var uan = splitedLineColumnText[0].NullIfWhiteSpaceTrim();
            var uin = splitedLineColumnText[1].NullIfWhiteSpaceTrim();
            var foreignerNumber = splitedLineColumnText[2].NullIfWhiteSpaceTrim();
            var idnNumber = splitedLineColumnText[3].NullIfWhiteSpaceTrim();
            var birthDate = splitedLineColumnText[4].GetDateTimeString().Value;
            var firstName = splitedLineColumnText[5].NullIfWhiteSpaceTrim();
            var middleName = splitedLineColumnText[6].NullIfWhiteSpaceTrim();
            var lastName = splitedLineColumnText[7].NullIfWhiteSpaceTrim();
            var otherNames = splitedLineColumnText[8].NullIfWhiteSpaceTrim();
            var firstNameAlt = splitedLineColumnText[9].NullIfWhiteSpaceTrim();
            var middleNameAlt = splitedLineColumnText[10].NullIfWhiteSpaceTrim();
            var lastNameAlt = splitedLineColumnText[11].NullIfWhiteSpaceTrim();
            var otherNamesAlt = splitedLineColumnText[12].NullIfWhiteSpaceTrim();
            var gender = splitedLineColumnText[13].GetEnumValueString<GenderType>();
            var email = splitedLineColumnText[14].NullIfWhiteSpaceTrim();
            var phoneNumber = Regex.Replace(splitedLineColumnText[15].NullIfWhiteSpaceTrim(), @"\s+", " ");
            var birthCountryId = countriesDict[splitedLineColumnText[16].NullIfWhiteSpaceTrim()];
            var birthSettlement = settlementsDict.GetDictValueOrNull(splitedLineColumnText[17].NullIfWhiteSpaceTrim());
            var birthSettlementId = birthSettlement?.Id;
            var birthDistrictId = splitedLineColumnText[18].NullIfWhiteSpaceTrim() == null ? birthSettlement?.DistrictId : districtDict[splitedLineColumnText[18].NullIfWhiteSpaceTrim()];
            var birthMunicipalityId = splitedLineColumnText[19].NullIfWhiteSpaceTrim() == null ? birthSettlement?.MunicipalityId : municipalityDict[splitedLineColumnText[19].NullIfWhiteSpaceTrim()];
            var foreignerBirthSettlement = splitedLineColumnText[20].NullIfWhiteSpaceTrim();
            var residenceCountryId = countriesDict[splitedLineColumnText[21].NullIfWhiteSpaceTrim()];
            var residenceSettlement = settlementsDict.GetDictValueOrNull(splitedLineColumnText[22].NullIfWhiteSpaceTrim());
            var residenceSettlementId = residenceSettlement?.Id;
            var residenceDistrictId = splitedLineColumnText[23].NullIfWhiteSpaceTrim() == null ? residenceSettlement?.DistrictId : districtDict[splitedLineColumnText[23].NullIfWhiteSpaceTrim()];
            var residenceMunicipalityId = splitedLineColumnText[24].NullIfWhiteSpaceTrim() == null ? residenceSettlement?.MunicipalityId : municipalityDict[splitedLineColumnText[24].NullIfWhiteSpaceTrim()];
            var postCode = splitedLineColumnText[25].NullIfWhiteSpaceTrim();
            var address = splitedLineColumnText[26].NullIfWhiteSpaceTrim();
            var foreignerAddress = splitedLineColumnText[27].NullIfWhiteSpaceTrim();
            var citizenshipId = countriesDict[splitedLineColumnText[28].NullIfWhiteSpaceTrim()].Value;
            var secondCitizenshipId = countriesDict.GetDictValueOrNull(splitedLineColumnText[29].NullIfWhiteSpaceTrim());

            bool isForApproval = string.IsNullOrWhiteSpace(uin)
                    && string.IsNullOrWhiteSpace(foreignerNumber)
                    && !string.IsNullOrWhiteSpace(idnNumber);

            var personLot = new PersonLot
            {
                CreateDate = createDate,
                CreateInstitutionId = institutionId,
                CreateSubordinateId = subordinateId,
                CreateUserId = userId,
                Uan = uan,
                State = isForApproval ? LotState.MissingPassportCopy : LotState.Actual,
                PersonLotActions = new List<PersonLotAction>
                {
                    new PersonLotAction
                        {
                            ActionDate = createDate,
                            InstitutionId = institutionId,
                            ActionType = isForApproval ? PersonLotActionType.CreateUnverifiedTxt :
                                string.IsNullOrWhiteSpace(uan) ? PersonLotActionType.CreateTxt : PersonLotActionType.PersonBasicEditTxt,
                            SubordinateId = subordinateId,
                            UserFullname = userFullname,
                            UserId = userId
                        }
                },
                PersonBasic = new PersonBasic
                {
                    State = PartState.Actual,
                    Uin = uin,
                    ForeignerNumber = foreignerNumber,
                    IdnNumber = idnNumber,
                    BirthDate = birthDate,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    OtherNames = otherNames,
                    FullName = firstName + (!string.IsNullOrWhiteSpace(middleName) ? $" {middleName}" : string.Empty) + $" {lastName}" + (!string.IsNullOrWhiteSpace(otherNames) ? $" {otherNames}" : string.Empty),
                    FirstNameAlt = firstNameAlt,
                    MiddleNameAlt = middleNameAlt,
                    LastNameAlt = lastNameAlt,
                    OtherNamesAlt = otherNamesAlt,
                    FullNameAlt = firstNameAlt + (!string.IsNullOrWhiteSpace(middleNameAlt) ? $" {middleNameAlt}" : string.Empty) + $" {lastNameAlt}" + (!string.IsNullOrWhiteSpace(otherNamesAlt) ? $" {otherNamesAlt}" : string.Empty),
                    Gender = gender,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    BirthCountryId = birthCountryId,
                    BirthSettlementId = birthSettlementId,
                    BirthDistrictId = birthDistrictId,
                    BirthMunicipalityId = birthMunicipalityId,
                    ForeignerBirthSettlement = foreignerBirthSettlement,
                    ResidenceCountryId = residenceCountryId,
                    ResidenceSettlementId = residenceSettlementId,
                    ResidenceDistrictId = residenceDistrictId,
                    ResidenceMunicipalityId = residenceMunicipalityId,
                    PostCode = postCode,
                    ResidenceAddress = address ?? foreignerAddress,
                    CitizenshipId = citizenshipId,
                    SecondCitizenshipId = secondCitizenshipId,
                    PartInfo = new PersonBasicInfo
                    {
                        ActionDate = createDate,
                        InstitutionId = institutionId,
                        SubordinateId = subordinateId,
                        UserFullname = userFullname,
                        UserId = userId
                    }
                }
            };

            return personLot;
        }

        private PersonStudentTxtDto ConstructPersonStudentTxtDto(List<string> splitedLineColumnText, DateTime createDate, int userId, string userFullname, int institutionId, int? subordinateId)
        {
            var uan = splitedLineColumnText[0].NullIfWhiteSpaceTrim();
            var birthDate = splitedLineColumnText[1].GetDateTimeString().Value;
            var institutionSpecialityId = int.Parse(splitedLineColumnText[2]);
            var periodYear = int.Parse(splitedLineColumnText[3]);
            var periodSemester = splitedLineColumnText[4].GetEnumValueString<Semester>();
            var periodId = periodIdDict[(periodYear, periodSemester)];
            var studentEventId = int.Parse(splitedLineColumnText[5]);
            var studentStatusId = studentStatusByEventDict[studentEventId];
            var course = splitedLineColumnText[6].GetEnumValueString<CourseType>();
            var studentSemester = splitedLineColumnText[7].GetEnumValueString<Semester>();
            var educationFeeTypeId = splitedLineColumnText[8].GetNullableIntFromString();
            var admissionReasonId = splitedLineColumnText[9].GetNullableIntFromString();
            var facultyNumber = splitedLineColumnText[10].NullIfWhiteSpaceTrim();
            PreviousEducationType? peType = splitedLineColumnText[11].TryParseEnumValue<PreviousEducationType>() ? splitedLineColumnText[11].GetEnumValueString<PreviousEducationType>() : null;
            PreviousHighSchoolEducationTypeTxt? previousHighSchoolEducationTypeTxt = splitedLineColumnText[12].TryParseEnumValue<PreviousHighSchoolEducationTypeTxt>() ? splitedLineColumnText[12].GetEnumValueString<PreviousHighSchoolEducationTypeTxt>() : null;
            PreviousHighSchoolEducationType? peHighSchoolType = previousHighSchoolEducationTypeTxt == PreviousHighSchoolEducationTypeTxt.MissingInRegister
                ? PreviousHighSchoolEducationType.MissingInRegister
                : previousHighSchoolEducationTypeTxt == PreviousHighSchoolEducationTypeTxt.Abroad
                    ? PreviousHighSchoolEducationType.Abroad
                    : null;
            var peResearchAreaId = researchAreaByCodeDict.GetDictValueOrNull(splitedLineColumnText[13].NullIfWhiteSpaceTrim());
            var note = splitedLineColumnText[14].NullIfWhiteSpaceTrim();
            var relocatedFromInstitutionSpecialityId = splitedLineColumnText[15].GetNullableIntFromString();
            var semesterRelocatedNumber = splitedLineColumnText[16].NullIfWhiteSpaceTrim();
            var semesterRelocatedDate = splitedLineColumnText[17].GetDateTimeString();
            var hasScholarship = splitedLineColumnText[18].GetBooleanFromString();
            var useHostel = splitedLineColumnText[19].GetBooleanFromString();
            var useHolidayBase = splitedLineColumnText[20].GetBooleanFromString();
            var participatedIntPrograms = splitedLineColumnText[21].GetBooleanFromString();
            var actionState = splitedLineColumnText[22].GetEnumValueString<SpecialityImportAction>();

            var personStudentTxtDto = new PersonStudentTxtDto
            {
                ActionState = actionState,
                AdmissionReasonId = admissionReasonId,
                BirthDate = birthDate,
                Course = course,
                CreateDate = createDate,
                EducationFeeTypeId = educationFeeTypeId,
                FacultyNumber = facultyNumber,
                HasScholarship = hasScholarship,
                CreateInstitutionId = institutionId,
                InstitutionSpecialityId = institutionSpecialityId,
                Note = note,
                ParticipatedIntPrograms = participatedIntPrograms,
                PeHighSchoolType = peHighSchoolType,
                PeResearchAreaId = peResearchAreaId,
                PeriodId = periodId,
                PeType = peType,
                RelocatedFromInstitutionSpecialityId = relocatedFromInstitutionSpecialityId,
                SemesterRelocatedDate = semesterRelocatedDate,
                SemesterRelocatedNumber = semesterRelocatedNumber,
                StudentEventId = studentEventId,
                StudentSemester = studentSemester,
                StudentStatusId = studentStatusId,
                CreateSubordinateId = subordinateId,
                Uan = uan,
                UseHolidayBase = useHolidayBase,
                UseHostel = useHostel,
                UserFullname = userFullname,
                UserId = userId
            };

            return personStudentTxtDto;
        }

        private PersonDoctoralTxtDto ConstructPersonDoctoralTxtDto(List<string> splitedLineColumnText, DateTime createDate, int userId, string userFullname, int institutionId, int? subordinateId)
        {
            var uan = splitedLineColumnText[0].NullIfWhiteSpaceTrim();
            var birthDate = splitedLineColumnText[1].GetDateTimeString().Value;
            var doctoralProgrammeId = int.Parse(splitedLineColumnText[2]);
            var protocolNumber = splitedLineColumnText[3].NullIfWhiteSpaceTrim();
            var protocolDate = splitedLineColumnText[4].GetDateTimeString().Value;
            var studentEventId = int.Parse(splitedLineColumnText[5]);
            var studentStatusId = studentStatusByEventDict[studentEventId];
            var yearType = splitedLineColumnText[6].GetEnumValueString<YearType>();
            AttestationType? atestationType = splitedLineColumnText[7].TryParseEnumValue<AttestationType>() ? splitedLineColumnText[7].GetEnumValueString<AttestationType>() : null;
            var educationFeeTypeId = splitedLineColumnText[8].GetNullableIntFromString();
            var admissionReasonId = splitedLineColumnText[9].GetNullableIntFromString();
            var startDate = splitedLineColumnText[10].GetDateTimeString();
            var endDate = splitedLineColumnText[11].GetDateTimeString(); 
            PreviousHighSchoolEducationTypeTxt? previousHighSchoolEducationTypeTxt = splitedLineColumnText[12].TryParseEnumValue<PreviousHighSchoolEducationTypeTxt>() ? splitedLineColumnText[12].GetEnumValueString<PreviousHighSchoolEducationTypeTxt>() : null;
            PreviousHighSchoolEducationType? peHighSchoolType = previousHighSchoolEducationTypeTxt == PreviousHighSchoolEducationTypeTxt.MissingInRegister
                ? PreviousHighSchoolEducationType.MissingInRegister
                : previousHighSchoolEducationTypeTxt == PreviousHighSchoolEducationTypeTxt.Abroad
                    ? PreviousHighSchoolEducationType.Abroad
                    : null;
            var peResearchAreaId = researchAreaByCodeDict.GetDictValueOrNull(splitedLineColumnText[13].NullIfWhiteSpaceTrim());
            var note = splitedLineColumnText[14].NullIfWhiteSpaceTrim();
            var relocatedFromInstitutionSpecialityId = splitedLineColumnText[15].GetNullableIntFromString();
            var semesterRelocatedNumber = splitedLineColumnText[16].NullIfWhiteSpaceTrim();
            var semesterRelocatedDate = splitedLineColumnText[17].GetDateTimeString();
            var hasScholarship = splitedLineColumnText[18].GetBooleanFromString();
            var useHostel = splitedLineColumnText[19].GetBooleanFromString();
            var useHolidayBase = splitedLineColumnText[20].GetBooleanFromString();
            var participatedIntPrograms = splitedLineColumnText[21].GetBooleanFromString();
            var actionState = splitedLineColumnText[22].GetEnumValueString<SpecialityImportAction>();

            var personStudentTxtDto = new PersonDoctoralTxtDto
            {
                ActionState = actionState,
                Atestation = atestationType,
                EndDate = endDate,
                ProtocolDate = protocolDate,
                ProtocolNumber = protocolNumber,
                StartDate = startDate,
                YearType = yearType,
                AdmissionReasonId = admissionReasonId,
                BirthDate = birthDate,
                CreateDate = createDate,
                EducationFeeTypeId = educationFeeTypeId,
                HasScholarship = hasScholarship,
                CreateInstitutionId = institutionId,
                DoctoralProgrammeId = doctoralProgrammeId,
                Note = note,
                ParticipatedIntPrograms = participatedIntPrograms,
                PeHighSchoolType = peHighSchoolType,
                PeResearchAreaId = peResearchAreaId,
                RelocatedFromInstitutionSpecialityId = relocatedFromInstitutionSpecialityId,
                SemesterRelocatedDate = semesterRelocatedDate,
                SemesterRelocatedNumber = semesterRelocatedNumber,
                StudentEventId = studentEventId,
                StudentStatusId = studentStatusId,
                CreateSubordinateId = subordinateId,
                Uan = uan,
                UseHolidayBase = useHolidayBase,
                UseHostel = useHostel,
                UserFullname = userFullname,
                UserId = userId
            };

            return personStudentTxtDto;
        }
    }
}
