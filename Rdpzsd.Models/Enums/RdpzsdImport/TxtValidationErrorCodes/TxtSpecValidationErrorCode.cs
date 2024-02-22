using System.ComponentModel;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    [Description("Код на грешката за семестриална информация")]
    public enum TxtSpecValidationErrorCode
    {
        [Description("Функционалността за редакция не е налична")]
        EditNotImplemented = 1,

        [Description("Грешен брой колони")]
        WrongColumnCount = 2,

        [Description("Не е намерено лице с посочения ЕАН и дата на раждане")]
        MissingPersonUan = 3,

        [Description("Номерът на специалността трябва да е цяло число")]
        InvalidSpecialityNumberType = 4,

        [Description("Не е намерена специалност/докторска програма с този номер или нямате права върху нея")]
        WrongSpecialityNumberOrNoPermissions = 5,

        [Description("Във файла се съдържа вече информация за лицето към посочената специалност")]
        DuplicateUanInstitutionSpeciality = 6,

        [Description("Невалидно действие (колона 18)")]
        InvalidImportSpecialityAction = 7,

        [Description("Невалиден/неактивен учебен период")]
        InvalidPeriod = 8,

        [Description("Невалидна стойност в полето курс на студента")]
        InvalidCourseType = 9,

        [Description("Невалидна стойност в полето семестър на студента")]
        InvalidStudentSemester = 10,

        [Description("Курсът на студента е по-голям от продължителността на обучение в специалността")]
        CourseGreaterThanSpecialityDuration = 11,

        [Description("Липсва продължителност на обучение за специалността/докторската програма")]
        InstitutionSpecialityHasNoDuration = 12,

        [Description("Невалидна стойност на семестриално събитие")]
        InvalidStudentEvent = 13,

        [Description("Невалидна стойност на семестриално събитие за първоначално записване на студент в специалност")]
        InvalidInitialSpecialityStudentEvent = 14,

        [Description("За първоначално записване на студент в специалност, трябва да е въведен първи курс и първи семестър")]
        InvalidInitialSpecialityCourseSemester = 15,

        [Description("Основанието за прием и вида на таксата на обучение трябва да са валидни номера от номенклатурите")]
        InvalidAdmissionReasonOrEducationFeeValue = 16,

        [Description("Липсва вид на такса на обучение с посочения номер в номенклатурата")]
        MissingEducationFee = 17,

        [Description("Липсва основание за прием с посочения номер в номенклатурата")]
        MissingAdmissionReason = 18,

        [Description("Посоченото основание за прием е невалидно за конкретното лице, спрямо неговите гражданства")]
        InvalidAdmissionReasonForCitizenship = 19,

        [Description("Вида на таксата на обучение не е валидна спрямо избраното основание за прием")]
        EducationalFeeMissingInAdmissionReason = 20,

        [Description("Вида на предходното образование е задължителен")]
        InvalidPeType = 21,

        [Description("Посочете предходно образование в България или чужбина")]
        InvalidPeHighSchoolType = 22,

        [Description("Предходно образование средно не трябва да има професионално направление")]
        SecondaryHasNoResearchArea = 23,

        [Description("Професионално направление към предходно образование е задължително")]
        PeReserchAreaRequired = 24,

        [Description("За магистри след висше видът на предходното образование трябва да е висше")]
        PeTypeMustBeHighForMasters = 25,

        [Description("Факултетния номер не трябва да е повече от 50 символа")]
        FacultyNumberLength = 26,

        [Description("Бележката не трябва да е повече от 500 символа")]
        NoteLength = 27,

        [Description("'Преместен от' не трябва да съдържа стойност")]
        RelocatedFromMustBeEmpty = 28,

        [Description("Номерът на документа за признаване не трябва да съдържа стойност")]
        RelocatedDocumentNumberMustBeEmpty = 29,

        [Description("Датата на документа за признаване не трябва да съдържа стойност")]
        RelocatedDocumentDateMustBeEmpty = 30,

        [Description("Номерът на документа за признаване е задължителен и трябва да е не повече от 30 символа")]
        RelocatedDocumentNumberRequired = 31,

        [Description("Датата на документа за признаване е задължителен и трябва да не е след текущата година или преди повече от 70 години")]
        RelocatedDocumentDateRequired = 32,

        [Description("Не е намерена специалност в регистъра, за която студентът е приключил обучението си и е преместен от нея")]
        RelocatedFromPartRequired = 33,

        [Description("Основанието за прием не трябва да съдържа стойност")]
        AdmissionReasonMustBeEmpty = 34,

        [Description("Факултетния номер не трябва да съдържа стойност")]
        FacultyNumberMustBeEmpty = 35,

        [Description("Видът на предходното образование не трябва да съдържа стойност")]
        PeTypeMustBeEmpty = 36,

        [Description("Видът на предходното образование (в България/чужбина) не трябва да съдържа стойност")]
        PeHighSchoolMustBeEmpty = 37,

        [Description("Професионалното направление на предходното образование не трябва да съдържа стойност")]
        PeResearchAreaMustBeEmpty = 38,

        [Description("Не може да се добавя семестър за студент в статус 'Дипломиран'")]
        CannotAddSemesterToGraduated = 39,

        [Description("Не може да се добавя семестър за студент в статус 'Приключил'")]
        CannotAddSemesterToCompleted = 40,

        [Description("Вида на такса на обучение не трябва да съдържа стойност, когато статуса на студента е 'Прекъснал', 'Приключил' или 'В процес на дипломиране'")]
        EducationalFeeMustBeEmpty = 41,

        [Description("Приключването на обучението на студента към специалността става в същия учебен период. Учебната година и учебния семестър трябва да са същите, като на последно въведената семестриална информация")]
        PeriodMustBeSameAsLastForCompletedStatus = 42,

        [Description("Приключването на обучението на студента към специалността става в същия курс и семестър. Курсът и семестъра на студента трябва да са същите, като на последно въведената семестриална информация")]
        CourseSemesterMustBeSameAsLastForCompletedStatus = 43,

        [Description("За да запишете студента, като 'семестриално завършил' той трябва да е достигнал последния семестър")]
        StudentNotLastSemesterForStatusCompleted = 44,

        [Description("Полето отговарящо на стипендията трябва да е от булев тип (0/1)")]
        InvalidHasScholarshipBoolean = 45,

        [Description("Полето отговарящо на общежитие трябва да е от булев тип (0/1)")]
        InvalidUseHostelBoolean = 46,

        [Description("Полето отговарящо на почивни бази трябва да е от булев тип (0/1)")]
        InvalidUseHolidayBaseBoolean = 47,

        [Description("Полето отговарящо на участвал в международни програми трябва да е от булев тип (0/1)")]
        InvalidParticipatedIntProgramsBoolean = 48,

        //[Description("При първоначално записване в специалност, студента не може да взима стипендия")]
        //InitialSpecialityHasNoScholarship = 49,

        [Description("Студент, който приключва обучението си в специалността не може да взима стипендия, да е на общежитие, да използва почивни бази или да участва в международни програми")]
        StatusCompletedNoScholarshipHosterlBaseProgrammes = 50,

        [Description("Прекъснал студент, не може да взима стипендия")]
        StatusInterruptedNoScholarship = 51,

        [Description("Прекъсването на обучението на студента към специалността става в същия учебен период. Учебната година и учебния семестър трябва да са същите, като на последно въведената семестриална информация")]
        PeriodMustBeSameAsLastForInterruptedStatus = 52,

        [Description("Продължението на прекъсване на студент или прекъсването му заради неплатена такса, става в следващия учебен период. Учебната година и учебния семестър трябва да са следващите, спрямо последно въведената семестриална информация")]
        PeriodMustBeNextForInterruptedAfterInterrupted = 53,

        [Description("Прекъсването на обучението на студента към специалността става в същия курс и семестър. Курсът и семестъра на студента трябва да са същите, като на последно въведената семестриална информация")]
        CourseSemesterMustBeSameAsLastForInterruptedStatus = 54,

        [Description("Прекъсването на обучението на студент към специалността, поради неплатена такса става в следващия курс и семестър. Курсът и семестъра на студента трява да са следващите, спрямо последно въведената семестриална информация")]
        CourseSemesterMustBeNextForInterruptedStatus = 55,

        [Description("Вида на таксата на обучение трябва да съдържа стойност")]
        EducationalFeeMustHaveValue = 56,

        [Description("Невалидна стойност на семестриално събитие към статус действащ, спрямо предходния семестър")]
        InvalidActiveStudentEventAfterSemester = 57,

        [Description("Предходният семестър е две години за една. Текущия също трябва да е две години за една")]
        LastSemesterIsIndividualPlanTwoYears = 58,

        [Description("Регулирана специалност няма възможност за две години/семестъра за един")]
        RegulatedSpecialityHasNoTwoSemestersForOne = 59,

        [Description("Две години/семестъра за един не са позволени за първи и последен курс")]
        TwoSemestersYearsForOneCantBeFirstOrLastSemester = 60,

        [Description("Студент достигнал последния си семестър, но има невзети изпити трябва да е със събитие 'Завършил учебния план с невзети изпити'")]
        StudentInLastSemesterMustBe113IfActive = 61,

        [Description("За студент, който не е достигнал последния семестър не е позволено подаването на събитие 'Завършил учебния план с невзети изпити'")]
        StudentNotInLastSemesterMustBeDifferent113IfActive = 62,

        [Description("За студенти продължаващи обучението си към специалността става в следващия учебен период. Учебната година и учебния семестър трябва да са следващите, спрямо последно въведената семестриална информация")]
        PeriodMustBeNextForActiveStatus = 63,

        [Description("За студенти завършили учебния план без невзети изпити, курсът и семестърът му трябва да са същите като предходния")]
        CourseSemesterMustBeSameForActiveStatus113 = 64,

        [Description("За студенти продължаващи обучението си към специалността става в следващия курс и семестър. Курсът и семестъра трябва да са следващите, спрямо последно въведената семестриална информация")]
        CourseSemesterMustBeNextForActiveStatus = 65,

        [Description("За студенти продължаващи обучението си към специалността след прекъсване става в същия курс и семестър. Курсът и семестъра трябва да са същите, като последно въведената семестриална информация")]
        CourseSemesterMustBeSameForActiveStatus = 66,

        [Description("Семестърът който студента повтаря не трябва да е по-голям от последния активен")]
        SemesterRepetitionMustNotBeGreaterThanLastSemester = 67,

        [Description("ЕАН и дата на раждане са задължителни полета")]
        UanAndBirthDateRequired = 68,

        [Description("Невалиден формат на дата на раждане")]
        InvalidBirthDateFormat = 69,

        [Description("За студента/докторанта не е намерена посочената специалност/докторска програма, която да бъде изтрита")]
        InstitutionSpecialityNotFoundForErase = 70,

        [Description("Семестъра към специалността, който се опитвате да изтриете не е намерен или не е последения въведен")]
        SemesterForEraseIsNotLastOrNotFound = 71,

        [Description("Номерът на протокол/заповед е задължително поле и не трябва да надвишава 15 символа")]
        InvalidProtocolNumber = 72,

        [Description("Датата на протокол/заповед е задължително поле и трябва да е между 1970 и текущата година")]
        InvalidProtocolDate = 73,

        [Description("Годината на обучение за докторантите е задължителна и стойността трябва да е между 1 и 5")]
        InvalidDoctoralYear = 74,

        [Description("Годината на обучение за докторантите не може да е по-голяма от допустимата за докторската програма")]
        DoctoralYearGreaterThanProgrammeDuration = 75,

        [Description("Атестацията трябва да е празна, за нова докторска програма")]
        NoAtestationOnInitialDoctoral = 76,

        [Description("Датата на зачисляване на докторант е задължителна и трябва да е между 1970 и текущата година")]
        DoctoralStartDateRequired = 77,

        [Description("Датата на отчисляване на докторант трябва да е между 1970 и текущата година + 5 години")]
        InvalidDoctoralEndDate = 78,

        [Description("Датата на отчисляване на докторант трябва да е след датата на зачисляване")]
        DoctoralEndDateMustBeAfterStartDate = 79,

        [Description("Невалидна стойност на семестриално събитие за първоначално записване на докторант в докторска програма")]
        InvalidInitialDoctoralStudentEvent = 80,

        [Description("За първоначално записване на докторант, трябва да е въведена първа година на обучение")]
        InvalidInitialDoctoralYear = 81,

        [Description("Датата на зачисляване не трябва да съдържа стойност")]
        DoctoralStartDateMustBeEmpty = 82,

        [Description("Очакваната дата на отчисляване не трябва да съдържа стойност")]
        DoctoralEndDateMustBeEmpty = 83,

        [Description("Не може да се добавя информация за обучението на докторант в статус 'Дипломиран'")]
        CannotAddSemesterToGraduatedDoctoral = 84,

        [Description("Не може да се добавя информация за обучението на докторант в статус 'Приключил'")]
        CannotAddSemesterToCompletedDoctoral = 85,

        [Description("Невалидна стойност на събитие")]
        InvalidDoctoralStudentEvent = 86,

        [Description("Докторант, който приключва обучението си в докторската програма не може да взима стипендия, да е на общежитие, да използва почивни бази или да участва в международни програми")]
        DoctoralStatusCompletedNoScholarshipHosterlBaseProgrammes = 87,

        [Description("Вида на такса на обучение не трябва да съдържа стойност, когато статуса на докторанта е 'Прекъснал', 'Приключил' или 'В процес на дипломиране'")]
        DoctoralEducationalFeeMustBeEmpty = 88,

        [Description("Събитие, което не е 'Атестация' не трябва да има оценка")]
        AtestationMustBeEmpty = 89,

        [Description("Датата на протокола/заповедта трябва да е след последно въведената")]
        DoctoralProtocolDateMustBeGreaterThanLast = 90,

        [Description("Годината на обучение на докторанта, не трябва да е преди последно въведената")]
        DoctoralYearMustBeGreaterThanLast = 91,

        [Description("Задължително е въвеждането на валидна такса на обучение на докторанта, когато събитието е 'Промяна на вида на таксата'")]
        DoctoralChangeFeeMustHaveNomValue = 92,

        [Description("Задължително е въвеждането на оценка на атестацията за докторанта")]
        DoctoralAtestationRequired = 93,

        [Description("Информацията за обучение към докторската програма, който се опитвате да изтриете не е намерен или не е последения въведен")]
        DoctoralSemesterForEraseIsNotLastOrNotFound = 94,

        [Description("Процеса по дипломиране на студента към специалността става в същия курс и семестър. Курсът и семестъра на студента трябва да са същите, като на последно въведената семестриална информация")]
        CourseSemesterMustBeSameAsLastForProcessGraduationStatus = 95,

        [Description("Невалидна стойност на семестриално събитие към статус прекъснал, спрямо предходния семестър")]
        InvalidInterruptedStudentEventAfterSemester = 96,

        [Description("Невалидна стойност на семестриално събитие към статус в процес на дипломиране, спрямо предходния семестър")]
        InvalidProcessGraduationStudentEventAfterSemester = 97,

        [Description("Семестриалното завършване на обучението на студента към специалността става в същия учебен период. Учебната година и учебния семестър трябва да са същите, като на последно въведената семестриална информация")]
        PeriodMustBeSameAsLastForGraduatedCourseEvent = 98,

        [Description("Отлагането на дипломиране на студента към специалността става в следващия учебен период. Учебната година и учебния семестър трябва да са следващите, спрямо последно въведената семестриална информация")]
        PeriodMustBeNextForProcrastinationEvent = 99,

        [Description("Годината на обучение на докторанта, трябва да е същата като последно въведената")]
        DoctoralYearMustBeSameAsLast = 100
    }
}
