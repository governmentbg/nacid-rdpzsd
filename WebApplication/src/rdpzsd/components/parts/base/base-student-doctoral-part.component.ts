import { ChangeDetectorRef, Directive, EventEmitter, Input, Output } from "@angular/core";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { StudentEvent, StudentEventAfterRelocation, StudentEventAfterRelocationAbroad, StudentEventArrivedStudentMobility, StudentEventAttestation, StudentEventChangeEducationFeeType, StudentEventCompletedCourseFailedExams, StudentEventDeductedWithDefense, StudentEventGraduatedCourse, StudentEventGraduatedWithDiploma, StudentEventGraduatedWithoutDiploma, StudentEventIndividualPlanTwoSemesters, StudentEventIndividualPlanTwoYears, StudentEventInitialRegistration, StudentEventLeftOwnWill, StudentEventNextSemester, StudentEventNextSemesterAfterBreak, StudentEventRelocation, StudentEventRemovalNonFulfillment, StudentEventRestoredRightsAfterDeregistration, StudentEventSemesterRepetition, StudentEventUnpaidFee, StudentEventWentStudentMobility } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus, StudentStatusActive, StudentStatusCompleted, StudentStatusGraduated, StudentStatusInterrupted, StudentStatusProcessGraduation } from "src/nomenclatures/models/student-status/student-status.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { BasePersonSemester } from "src/rdpzsd/models/parts/base/base-person-semester.model";
import { BasePersonStudentDoctoral } from "src/rdpzsd/models/parts/base/base-person-student-doctoral.model";
import { PartInfo } from "src/rdpzsd/models/parts/base/part-info.model";
import { Semester } from "src/shared/enums/semester.enum";
import { UserDataService } from "src/users/services/user-data.service";

@Directive()
export abstract class BaseStudentDoctoralPartComponent<TPart extends BasePersonStudentDoctoral<TPartInfo, TSemester>, TPartInfo extends PartInfo, TSemester extends BasePersonSemester> {

    collapsed = true;

    @Output() partRemovedEvent: EventEmitter<void> = new EventEmitter<void>();

    @Input() personLotId: number;
    @Input() personLotState: LotState;

    studentStatusActive = StudentStatusActive;
    studentStatusInterrupted = StudentStatusInterrupted;
    studentStatusCompleted = StudentStatusCompleted;
    studentStatusGraduated = StudentStatusGraduated;
    studentStatusProcessGraduation = StudentStatusProcessGraduation;

    studentEventInitialRegistration = StudentEventInitialRegistration;
    studentEventAfterRelocation = StudentEventAfterRelocation;
    studentEventAfterRelocationAbroad = StudentEventAfterRelocationAbroad;
    studentEventArrivedStudentMobility = StudentEventArrivedStudentMobility;
    studentEventNextSemester = StudentEventNextSemester;
    studentEventAttestation = StudentEventAttestation;
    studentEventChangeEducationFeeType = StudentEventChangeEducationFeeType;
    studentEventNextSemesterAfterBreak = StudentEventNextSemesterAfterBreak;
    studentEventIndividualPlanTwoYears = StudentEventIndividualPlanTwoYears;
    studentEventIndividualPlanTwoSemesters = StudentEventIndividualPlanTwoSemesters;
    studentEventWentStudentMobility = StudentEventWentStudentMobility;
    studentEventSemesterRepetition = StudentEventSemesterRepetition;
    studentEventRestoredRightsAfterDeregistration = StudentEventRestoredRightsAfterDeregistration;
    studentEventCompletedCourseFailedExams = StudentEventCompletedCourseFailedExams;
    studentEventLeftOwnWill = StudentEventLeftOwnWill;
    studentEventRemovalNonFulfillment = StudentEventRemovalNonFulfillment;
    studentEventDeductedWithDefense = StudentEventDeductedWithDefense;
    studentEventGraduatedCourse = StudentEventGraduatedCourse;
    studentEventGraduatedWithoutDiploma = StudentEventGraduatedWithoutDiploma;
    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;

    studentEventUnpaidFee = StudentEventUnpaidFee;

    studentEventRelocation = StudentEventRelocation;

    semesterType = Semester;

    part: TPart
    @Input('part')
    set partSetter(part: TPart) {
        this.part = part;

        if (!this.part.id) {
            this.isEditMode = true;
            this.collapsed = false;
        }
    }

    partState = PartState;
    lotState = LotState;
    isEditMode = false;
    isValidForm = false;

    constructor(
        protected cdr: ChangeDetectorRef,
        public userDataService: UserDataService,
        protected studentStatusResource: BaseNomenclatureResource<StudentStatus, NomenclatureFilterDto>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource
    ) {
    }

    modelChange(part: TPart) {
        if (part) {
            this.part = part;
            this.customLogicOnUpdate();
        } else {
            this.partRemovedEvent.emit();
        }
    }

    addSemester(newSemester: TSemester) {
        this.part.semesters.unshift(newSemester);
        this.cdr.detectChanges();
    }

    removeSemester(index: number) {
        this.part.semesters.splice(index, 1);
        this.part.studentStatusId = this.part.semesters[0].studentStatusId;
        this.part.studentStatus = this.part.semesters[0].studentStatus;
        this.part.studentEvent = this.part.semesters[0].studentEvent;
        this.part.studentEventId = this.part.semesters[0].studentEventId;
        this.customLogicOnDeleteSemester();
    }

    updateSemester(index: number, updatedSemester: TSemester) {
        this.part.semesters[index] = updatedSemester;
        if (index === 0) {
            this.part.studentStatusId = updatedSemester.studentStatusId;
            this.part.studentStatus = updatedSemester.studentStatus;
            this.part.studentEvent = updatedSemester.studentEvent;
            this.part.studentEventId = updatedSemester.studentEventId;
        }
        this.customLogicOnUpdate();
    }

    protected getNextPeriod(year: number, semester: Semester) {
        return new Observable((observer: Observer<any>) => {
            return this.periodNomenclatureResource.getNextPeriod(year, semester)
                .pipe(
                    catchError(() => {
                        observer.complete();
                        return throwError(() => new Error('Next period not started'));
                    })
                )
                .subscribe(period => {
                    observer.next(period);
                    observer.complete();
                });
        });
    }

    protected getStudentStatusAlias(alias: string) {
        return new Observable((observer: Observer<any>) => {
            return this.studentStatusResource.getByAlias('StudentStatus', alias)
                .pipe(
                    catchError(() => {
                        observer.complete();
                        return throwError(() => new Error('No student status found'));
                    })
                )
                .subscribe(studentStatus => {
                    observer.next(studentStatus);
                    observer.complete();
                });
        });
    }

    protected getStudentEventAlias(alias: string) {
        return new Observable((observer: Observer<any>) => {
            return this.studentEventResource.getByAlias('StudentEvent', alias)
                .pipe(
                    catchError(() => {
                        observer.complete();
                        return throwError(() => new Error('No student event found'));
                    })
                )
                .subscribe(studentEvent => {
                    observer.next(studentEvent);
                    observer.complete();
                });
        });
    }

    customLogicOnUpdate() {

    }

    customLogicOnDeleteSemester() {

    }
}