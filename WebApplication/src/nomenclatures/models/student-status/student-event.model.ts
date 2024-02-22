import { Nomenclature } from "../base/nomenclature.model";
import { StudentEventQualification } from "./student-event-qualification.model";
import { StudentStatus } from "./student-status.model";

export const StudentEventInitialRegistration = 'initialRegistration';
export const StudentEventAfterRelocation = 'nextSemesterAfterRelocation';
export const StudentEventAttestation = 'attestation';
export const StudentEventExtendedTerm = 'extendedTerm';
export const StudentEventAfterRelocationAbroad = 'nextSemesterAfterRelocationAbroad';
export const StudentEventArrivedStudentMobility = 'arrivedStudentMobility';
export const StudentEventNextSemester = 'nextSemester';
export const StudentEventNextSemesterAfterBreak = 'nextSemesterAfterBreak';
export const StudentEventIndividualPlanTwoYears = 'individualPlanTwoYears';
export const StudentEventIndividualPlanTwoSemesters = 'individualPlanTwoSemesters';
export const StudentEventWentStudentMobility = 'wentStudentMobility';
export const StudentEventSemesterRepetition = 'semesterRepetition';
export const StudentEventRestoredRightsAfterDeregistration = 'restoredRightsAfterDeregistration';
export const StudentEventCompletedCourseFailedExams = 'completedCourseWthFailedExams';
export const StudentEventChangeEducationFeeType = 'changeEducationFeeType';

export const StudentEventUnpaidFee = 'unpaidFee';

export const StudentEventRelocation = 'relocation';
export const StudentEventLeftOwnWill = 'leftOwnWill';
export const StudentEventRelocationAbroad = 'relocationAbroad';
export const StudentEventDeductedNoDefense = 'deductedNoDefense';
export const StudentEventRemovalIncorrectData = 'removalIncorrectData';
export const StudentEventRemovalNonFulfillment = 'removalNonFulfillment';
export const StudentEventRemovalConviction = 'removalConviction';
export const StudentEventDeceased = 'deceased';

export const StudentEventGraduatedWithoutDiploma = 'graduatedWithoutDiploma';
export const StudentEventGraduatedWithDiploma = 'graduatedWithDiploma';

export const StudentEventGraduatedCourse = 'graduatedCourse';
export const StudentEventDeductedWithDefense = 'deductedWithDefense';
export const StudentEventPostponementFailedExam = 'postponementFailedExam';
export const StudentEventPostponementOwnWill = 'postponementOwnWill';
export const StudentEventPostponementIllness = 'postponementIllness';
export const StudentEventPostponementPregnancy = 'postponementPregnancy';

export class StudentEvent extends Nomenclature {
    alias: string;
    studentStatusId: number;
    studentStatus: StudentStatus;

    studentEventQualifications: StudentEventQualification[] = [];
}