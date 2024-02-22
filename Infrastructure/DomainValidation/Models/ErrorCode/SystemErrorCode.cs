namespace Infrastructure.DomainValidation.Models.ErrorCode
{
    public enum SystemErrorCode
    {
        // System
        System_NotEnoughPermissions,
        System_UnableToGetRndOrganizationPermissions,
        System_UnableToGetEmsUsers,
        System_UnableToConnectToRdDocuments,
        System_FunctionalityNotSupported,

        // Lot
        Lot_WrongLotState,

        // PersonLot
        PersonLot_CantEraseStudentDoctoral,

        // Part
        Part_WrongPartState,

        //Nomenclature
        Nomenclature_CannotBeDeletedBecauseInUse,
        AdmissionReason_NameCyrilic,
        AdmissionReason_NameAltLatin,
        AdmissionReason_ShortNameCyrilic,
        AdmissionReason_ShortNameAltLatin,
        AdmissionReason_EducationFeeTypeMustBeUnique,
        AdmissionReason_EducationFeeTypeRequired,
        AdmissionReason_StudentTypeRequired,
        Period_NotValidPeriod,
        Period_Exists,
        Period_NextPeriodNotStarted,
        School_SchoolByMigrationIdNotFound,
        School_NameCyrilic,
        School_SchoolStateRequired,
        School_SchoolParentRequired,

        // PersonBasic
        PersonBasic_FirstLastNameRequiredCyrillic,
        PersonBasic_MiddleNameCyrillic,
        PersonBasic_OtherNamesCyrillic,
        PersonBasic_FirstLastNameAltRequiredLatin,
        PersonBasic_MiddleNameAlt,
        PersonBasic_OtherNamesAltLatin,
        PersonBasic_BirthCountryRequired,
        PersonBasic_BirthSettlementRequired,
        PersonBasic_SecondCitizenshipDifferentThanMain,
        PersonBasic_ResidenceCountryRequired,
        PersonBasic_ResidenceSettlementRequired,
        PersonBasic_UinFnIdnRequired,
        PersonBasic_UinFnLength,
        PersonBasic_UinExists,
        PersonBasic_UinInvalid,
        PersonBasic_ForeignerNumberExists,
        PersonBasic_ForeignerNumberInvalid,
        PersonBasic_InvalidEmail,
        PersonBasic_EmailNotUnique,
        PersonBasic_InvalidPhoneNumber,
        PersonBasic_NoIdn,
        PersonBasic_CitizenshipsMustBeDifferent,

        // PersonPreviousEducation
        PersonPreviousEducation_DiplomaNumberMissing,
        PersonPreviousEducation_InvalidDiplomaDate,
        PersonPreviousEducation_MissingPreviousEducationType,
        PersonPreviousEducation_NoPreviousEducation,
        PersonPreviousEducation_NoInstitution,
        PersonPreviousEducation_NoSpeciality,
        PersonPreviousEducation_NoRecognizedSpeciality,
        PersonPreviousEducation_NoAcquiredSpeciality,
        PersonPreviousEducation_NoCountryAbroad,
        PersonPreviousEducation_InvalidResearchArea,
        PersonPreviousEducation_InvalidEducationalQualification,
        PersonPreviousEducation_InvalidAcquiredEducationalQualification,
        PersonPreviousEducation_InvalidRecognitionNumber,
        PersonPreviousEducation_InvalidRecognitionDate,
        PersonPreviousEducation_InvalidRecognitionDocument,

        // PersonSecondary
        PersonSecondary_NoInformation,
        PersonSecondary_InvalidGraduationYear,
        PersonSecondary_InvalidSchool,
        PersonSecondary_NotFoundSecondaryByUin,
        PersonSecondary_InvalidRecognitionNumber,
        PersonSecondary_InvalidRecognitionDate,
        PersonSecondary_InvalidRecognitionDocument,

        // PersonStudentDoctoral
        PersonStudentDoctoral_NotEnoughPermissions,
        PersonStudentDoctoral_NoPermissionsForBachelor,
        PersonStudentDoctoral_NoPermissionsForMaster,
        PersonStudentDoctoral_NoPermissionsForDoctoral,
        PersonStudentDoctoral_OneSemesterRequired,
        PersonStudentDoctoral_CannotEditGraduated,
        PersonStudentDoctoral_InvalidPeriod,
        PersonStudentDoctoral_InvalidProtocolDateNumber,
        PersonStudentDoctoral_InvalidYearType,
        PersonStudentDoctoral_InvalidStudentStatus,
        PersonStudentDoctoral_InvalidStudentEvent,
        PersonStudentDoctoral_WrongStudentEvent,
        PersonStudentDoctoral_InvalidEducationFeeType,
        PersonStudentDoctoral_InvalidRelocatedFromPart,
        PersonStudentDoctoral_RelocatedFileMissing,
        PersonStudentDoctoral_InvalidCourse,
        PersonStudentDoctoral_InvalidStudentSemester,
        PersonStudentDoctoral_InvalidIndividualPlanTwoYears,
        PersonStudentDoctoral_InvalidIndividualPlanTwoSemesters,
        PersonStudentDoctoral_SemesterNotUnique,
        PersonStudentDoctoral_MustHaveAtleastOneSemester,
        PersonStudentDoctoral_CantDeleteWhenHasMoreThanOneSemester,
        PersonStudentDoctoral_CantDeleteSelectedAsRelocated,
        PersonStudentDoctoral_InvalidProtocolNumber,
        PersonStudentDoctoral_InvalidProtocolDate,
        PersonStudentDoctoral_ProtocolDateCannotBeLessThanPrevious,
        PersonStudentDoctoral_InvalidProtocolType,
        PersonStudentDoctoral_InvalidStickerYear,
        PersonStudentDoctoral_InvalidDiplomaNumber,
        PersonStudentDoctoral_InvalidRegistrationDiplomaNumber,
        PersonStudentDoctoral_InvalidDiplomaDate,
        PersonStudentDoctoral_InvalidDuplicateStickerYear,
        PersonStudentDoctoral_InvalidDuplicateDiplomaNumber,
        PersonStudentDoctoral_InvalidDuplicateRegistrationDiplomaNumber,
        PersonStudentDoctoral_InvalidDuplicateDiplomaDate,

        PersonStudentProtocol_StudentNotFinishedCourse,

        PersonStudentSticker_ProtocolsError,
        PersonStudentSticker_SemesterError,
        PersonStudentSticker_SendForSticker,
        PersonStudentSticker_InvalidYear,
        PersonStudentSticker_ReturnForEdit,
        PersonStudentSticker_ForPrint,
        PersonStudentSticker_Recieved,
        PersonStudentSticker_ReissueSticker,
        PersonStudentSticker_DiplomaError,
        PersonStudentSticker_DuplicateDiplomaError,
        PersonStudentSticker_EditNotAllowed,

        PersonStudentDiploma_InvalidDiplomaNumber,
        PersonStudentDiploma_InvalidRegistrationDiplomaNumber,
        PersonStudentDiploma_MissingDiplomaFile,
        PersonStudentDiploma_NotUniqueDiploma,
        PersonStudentDiploma_InvalidDiplomaDate,

        PersonStudentDuplicateDiploma_NotUniqueValidDuplicateDiploma,

        // FileUpload
        FileUpload_WrongFileEncoding,
        FileUpload_FileSizeRestriction,
        FileUpload_WrongFileType,
        FileUpload_PleaseAttachFile,

        // RdpzsdImport
        RdpzsdImport_WrongState,
        RdpzsdImport_SpecialityImportInProgress,
        RdpzsdImport_WrongSpecialityImportType,
        RdpzsdImport_WrongPersonImportType
    }
}
