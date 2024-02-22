using Infrastructure.Constants;
using Infrastructure.Extensions;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Nomenclature.Country;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Import.Services.TxtValidation.SpecialitySemesterValidation
{
    public class AddSpecialitySemesterTxtValidationService
    {
        private HashSet<int> initialSpecialityStudentEventHashSet = new HashSet<int> { 100, 104, 105, 106, 107 };
        private HashSet<int> activeStudentEventHashSet = new HashSet<int> { 101, 103, 108, 110, 111, 112, 113 };
        private HashSet<int> activeAfterActiveStudentEventHashSet = new HashSet<int> { 101, 108, 110, 111, 113 };
        private HashSet<int> activeAfterInterruptedStudentEventHashSet = new HashSet<int> { 103, 108, 111, 112 };
        private HashSet<int> interruptedStudentEventHashSet = new HashSet<int> { 201, 202, 203, 204, 205, 206, 207, 208 };
        private HashSet<int> completedStudentEventHashSet = new HashSet<int> { 303, 304, 305, 307, 308, 309, 310 };
        private HashSet<int> processGraduationStudentEventHashSet = new HashSet<int> { 401, 403, 404, 405, 406 };

        private HashSet<int> initialDoctoralStudentEventHashSet = new HashSet<int> { 100, 104, 105, 106 };
        private HashSet<int> activeDoctoralStudentEventHashSet = new HashSet<int> { 102, 103, 108, 109, 114 };
        private HashSet<int> activeDoctoralAfterActiveStudentEventHashSet = new HashSet<int> { 102, 108, 109, 114 };
        private HashSet<int> activeDoctoralAfterInterruptedStudentEventHashSet = new HashSet<int> { 103, 108 };
        private HashSet<int> interruptedDoctoralStudentEventHashSet = new HashSet<int> { 201, 202, 203, 204, 205, 206, 207, 208 };
        private HashSet<int> completedDoctoralStudentEventHashSet = new HashSet<int> { 303, 304, 305, 307, 308, 309, 310 };
        private HashSet<int> processGraduationDoctoralStudentEventHashSet = new HashSet<int> { 402, 403, 404, 405, 406 };

        public void AddSemesterValidation(PersonStudent personStudent, List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes, HashSet<int> educationalFeeHashSet)
        {
            var periodYear = int.Parse(lineColumnDtos.Single(e => e.Index == 3).Value);
            var periodSemester = lineColumnDtos.Single(e => e.Index == 4).Value.GetEnumValueString<Semester>();
            var studentEventId = int.Parse(lineColumnDtos.Single(e => e.Index == 5).Value);
            var courseType = lineColumnDtos.Single(e => e.Index == 6).Value.GetEnumValueString<CourseType>();
            var studentSemester = lineColumnDtos.Single(e => e.Index == 7).Value.GetEnumValueString<Semester>();
            var educationalFeeValue = lineColumnDtos.Single(e => e.Index == 8).Value;
            var noteValue = lineColumnDtos.Single(e => e.Index == 14).Value;

            var admissionReasonValue = lineColumnDtos.Single(e => e.Index == 9).Value;
            var facultyNumberValue = lineColumnDtos.Single(e => e.Index == 10).Value;
            var peTypeValue = lineColumnDtos.Single(e => e.Index == 11).Value;
            var peHighSchoolTypeValue = lineColumnDtos.Single(e => e.Index == 12).Value;
            var peResearchAreaValue = lineColumnDtos.Single(e => e.Index == 13).Value;
            var relocatedFromPartValue = lineColumnDtos.Single(e => e.Index == 15).Value;
            var relocatedDocumentNumberValue = lineColumnDtos.Single(e => e.Index == 16).Value;
            var relocatedDocumentDateValue = lineColumnDtos.Single(e => e.Index == 17).Value;

            var hasScholarship = lineColumnDtos.Single(e => e.Index == 18).Value.GetBooleanFromString();
            var useHostel = lineColumnDtos.Single(e => e.Index == 19).Value.GetBooleanFromString();
            var useHolidayBase = lineColumnDtos.Single(e => e.Index == 20).Value.GetBooleanFromString();
            var participatedIntPrograms = lineColumnDtos.Single(e => e.Index == 21).Value.GetBooleanFromString();

            var lastStudentSemester = personStudent.Semesters
                    .OrderByDescending(e => e.Period.Year)
                    .ThenByDescending(e => e.Period.Semester)
                    .ThenByDescending(e => e.StudentStatusId)
                    .ThenByDescending(e => e.Id)
                    .First();

            #region Must be empty values

            if (!string.IsNullOrWhiteSpace(admissionReasonValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.AdmissionReasonMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(facultyNumberValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.FacultyNumberMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(peTypeValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeTypeMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(peHighSchoolTypeValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeHighSchoolMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(peResearchAreaValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeResearchAreaMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedFromPartValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedFromMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedDocumentNumberValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentNumberMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedDocumentDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentDateMustBeEmpty);
            }
            #endregion

            // Note
            if (!string.IsNullOrWhiteSpace(noteValue) && noteValue.Length > 500)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.NoteLength);
            }

            if (personStudent.StudentStatus.Alias == StudentStatusConstants.Graduated)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CannotAddSemesterToGraduated);
            }
            else if (personStudent.StudentStatus.Alias == StudentStatusConstants.Completed)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CannotAddSemesterToCompleted);
            }
            else
            {
                #region StudentEvent - Active
                if (activeStudentEventHashSet.Contains(studentEventId))
                {
                    if (string.IsNullOrWhiteSpace(educationalFeeValue) || !ValidatePropertiesStatic.NumbersOnly(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EducationalFeeMustHaveValue);
                    }
                    else
                    {
                        var educationalFeeId = int.Parse(educationalFeeValue);

                        if (!educationalFeeHashSet.Contains(educationalFeeId))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.MissingEducationFee);
                        }
                    }

                    var periodYearToCheck = lastStudentSemester.Period.Year;
                    var periodSemesterToCheck = lastStudentSemester.Period.Semester;

                    if (periodSemesterToCheck == Semester.First)
                    {
                        periodSemesterToCheck = Semester.Second;
                    }
                    else
                    {
                        periodYearToCheck++;
                        periodSemesterToCheck = Semester.First;
                    }

                    if (lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.Active && activeAfterActiveStudentEventHashSet.Contains(studentEventId))
                    {
                        var courseTypeToCheck = (lastStudentSemester.StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoSemesters || (lastStudentSemester.StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoYears && lastStudentSemester.SecondFromTwoYearsPlan))
                            ? lastStudentSemester.IndividualPlanCourse
                            : lastStudentSemester.Course;
                        var studentSemesterToCheck = (lastStudentSemester.StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoSemesters || (lastStudentSemester.StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoYears && lastStudentSemester.SecondFromTwoYearsPlan))
                            ? lastStudentSemester.IndividualPlanSemester
                            : lastStudentSemester.StudentSemester;

                        var institutionSpeciality = personStudent.InstitutionSpeciality;
                        var institutionSpecialityDuration = institutionSpeciality.Duration.Value;
                        var isLastSemesterForInsert = (institutionSpecialityDuration % 1 == 0
                                && (int)courseType == (int)institutionSpecialityDuration
                                && studentSemester == Semester.Second)
                            || (institutionSpecialityDuration % 1 != 0
                                && Math.Ceiling(institutionSpecialityDuration) == (int)courseType
                                && studentSemester == Semester.First);

                        if (studentEventId != 113)
                        {
                            if (studentSemesterToCheck == Semester.First)
                            {
                                studentSemesterToCheck = Semester.Second;
                            }
                            else
                            {
                                courseTypeToCheck++;
                                studentSemesterToCheck = Semester.First;
                            }
                        }

                        // Check if it's two semesters/years for one and speciality is regulated
                        if (institutionSpeciality.Speciality.IsRegulated && (studentEventId == 110 || studentEventId == 111))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RegulatedSpecialityHasNoTwoSemestersForOne);
                        }

                        // Check if two semesters/years for one is in course > 1 and course < last
                        if ((studentEventId == 110 || studentEventId == 111)
                            && ((int)courseType >= Math.Ceiling(institutionSpeciality.Duration.Value) || (int)courseType < 2))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.TwoSemestersYearsForOneCantBeFirstOrLastSemester);
                        }

                        // Check if it's two years for one and it has two years for one first semester before this
                        if (lastStudentSemester.StudentEvent?.Alias == StudentEventConstants.IndividualPlanTwoYears && !lastStudentSemester.SecondFromTwoYearsPlan && studentEventId != 110)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.LastSemesterIsIndividualPlanTwoYears);
                        }

                        // Check if speciality is last semester and event is different from 113 or it's selected 113 and it's not last semester
                        if (isLastSemesterForInsert && studentEventId != 113)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.StudentInLastSemesterMustBe113IfActive);
                        }
                        else if (!isLastSemesterForInsert && studentEventId == 113)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.StudentNotInLastSemesterMustBeDifferent113IfActive);
                        }

                        // Check period if it's ok
                        if (periodYearToCheck != periodYear || periodSemesterToCheck != periodSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeNextForActiveStatus);
                        }

                        // Check courseType and studentSemester if they are ok
                        if (courseTypeToCheck != courseType || studentSemesterToCheck != studentSemester)
                        {
                            if (studentEventId == 113)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeSameForActiveStatus113);
                            }
                            else
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeNextForActiveStatus);
                            }
                        }
                    }
                    else if (lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.Interrupted && activeAfterInterruptedStudentEventHashSet.Contains(studentEventId))
                    {
                        var courseTypeToCheck = lastStudentSemester.Course;
                        var studentSemesterToCheck = lastStudentSemester.StudentSemester;

                        var institutionSpeciality = personStudent.InstitutionSpeciality;

                        // Check if it's two semesters for one and speciality is regulated
                        if (institutionSpeciality.Speciality.IsRegulated && studentEventId == 111)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RegulatedSpecialityHasNoTwoSemestersForOne);
                        }

                        // Check if two semesters for one is in course > 1 and course < last
                        if (studentEventId == 111
                            && ((int)courseType >= Math.Ceiling(institutionSpeciality.Duration.Value) || (int)courseType < 2))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.TwoSemestersYearsForOneCantBeFirstOrLastSemester);
                        }

                        // Check period if it's ok
                        if (periodYearToCheck != periodYear || periodSemesterToCheck != periodSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeNextForActiveStatus);
                        }

                        // Check courseType and studentSemester if they are ok
                        if (studentEventId != 112)
                        {
                            // Check courseType and studentSemester if they are ok
                            if (courseTypeToCheck != courseType || studentSemesterToCheck != studentSemester)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeSameForActiveStatus);
                            }
                        }
                        else if (lastStudentSemester.StudentSemester == Semester.First
                            ? (courseType > lastStudentSemester.Course || (courseType == lastStudentSemester.Course && studentSemester == Semester.Second))
                            : courseType > lastStudentSemester.Course)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.SemesterRepetitionMustNotBeGreaterThanLastSemester);
                        }
                    }
                    else
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidActiveStudentEventAfterSemester);
                    }
                }
                #endregion
                #region StudentEvent - Interrupted
                else if (interruptedStudentEventHashSet.Contains(studentEventId))
                {
                    if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EducationalFeeMustBeEmpty);
                    }

                    if (lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.Active && studentEventId != 201)
                    {
                        // Check period if it's ok
                        if (lastStudentSemester.Period.Year != periodYear || lastStudentSemester.Period.Semester != periodSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeSameAsLastForInterruptedStatus);
                        }

                        // Check courseType and studentSemester if they are ok
                        if (lastStudentSemester.Course != courseType || lastStudentSemester.StudentSemester != studentSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeSameAsLastForInterruptedStatus);
                        }
                    }
                    else if (lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.Interrupted || studentEventId == 201)
                    {
                        var periodYearToCheck = lastStudentSemester.Period.Year;
                        var periodSemesterToCheck = lastStudentSemester.Period.Semester;
                        var courseTypeToCheck = lastStudentSemester.Course;
                        var studentSemesterToCheck = lastStudentSemester.StudentSemester;

                        if (periodSemesterToCheck == Semester.First)
                        {
                            periodSemesterToCheck = Semester.Second;
                        }
                        else
                        {
                            periodYearToCheck++;
                            periodSemesterToCheck = Semester.First;
                        }

                        // Check period if it's ok
                        if (periodYearToCheck != periodYear || periodSemesterToCheck != periodSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeNextForInterruptedAfterInterrupted);
                        }

                        // The only case when courseType and semester must be next is this
                        if (studentEventId == 201 && lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.Active)
                        {
                            if (studentSemesterToCheck == Semester.First)
                            {
                                studentSemesterToCheck = Semester.Second;
                            }
                            else
                            {
                                courseTypeToCheck++;
                                studentSemesterToCheck = Semester.First;
                            }

                            // Check courseType and studentSemester if they are ok
                            if (courseTypeToCheck != courseType || studentSemesterToCheck != studentSemester)
                            {
                                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeNextForInterruptedStatus);
                            }
                        }
                        // Check courseType and studentSemester if they are ok
                        else if (courseTypeToCheck != courseType || studentSemesterToCheck != studentSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeSameAsLastForInterruptedStatus);
                        }
                    }
                    else
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidInterruptedStudentEventAfterSemester);
                    }
                }
                #endregion
                #region StudentEvent - Completed
                else if (completedStudentEventHashSet.Contains(studentEventId))
                {
                    if (hasScholarship || useHostel || useHolidayBase || participatedIntPrograms)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.StatusCompletedNoScholarshipHosterlBaseProgrammes);
                    }

                    if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EducationalFeeMustBeEmpty);
                    }

                    if (lastStudentSemester.Period.Year != periodYear || lastStudentSemester.Period.Semester != periodSemester)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeSameAsLastForCompletedStatus);
                    }

                    if (lastStudentSemester.Course != courseType || lastStudentSemester.StudentSemester != studentSemester)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeSameAsLastForCompletedStatus);
                    }
                }
                #endregion
                #region StudentEvent - ProcessGraduation
                else if (processGraduationStudentEventHashSet.Contains(studentEventId))
                {
                    if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EducationalFeeMustBeEmpty);
                    }

                    if (lastStudentSemester.Course != courseType || lastStudentSemester.StudentSemester != studentSemester)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CourseSemesterMustBeSameAsLastForProcessGraduationStatus);
                    }

                    if (lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.Active && studentEventId == 401)
                    {
                        if (lastStudentSemester.Period.Year != periodYear || lastStudentSemester.Period.Semester != periodSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeSameAsLastForGraduatedCourseEvent);
                        }

                        var institutionSpecialityDuration = personStudent.InstitutionSpeciality.Duration.Value;

                        if (institutionSpecialityDuration % 1 == 0
                                && ((int)courseType != (int)institutionSpecialityDuration || studentSemester != Semester.Second))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.StudentNotLastSemesterForStatusCompleted);
                        }
                        else if (institutionSpecialityDuration % 1 != 0
                            && (Math.Ceiling(institutionSpecialityDuration) != (int)courseType || studentSemester != Semester.First))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.StudentNotLastSemesterForStatusCompleted);
                        }
                    }
                    else if (lastStudentSemester.StudentStatus?.Alias == StudentStatusConstants.ProcessGraduation && studentEventId != 401)
                    {
                        var periodYearToCheck = lastStudentSemester.Period.Year;
                        var periodSemesterToCheck = lastStudentSemester.Period.Semester;

                        if (periodSemesterToCheck == Semester.First)
                        {
                            periodSemesterToCheck = Semester.Second;
                        }
                        else
                        {
                            periodYearToCheck++;
                            periodSemesterToCheck = Semester.First;
                        }

                        if (periodYearToCheck != periodYear || periodSemesterToCheck != periodSemester)
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeriodMustBeNextForProcrastinationEvent);
                        }
                    }
                    else
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidProcessGraduationStudentEventAfterSemester);
                    }
                }
                #endregion
                else
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidStudentEvent);
                }
            }
        }

        public void InitialSpecialityValidation(PersonLot personLot, List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes, Dictionary<int, AdmissionReason> admissionReasonDict, HashSet<int> educationalFeeHashSet, HashSet<string> researchAreaCodeHashSet, string institutionSpecialituEdQualificationAlias, IEnumerable<PersonStudent> personCompletedRelocatedSpecialities)
        {
            var studentEventId = int.Parse(lineColumnDtos.Single(e => e.Index == 5).Value);
            var courseType = lineColumnDtos.Single(e => e.Index == 6).Value.GetEnumValueString<CourseType>();
            var studentSemester = lineColumnDtos.Single(e => e.Index == 7).Value.GetEnumValueString<Semester>();
            var educationalFeeValue = lineColumnDtos.Single(e => e.Index == 8).Value;
            var admissionReasonValue = lineColumnDtos.Single(e => e.Index == 9).Value;
            var facultyNumberValue = lineColumnDtos.Single(e => e.Index == 10).Value;
            var peTypeValue = lineColumnDtos.Single(e => e.Index == 11).Value;
            var peHighSchoolTypeValue = lineColumnDtos.Single(e => e.Index == 12).Value;
            var peResearchAreaValue = lineColumnDtos.Single(e => e.Index == 13).Value;
            var noteValue = lineColumnDtos.Single(e => e.Index == 14).Value;
            var relocatedFromPartValue = lineColumnDtos.Single(e => e.Index == 15).Value;
            var relocatedDocumentNumberValue = lineColumnDtos.Single(e => e.Index == 16).Value;
            var relocatedDocumentDateValue = lineColumnDtos.Single(e => e.Index == 17).Value;
            var currentYear = DateTime.Now.Year;

            // AdmissionReason and EducationalFee validation
            AdmissionReasonEducationalFeeValidation(admissionReasonValue, educationalFeeValue, txtValidationErrorCodes, admissionReasonDict, educationalFeeHashSet, personLot);

            // FacultyNumber
            if (!string.IsNullOrWhiteSpace(facultyNumberValue) && facultyNumberValue.Length > 50)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.FacultyNumberLength);
            }

            // Note
            if (!string.IsNullOrWhiteSpace(noteValue) && noteValue.Length > 500)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.NoteLength);
            }

            // PE validation
            PEValidation(peTypeValue, peHighSchoolTypeValue, peResearchAreaValue, txtValidationErrorCodes, researchAreaCodeHashSet, institutionSpecialituEdQualificationAlias);

            // StudentEvent validation
            if (!initialSpecialityStudentEventHashSet.Contains(studentEventId))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidInitialSpecialityStudentEvent);
            }
            else
            {
                if (studentEventId == 100 || studentEventId == 106 || studentEventId == 107)
                {
                    if (studentEventId == 100 && (courseType != CourseType.First || studentSemester != Semester.First))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidInitialSpecialityCourseSemester);
                    }

                    RelocationMustBeEmpty(relocatedFromPartValue, relocatedDocumentNumberValue, relocatedDocumentDateValue, txtValidationErrorCodes);
                }
                else if (studentEventId == 104 || studentEventId == 105)
                {
                    RelocationValidation(relocatedFromPartValue, relocatedDocumentNumberValue, relocatedDocumentDateValue, studentEventId, currentYear, personCompletedRelocatedSpecialities.Select(e => e.InstitutionSpecialityId), txtValidationErrorCodes);
                }
            }
        }

        public void AddDoctoralSemesterValidation(PersonDoctoral personDoctoral, List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes, HashSet<int> educationalFeeHashSet)
        {
            var protocolDate = lineColumnDtos.Single(e => e.Index == 4).Value.GetDateTimeString().Value;
            var studentEventId = int.Parse(lineColumnDtos.Single(e => e.Index == 5).Value);
            var doctoralYear = lineColumnDtos.Single(e => e.Index == 6).Value.GetEnumValueString<YearType>();
            var atestationValue = lineColumnDtos.Single(e => e.Index == 7).Value;
            var educationalFeeValue = lineColumnDtos.Single(e => e.Index == 8).Value;
            var noteValue = lineColumnDtos.Single(e => e.Index == 14).Value;

            var admissionReasonValue = lineColumnDtos.Single(e => e.Index == 9).Value;
            var startDateValue = lineColumnDtos.Single(e => e.Index == 10).Value;
            var endDateValue = lineColumnDtos.Single(e => e.Index == 11).Value;
            var peHighSchoolTypeValue = lineColumnDtos.Single(e => e.Index == 12).Value;
            var peResearchAreaValue = lineColumnDtos.Single(e => e.Index == 13).Value;
            var relocatedFromPartValue = lineColumnDtos.Single(e => e.Index == 15).Value;
            var relocatedDocumentNumberValue = lineColumnDtos.Single(e => e.Index == 16).Value;
            var relocatedDocumentDateValue = lineColumnDtos.Single(e => e.Index == 17).Value;

            var hasScholarship = lineColumnDtos.Single(e => e.Index == 18).Value.GetBooleanFromString();
            var useHostel = lineColumnDtos.Single(e => e.Index == 19).Value.GetBooleanFromString();
            var useHolidayBase = lineColumnDtos.Single(e => e.Index == 20).Value.GetBooleanFromString();
            var participatedIntPrograms = lineColumnDtos.Single(e => e.Index == 21).Value.GetBooleanFromString();

            var lastDoctoralSemester = personDoctoral.Semesters
                    .OrderByDescending(e => e.ProtocolDate.Date)
                    .ThenByDescending(e => e.Id)
                    .First();

            #region Must be empty values

            if (!string.IsNullOrWhiteSpace(admissionReasonValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.AdmissionReasonMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(startDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralStartDateMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(endDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEndDateMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(peHighSchoolTypeValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeHighSchoolMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(peResearchAreaValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeResearchAreaMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedFromPartValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedFromMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedDocumentNumberValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentNumberMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedDocumentDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentDateMustBeEmpty);
            }
            #endregion

            // Last doctoral semester check
            if (lastDoctoralSemester.ProtocolDate.Date > protocolDate.Date)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralProtocolDateMustBeGreaterThanLast);
            }

            if (doctoralYear < lastDoctoralSemester.YearType)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralYearMustBeGreaterThanLast);
            }

            // Note
            if (!string.IsNullOrWhiteSpace(noteValue) && noteValue.Length > 500)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.NoteLength);
            }

            if (lastDoctoralSemester.StudentStatus.Alias == StudentStatusConstants.Graduated)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CannotAddSemesterToGraduatedDoctoral);
            }
            else if (lastDoctoralSemester.StudentStatus.Alias == StudentStatusConstants.Completed)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.CannotAddSemesterToCompletedDoctoral);
            }
            else
            {
                #region StudentEvent - Active
                if (activeDoctoralStudentEventHashSet.Contains(studentEventId))
                {
                    if (lastDoctoralSemester.StudentStatus?.Alias == StudentStatusConstants.Active && activeDoctoralAfterActiveStudentEventHashSet.Contains(studentEventId))
                    {
                        if (studentEventId != 114 && !string.IsNullOrWhiteSpace(educationalFeeValue))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEducationalFeeMustBeEmpty);
                        }

                        if (studentEventId == 114
                            && (string.IsNullOrWhiteSpace(educationalFeeValue)
                                || !ValidatePropertiesStatic.NumbersOnly(educationalFeeValue)
                                || !educationalFeeHashSet.Contains(int.Parse(educationalFeeValue))))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralChangeFeeMustHaveNomValue);
                        }

                        if (studentEventId == 102
                            && (string.IsNullOrWhiteSpace(atestationValue)
                                || !atestationValue.TryParseEnumValue<AttestationType>()))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralAtestationRequired);
                        }
                    }
                    else if (lastDoctoralSemester.StudentStatus?.Alias == StudentStatusConstants.Interrupted && activeDoctoralAfterInterruptedStudentEventHashSet.Contains(studentEventId))
                    {
                        if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEducationalFeeMustBeEmpty);
                        }
                    }
                    else
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidActiveStudentEventAfterSemester);
                    }
                }
                #endregion
                #region StudentEvent - Interrupted
                else if (interruptedDoctoralStudentEventHashSet.Contains(studentEventId))
                {
                    if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEducationalFeeMustBeEmpty);
                    }

                    if (lastDoctoralSemester.StudentStatus.Alias != StudentStatusConstants.Active
                        && lastDoctoralSemester.StudentStatus.Alias != StudentStatusConstants.Interrupted)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidInterruptedStudentEventAfterSemester);
                    }
                }
                #endregion
                #region StudentEvent - Completed
                else if (completedDoctoralStudentEventHashSet.Contains(studentEventId))
                {
                    if (hasScholarship || useHostel || useHolidayBase || participatedIntPrograms)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralStatusCompletedNoScholarshipHosterlBaseProgrammes);
                    }

                    if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEducationalFeeMustBeEmpty);
                    }

                    if (!string.IsNullOrWhiteSpace(atestationValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.AtestationMustBeEmpty);
                    }
                }
                #endregion
                #region StudentEvent - ProcessGraduation
                else if (processGraduationDoctoralStudentEventHashSet.Contains(studentEventId))
                {
                    if (!string.IsNullOrWhiteSpace(educationalFeeValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEducationalFeeMustBeEmpty);
                    }

                    if (!string.IsNullOrWhiteSpace(atestationValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.AtestationMustBeEmpty);
                    }

                    if (lastDoctoralSemester.YearType != doctoralYear)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralYearMustBeSameAsLast);
                    }

                    if ((lastDoctoralSemester.StudentStatus?.Alias != StudentStatusConstants.Active && lastDoctoralSemester.StudentStatus?.Alias != StudentStatusConstants.ProcessGraduation)
                        || (lastDoctoralSemester.StudentStatus?.Alias == StudentStatusConstants.Active && studentEventId != 402)
                        || (lastDoctoralSemester.StudentStatus?.Alias == StudentStatusConstants.ProcessGraduation && studentEventId == 402))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidProcessGraduationStudentEventAfterSemester);
                    }
                }
                #endregion
                else
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidDoctoralStudentEvent);
                }
            }
        }

        public void InitialDoctoralValidation(PersonLot personLot, List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes, Dictionary<int, AdmissionReason> admissionReasonDict, HashSet<int> educationalFeeHashSet, HashSet<string> researchAreaCodeHashSet, IEnumerable<PersonDoctoral> personCompletedRelocatedDoctorals)
        {
            var studentEventId = int.Parse(lineColumnDtos.Single(e => e.Index == 5).Value);
            var doctoralYearType = lineColumnDtos.Single(e => e.Index == 6).Value.GetEnumValueString<YearType>();
            var atestation = lineColumnDtos.Single(e => e.Index == 7).Value;
            var educationalFeeValue = lineColumnDtos.Single(e => e.Index == 8).Value;
            var admissionReasonValue = lineColumnDtos.Single(e => e.Index == 9).Value;
            var startDateValue = lineColumnDtos.Single(e => e.Index == 10).Value;
            var endDateValue = lineColumnDtos.Single(e => e.Index == 11).Value;
            var peHighSchoolTypeValue = lineColumnDtos.Single(e => e.Index == 12).Value;
            var peResearchAreaValue = lineColumnDtos.Single(e => e.Index == 13).Value;
            var noteValue = lineColumnDtos.Single(e => e.Index == 14).Value;
            var relocatedFromPartValue = lineColumnDtos.Single(e => e.Index == 15).Value;
            var relocatedDocumentNumberValue = lineColumnDtos.Single(e => e.Index == 16).Value;
            var relocatedDocumentDateValue = lineColumnDtos.Single(e => e.Index == 17).Value;
            var currentYear = DateTime.Now.Year;

            // AdmissionReason and EducationalFee validation
            AdmissionReasonEducationalFeeValidation(admissionReasonValue, educationalFeeValue, txtValidationErrorCodes, admissionReasonDict, educationalFeeHashSet, personLot);

            // Note
            if (!string.IsNullOrWhiteSpace(noteValue) && noteValue.Length > 500)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.NoteLength);
            }

            // StartDate and EndDate
            if (string.IsNullOrWhiteSpace(startDateValue)
                || !ValidatePropertiesStatic.IsValidDateBetween(startDateValue, 1970, currentYear))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralStartDateRequired);
            }
            else if (!string.IsNullOrWhiteSpace(endDateValue))
            {
                if (!ValidatePropertiesStatic.IsValidDateBetween(endDateValue, 1970, currentYear + 5))
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidDoctoralEndDate);
                }
                else if (startDateValue.GetDateTimeString().Value.Date > endDateValue.GetDateTimeString().Value.Date)
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralEndDateMustBeAfterStartDate);
                }
            }

            // Atestation must be null
            if (!string.IsNullOrWhiteSpace(atestation))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.NoAtestationOnInitialDoctoral);
            }

            // PE validation
            PEValidation("2", peHighSchoolTypeValue, peResearchAreaValue, txtValidationErrorCodes, researchAreaCodeHashSet, "doctor");

            // StudentEvent validation
            if (!initialDoctoralStudentEventHashSet.Contains(studentEventId))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidInitialDoctoralStudentEvent);
            }
            else
            {
                if (studentEventId == 100 || studentEventId == 106)
                {
                    if (studentEventId == 100 && doctoralYearType != YearType.FirstYear)
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidInitialDoctoralYear);
                    }

                    RelocationMustBeEmpty(relocatedFromPartValue, relocatedDocumentNumberValue, relocatedDocumentDateValue, txtValidationErrorCodes);
                }
                else if (studentEventId == 104 || studentEventId == 105)
                {
                    RelocationValidation(relocatedFromPartValue, relocatedDocumentNumberValue, relocatedDocumentDateValue, studentEventId, currentYear, personCompletedRelocatedDoctorals.Select(e => e.InstitutionSpecialityId), txtValidationErrorCodes);
                }
            }
        }

        private void AdmissionReasonEducationalFeeValidation(string admissionReasonValue, string educationalFeeValue, List<TxtSpecValidationErrorCode> txtValidationErrorCodes, Dictionary<int, AdmissionReason> admissionReasonDict, HashSet<int> educationalFeeHashSet, PersonLot personLot)
        {
            if (string.IsNullOrWhiteSpace(admissionReasonValue)
                || string.IsNullOrWhiteSpace(educationalFeeValue)
                || !ValidatePropertiesStatic.NumbersOnly(admissionReasonValue)
                || !ValidatePropertiesStatic.NumbersOnly(educationalFeeValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidAdmissionReasonOrEducationFeeValue);
            }
            else
            {
                var educationalFeeId = int.Parse(educationalFeeValue);
                var hasEducationalFeeInNom = educationalFeeHashSet.Contains(educationalFeeId);
                var admissionReason = admissionReasonDict.GetDictValueOrNull(int.Parse(admissionReasonValue));

                if (!hasEducationalFeeInNom)
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.MissingEducationFee);
                }

                if (admissionReason == null)
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.MissingAdmissionReason);
                }

                if (hasEducationalFeeInNom && admissionReason != null)
                {
                    // Check if EducationalFee is in Admission reason
                    if (!admissionReason.AdmissionReasonEducationFees.Select(e => e.EducationFeeTypeId).Contains(educationalFeeId))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EducationalFeeMissingInAdmissionReason);
                    }

                    // Check if Person citizenship is acceptable for this admission reason
                    var includeCountriesIds = admissionReason.AdmissionReasonCitizenships.Where(e => e.ExcludeCountry == false).Select(e => e.CountryId).ToList();
                    var excludeCountriesIds = admissionReason.AdmissionReasonCitizenships.Where(e => e.ExcludeCountry == true).Select(e => e.CountryId).ToList();
                    var countryUnion = admissionReason.CountryUnion;

                    var personCountryIsIncluded = includeCountriesIds.Any()
                        && (includeCountriesIds.Contains(personLot.PersonBasic.CitizenshipId)
                            || (personLot.PersonBasic.SecondCitizenshipId.HasValue && includeCountriesIds.Contains(personLot.PersonBasic.SecondCitizenshipId.Value)));

                    if (excludeCountriesIds.Any()
                        && (excludeCountriesIds.Contains(personLot.PersonBasic.CitizenshipId)
                            || (personLot.PersonBasic.SecondCitizenshipId.HasValue && excludeCountriesIds.Contains(personLot.PersonBasic.SecondCitizenshipId.Value))))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidAdmissionReasonForCitizenship);
                    }
                    else if (countryUnion.HasValue)
                    {
                        if (countryUnion.Value == CountryUnion.EuAndEea
                            && !personLot.PersonBasic.Citizenship.EeaCountry
                            && !personLot.PersonBasic.Citizenship.EuCountry
                            && !personCountryIsIncluded
                            && (personLot.PersonBasic.SecondCitizenship == null
                                || (personLot.PersonBasic.SecondCitizenship?.EuCountry == false && personLot.PersonBasic.SecondCitizenship?.EeaCountry == false)))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidAdmissionReasonForCitizenship);
                        }

                        if (countryUnion.Value == CountryUnion.OtherCountries
                            && (personLot.PersonBasic.Citizenship.EeaCountry
                                || personLot.PersonBasic.Citizenship.EuCountry
                                || !personCountryIsIncluded
                                || (personLot.PersonBasic.SecondCitizenship != null
                                    && (personLot.PersonBasic.SecondCitizenship?.EuCountry == true || personLot.PersonBasic.SecondCitizenship?.EeaCountry == true))))
                        {
                            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidAdmissionReasonForCitizenship);
                        }
                    }
                }
            }
        }

        public void PEValidation(string peTypeValue, string peHighSchoolTypeValue, string peResearchAreaValue, List<TxtSpecValidationErrorCode> txtValidationErrorCodes, HashSet<string> researchAreaCodeHashSet, string institutionSpecialituEdQualificationAlias)
        {
            if (string.IsNullOrWhiteSpace(peTypeValue)
                || !peTypeValue.TryParseEnumValue<PreviousEducationType>())
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidPeType);
            }
            else
            {
                var peType = peTypeValue.GetEnumValueString<PreviousEducationType>();

                if (peType == PreviousEducationType.Missing)
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidPeType);
                }
                else if (peType == PreviousEducationType.HighSchool)
                {
                    if (string.IsNullOrWhiteSpace(peResearchAreaValue)
                        || peResearchAreaValue.Length != 4
                        || !researchAreaCodeHashSet.Contains(peResearchAreaValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeReserchAreaRequired);
                    }

                    if (string.IsNullOrWhiteSpace(peHighSchoolTypeValue)
                        || !peHighSchoolTypeValue.TryParseEnumValue<PreviousHighSchoolEducationTypeTxt>())
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.InvalidPeHighSchoolType);
                    }
                }
                else if (peType == PreviousEducationType.Secondary)
                {
                    if (!string.IsNullOrWhiteSpace(peResearchAreaValue))
                    {
                        txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.SecondaryHasNoResearchArea);
                    }
                }

                if (institutionSpecialituEdQualificationAlias == EducationalQualificationConstants.MasterHigh && peType != PreviousEducationType.HighSchool)
                {
                    txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.PeTypeMustBeHighForMasters);
                }
            }
        }

        public void RelocationMustBeEmpty(string relocatedFromPartValue, string relocatedDocumentNumberValue, string relocatedDocumentDateValue, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            if (!string.IsNullOrWhiteSpace(relocatedFromPartValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedFromMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedDocumentNumberValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentNumberMustBeEmpty);
            }

            if (!string.IsNullOrWhiteSpace(relocatedDocumentDateValue))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentDateMustBeEmpty);
            }
        }

        public void RelocationValidation(string relocatedFromPartValue, string relocatedDocumentNumberValue, string relocatedDocumentDateValue, int studentEventId, int currentYear, IEnumerable<int> personCompletedRelocatedSpecialityIds, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            if (string.IsNullOrWhiteSpace(relocatedDocumentNumberValue) || relocatedDocumentNumberValue.Length > 30)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentNumberRequired);
            }

            if (string.IsNullOrWhiteSpace(relocatedDocumentDateValue)
                || !ValidatePropertiesStatic.IsValidDateBetween(relocatedDocumentDateValue, currentYear - 70, currentYear))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedDocumentDateRequired);
            }

            if (studentEventId == 104
                && (string.IsNullOrWhiteSpace(relocatedFromPartValue)
                    || !ValidatePropertiesStatic.NumbersOnly(relocatedFromPartValue)
                    || !personCompletedRelocatedSpecialityIds.Contains(int.Parse(relocatedFromPartValue))))
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.RelocatedFromPartRequired);
            }
        }
    }
}
