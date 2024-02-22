using Infrastructure.Constants;
using Infrastructure.Extensions;
using Rdpzsd.Import.Services.TxtValidation.SpecialitySemesterTxtValidation;
using Rdpzsd.Import.Services.TxtValidation.SpecialitySemesterValidation;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.RdpzsdImport;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Import.Services.TxtValidation
{
    public class SpecialityImportTxtValidationService
    {
        private readonly AddSpecialitySemesterTxtValidationService addSpecialitySemesterTxtValidationService;
        private readonly EditSpecialitySemesterTxtValidationService editSpecialitySemesterTxtValidationService;
        private readonly EraseSpecialitySemesterTxtValidationService eraseSpecialitySemesterTxtValidationService;
        private readonly NomenclatureDictionariesService nomenclatureDictionariesService;
        private Dictionary<(string, DateTime), PersonLot> personUanStudentDict;
        private Dictionary<int, InstitutionSpeciality> institutionSpecialityDict;
        private Dictionary<int, InstitutionSpeciality> doctoralProgrammeDict;
        private Dictionary<int, AdmissionReason> studentAdmissionReasonDict;
        private Dictionary<int, AdmissionReason> doctoralAdmissionReasonDict;
        private HashSet<int> educationalFeeHashSet;
        private HashSet<(int, Semester)> periodHashSet;
        private HashSet<int> studentEventHashSet;
        private HashSet<int> doctoralEventHashSet;
        private HashSet<string> researchAreaCodeHashSet;
        private HashSet<(string, string)> processedUanInstitutionSpecialityHashSet = new HashSet<(string, string)>();

        public SpecialityImportTxtValidationService(
            AddSpecialitySemesterTxtValidationService addSpecialitySemesterTxtValidationService,
            EditSpecialitySemesterTxtValidationService editSpecialitySemesterTxtValidationService,
            EraseSpecialitySemesterTxtValidationService eraseSpecialitySemesterTxtValidationService,
            NomenclatureDictionariesService nomenclatureDictionariesService
            )
        {
            this.addSpecialitySemesterTxtValidationService = addSpecialitySemesterTxtValidationService;
            this.editSpecialitySemesterTxtValidationService = editSpecialitySemesterTxtValidationService;
            this.eraseSpecialitySemesterTxtValidationService = eraseSpecialitySemesterTxtValidationService;
            this.nomenclatureDictionariesService = nomenclatureDictionariesService;
        }

        private void LoadDictionaries()
        {
            periodHashSet = nomenclatureDictionariesService.GetPeriodHashSet();
            (studentEventHashSet, doctoralEventHashSet) = nomenclatureDictionariesService.GetStudentEventHashSet();
            (studentAdmissionReasonDict, doctoralAdmissionReasonDict) = nomenclatureDictionariesService.GetAdmissionReasonDict();
            educationalFeeHashSet = nomenclatureDictionariesService.GetEducationalFeeHashSet();
            researchAreaCodeHashSet = nomenclatureDictionariesService.GetResearchAreaCodeHashSet();
        }

        public List<LineDto<TxtSpecValidationErrorCode>> Validate(List<LineDto<TxtSpecValidationErrorCode>> lineDtos, int institutionId, bool isSubordinate)
        {
            LoadDictionaries();

            var txtUans = lineDtos
                .Select(e => e.Columns.FirstOrDefault(s => s.Index == 0)?.Value)?
                .Where(e => e?.Length == 7)?
                .ToList();

            // Get PersonLots from Uan's in txt file
            personUanStudentDict = nomenclatureDictionariesService.GetPersonLotStudentByUansDict(txtUans);
            // Get all specialities for this Institution
            (institutionSpecialityDict, doctoralProgrammeDict) = nomenclatureDictionariesService.GetInstitutionSpecialitiesDict(institutionId, isSubordinate);

            foreach (var line in lineDtos)
            {
                if (line.Columns.Count != 23)
                {
                    line.ErrorCodes.Add(TxtSpecValidationErrorCode.WrongColumnCount);
                    continue;
                }

                line.ErrorCodes.AddRange(ValidateColumns(line));
            }

            return lineDtos;
        }

        private List<TxtSpecValidationErrorCode> ValidateColumns(LineDto<TxtSpecValidationErrorCode> lineDto)
        {
            var txtValidationErrorCodes = new List<TxtSpecValidationErrorCode>();

            var lineColumnDtos = lineDto.Columns;

            var uanValue = lineColumnDtos.Single(e => e.Index == 0).Value;
            var birthDateValue = lineColumnDtos.Single(e => e.Index == 1).Value;
            var specialityIdValue = lineColumnDtos.Single(e => e.Index == 2).Value;

            #region Check for duplicate Uan/InstitutionSpeciality

            if (processedUanInstitutionSpecialityHashSet.Contains((uanValue, specialityIdValue)))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DuplicateUanInstitutionSpeciality);
            }

            #endregion

            #region Uan

            PersonLot personLot = null;

            if (string.IsNullOrWhiteSpace(uanValue) || string.IsNullOrWhiteSpace(birthDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.UanAndBirthDateRequired);
            }
            else if (!ValidatePropertiesStatic.IsValidDateBetween(birthDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidBirthDateFormat);
            }
            else
            {
                personLot = personUanStudentDict.GetValueOrDefault((uanValue, birthDateValue.GetDateTimeString().Value.Date));

                if (personLot == null)
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.MissingPersonUan);
                }
            }

            #endregion

            #region InstitutionSpeciality permissions for user institution.

            if (string.IsNullOrWhiteSpace(specialityIdValue) || !ValidatePropertiesStatic.NumbersOnly(specialityIdValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidSpecialityNumberType);
            }
            else
            {
                var institutionSpeciality = institutionSpecialityDict.GetValueOrDefault(int.Parse(specialityIdValue));

                if (institutionSpeciality != null)
                {
                    #region Course and Semester check

                    var courseTypeValue = lineColumnDtos.Single(e => e.Index == 6).Value;
                    var studentSemesterValue = lineColumnDtos.Single(e => e.Index == 7).Value;

                    bool checkDurationCourse = true;

                    if (string.IsNullOrWhiteSpace(courseTypeValue) || !courseTypeValue.TryParseEnumValue<CourseType>())
                    {
                        checkDurationCourse = false;
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidCourseType);
                    }

                    if (string.IsNullOrWhiteSpace(studentSemesterValue) || !studentSemesterValue.TryParseEnumValue<Semester>())
                    {
                        checkDurationCourse = false;
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidStudentSemester);
                    }

                    if (!institutionSpeciality.Duration.HasValue)
                    {
                        checkDurationCourse = false;
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InstitutionSpecialityHasNoDuration);
                    }

                    if (checkDurationCourse)
                    {
                        var institutionSpecialityDuration = institutionSpeciality.Duration.Value;
                        var courseType = decimal.Parse(courseTypeValue);
                        var studentSemester = studentSemesterValue.GetEnumValueString<Semester>();

                        if (institutionSpecialityDuration % 1 == 0)
                        {
                            if (courseType > institutionSpecialityDuration)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseGreaterThanSpecialityDuration);
                            }
                        }
                        else
                        {
                            if (courseType > Math.Ceiling(institutionSpecialityDuration)
                                || (courseType == Math.Ceiling(institutionSpecialityDuration) && studentSemester == Semester.Second))
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseGreaterThanSpecialityDuration);
                            }
                        }
                    }
                    #endregion

                    #region Period check

                    var periodYearValue = lineColumnDtos.Single(e => e.Index == 3).Value;
                    var periodSemesterValue = lineColumnDtos.Single(e => e.Index == 4).Value;

                    if (string.IsNullOrWhiteSpace(periodYearValue)
                        || string.IsNullOrWhiteSpace(periodSemesterValue)
                        || !ValidatePropertiesStatic.NumbersOnly(periodYearValue)
                        || !periodSemesterValue.TryParseEnumValue<Semester>()
                        || !periodHashSet.Contains((int.Parse(periodYearValue), periodSemesterValue.GetEnumValueString<Semester>())))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidPeriod);
                    }

                    #endregion

                    #region StudentEvent not null check
                    var studentEventValue = lineColumnDtos.Single(e => e.Index == 5).Value;

                    if (string.IsNullOrWhiteSpace(studentEventValue)
                        || !ValidatePropertiesStatic.NumbersOnly(studentEventValue)
                        || !studentEventHashSet.Contains(int.Parse(studentEventValue)))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidStudentEvent);
                    }
                    #endregion
                }
                else
                {
                    var doctoralProgramme = doctoralProgrammeDict.GetDictValueOrNull(int.Parse(specialityIdValue));

                    if (doctoralProgramme != null)
                    {
                        #region Doctoral year check

                        var doctoralYearValue = lineColumnDtos.Single(e => e.Index == 6).Value;

                        bool checkDurationDoctoralYear = true;

                        if (string.IsNullOrWhiteSpace(doctoralYearValue) || !doctoralYearValue.TryParseEnumValue<YearType>())
                        {
                            checkDurationDoctoralYear = false;
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidDoctoralYear);
                        }

                        if (!doctoralProgramme.Duration.HasValue)
                        {
                            checkDurationDoctoralYear = false;
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InstitutionSpecialityHasNoDuration);
                        }

                        if (checkDurationDoctoralYear)
                        {
                            var doctoralProgrammeDuration = doctoralProgramme.Duration.Value;
                            var doctoralYear = decimal.Parse(doctoralYearValue);

                            if (doctoralYear > doctoralProgrammeDuration)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralYearGreaterThanProgrammeDuration);
                            }
                        }
                        #endregion

                        #region Protocol number and date check

                        var protocolNumberValue = lineColumnDtos.Single(e => e.Index == 3).Value;
                        var protocolDateValue = lineColumnDtos.Single(e => e.Index == 4).Value;
                        var currentYear = DateTime.Now.Year;

                        if (string.IsNullOrWhiteSpace(protocolNumberValue) || protocolNumberValue.Length > 15)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidProtocolNumber);
                        }

                        if (string.IsNullOrWhiteSpace(protocolDateValue)
                            || !ValidatePropertiesStatic.IsValidDateBetween(protocolDateValue, 1970, currentYear))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidProtocolDate);
                        }
                        #endregion

                        #region DoctoralEvent not null check
                        var doctoralEventValue = lineColumnDtos.Single(e => e.Index == 5).Value;

                        if (string.IsNullOrWhiteSpace(doctoralEventValue)
                            || !ValidatePropertiesStatic.NumbersOnly(doctoralEventValue)
                            || !doctoralEventHashSet.Contains(int.Parse(doctoralEventValue)))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidStudentEvent);
                        }
                        #endregion

                        lineDto.LineType = LineDtoType.DoctoralProgramme;
                    }
                    else
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.WrongSpecialityNumberOrNoPermissions);
                    }
                }
            }

            #endregion

            #region Valid booleans (HasScholarship, UseHostel, UseHolidayBase, ParticipatedIntPrograms)
            var hasScholarshipValue = lineColumnDtos.Single(e => e.Index == 18).Value;
            var useHostelValue = lineColumnDtos.Single(e => e.Index == 19).Value;
            var useHolidayBaseValue = lineColumnDtos.Single(e => e.Index == 20).Value;
            var participatedIntProgramsValue = lineColumnDtos.Single(e => e.Index == 21).Value;

            if (string.IsNullOrWhiteSpace(hasScholarshipValue) || !ValidatePropertiesStatic.IsValidBooleanNumber(hasScholarshipValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidHasScholarshipBoolean);
            }

            if (string.IsNullOrWhiteSpace(useHostelValue) || !ValidatePropertiesStatic.IsValidBooleanNumber(useHostelValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidUseHostelBoolean);
            }

            if (string.IsNullOrWhiteSpace(useHolidayBaseValue) || !ValidatePropertiesStatic.IsValidBooleanNumber(useHolidayBaseValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidUseHolidayBaseBoolean);
            }

            if (string.IsNullOrWhiteSpace(participatedIntProgramsValue) || !ValidatePropertiesStatic.IsValidBooleanNumber(participatedIntProgramsValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidParticipatedIntProgramsBoolean);
            }
            #endregion

            #region ImportSpecialityAction try parse to enum

            var importSpecialityActionValue = lineColumnDtos.Single(e => e.Index == 22).Value;

            if (string.IsNullOrWhiteSpace(importSpecialityActionValue) || !importSpecialityActionValue.TryParseEnumValue<SpecialityImportAction>())
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidImportSpecialityAction);
            }

            #endregion

            #region If there are no errors continue custom validations by SpecialityImportAction
            if (!txtValidationErrorCodes.Any())
            {
                var specialityImportAction = importSpecialityActionValue.GetEnumValueString<SpecialityImportAction>();

                if (lineDto.LineType == LineDtoType.DoctoralProgramme)
                {
                    #region Doctoral programme check
                    var personDoctoral = personLot.PersonDoctorals
                        .OrderByDescending(e => e.Id)
                        .FirstOrDefault(e => e.InstitutionSpecialityId == int.Parse(specialityIdValue) && e.State != PartState.Erased);

                    switch (specialityImportAction)
                    {
                        case SpecialityImportAction.Add:
                            if (personDoctoral != null)
                            {
                                addSpecialitySemesterTxtValidationService.AddDoctoralSemesterValidation(personDoctoral, lineColumnDtos, txtValidationErrorCodes, educationalFeeHashSet);
                            }
                            else
                            {
                                var personCompletedRelocatedDoctorals = personLot.PersonDoctorals
                                    .Where(e => e.StudentEventId == 304 && e.State != PartState.Erased);

                                addSpecialitySemesterTxtValidationService.InitialDoctoralValidation(personLot, lineColumnDtos, txtValidationErrorCodes, doctoralAdmissionReasonDict, educationalFeeHashSet, researchAreaCodeHashSet, personCompletedRelocatedDoctorals);
                            }
                            break;
                        case SpecialityImportAction.Edit:
                            if (personDoctoral.Semesters.Count > 1)
                            {
                                editSpecialitySemesterTxtValidationService.EditDoctoralSemesterValidation(lineColumnDtos, txtValidationErrorCodes);
                            }
                            else
                            {
                                editSpecialitySemesterTxtValidationService.EditDoctoralProgrammeValidation(lineColumnDtos, txtValidationErrorCodes);
                            }
                            break;
                        case SpecialityImportAction.Erase:
                            if (personDoctoral == null)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InstitutionSpecialityNotFoundForErase);
                            }
                            else if (personDoctoral.Semesters.Count > 1)
                            {
                                eraseSpecialitySemesterTxtValidationService.EraseDoctoralSemesterValidation(personDoctoral, lineColumnDtos, txtValidationErrorCodes);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                else
                {
                    #region Speciality check

                    var personStudent = personLot.PersonStudents
                        .OrderByDescending(e => e.Id)
                        .FirstOrDefault(e => e.InstitutionSpecialityId == int.Parse(specialityIdValue) && e.State != PartState.Erased);

                    switch (specialityImportAction)
                    {
                        case SpecialityImportAction.Add:
                            if (personStudent != null)
                            {
                                addSpecialitySemesterTxtValidationService.AddSemesterValidation(personStudent, lineColumnDtos, txtValidationErrorCodes, educationalFeeHashSet);
                            }
                            else
                            {
                                var institutionSpeciality = institutionSpecialityDict.GetValueOrDefault(int.Parse(specialityIdValue));

                                var personCompletedRelocatedSpecialities = personLot.PersonStudents
                                    .Where(e => e.StudentEventId == 304 && e.State != PartState.Erased);

                                addSpecialitySemesterTxtValidationService.InitialSpecialityValidation(personLot, lineColumnDtos, txtValidationErrorCodes, studentAdmissionReasonDict, educationalFeeHashSet, researchAreaCodeHashSet, institutionSpeciality.Speciality.EducationalQualification.Alias, personCompletedRelocatedSpecialities);
                            }
                            break;
                        case SpecialityImportAction.Edit:
                            if (personStudent.Semesters.Count > 1)
                            {
                                editSpecialitySemesterTxtValidationService.EditSemesterValidation(lineColumnDtos, txtValidationErrorCodes);
                            }
                            else
                            {
                                editSpecialitySemesterTxtValidationService.EditSpecialityValidation(lineColumnDtos, txtValidationErrorCodes);
                            }
                            break;
                        case SpecialityImportAction.Erase:
                            if (personStudent == null)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InstitutionSpecialityNotFoundForErase);
                            }
                            else if (personStudent.Semesters.Count > 1)
                            {
                                eraseSpecialitySemesterTxtValidationService.EraseSemesterValidation(personStudent, lineColumnDtos, txtValidationErrorCodes);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }
            #endregion

            processedUanInstitutionSpecialityHashSet.Add((uanValue, specialityIdValue));

            return txtValidationErrorCodes;
        }
    }
}
