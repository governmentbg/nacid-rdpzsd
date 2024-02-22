import { ChangeDetectorRef, Component, Input, ViewChild } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { forkJoin } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { StudentEvent, StudentEventGraduatedWithDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { PartHistoryDto } from "src/rdpzsd/dtos/parts/part-history.dto";
import { StudentProtocolType } from "src/rdpzsd/enums/parts/student-protocol-type.enum";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentHistory } from "src/rdpzsd/models/parts/person-student/history/person-student-history.model";
import { PersonStudentInfo } from "src/rdpzsd/models/parts/person-student/person-student-info.model";
import { PersonStudentProtocol } from "src/rdpzsd/models/parts/person-student/person-student-protocol.model";
import { PersonStudentSemester } from "src/rdpzsd/models/parts/person-student/person-student-semester.model";
import { PersonStudent } from "src/rdpzsd/models/parts/person-student/person-student.model";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { UserDataService } from "src/users/services/user-data.service";
import { BaseStudentDoctoralPartComponent } from "../base/base-student-doctoral-part.component";
import { PersonStudentStickerNotesModalComponent } from "./graduation/stickers/person-student-sticker-notes.modal";
import { PersonStudentStickerValidationInfoComponent } from "./graduation/stickers/person-student-sticker-validation-info.component";
import { PersonStudentHistoryComponent } from "./person-student-history.component";

@Component({
    selector: 'person-student-part',
    templateUrl: './person-student-part.component.html',
    providers: [BaseNomenclatureResource]
})
export class PersonStudentPartComponent extends BaseStudentDoctoralPartComponent<PersonStudent, PersonStudentInfo, PersonStudentSemester> {

    part: PersonStudent
    @Input('part')
    set partSetter(part: PersonStudent) {
        this.part = part;
        this.lastActiveSemester = this.isLastActiveSemester();

        if (!this.part.id) {
            this.isEditMode = true;
            this.collapsed = false;
        }

        if (this.part?.id
            && this.currentPersonContext.openedFromStickers
            && this.part.id === this.currentPersonContext.personStudentStickerDto?.partId) {
            this.collapsed = false;
        }
    }

    @ViewChild('stickerValidationInfo') stickerValidationInfoComponent: PersonStudentStickerValidationInfoComponent;

    studentProtocolType = StudentProtocolType;
    studentStickerState = StudentStickerState;

    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;

    lastActiveSemester = false;

    constructor(
        protected cdr: ChangeDetectorRef,
        public userDataService: UserDataService,
        private modalService: NgbModal,
        public currentPersonContext: CurrentPersonContextService,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        public configuration: Configuration,
        protected studentStatusResource: BaseNomenclatureResource<StudentStatus, NomenclatureFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource
    ) {
        super(cdr, userDataService, studentStatusResource, studentEventResource, periodNomenclatureResource)
    }

    showHistory(partHistoryDto: PartHistoryDto<PersonStudent, PersonStudentHistory>) {
        const modal = this.modalService.open(PersonStudentHistoryComponent, { backdrop: 'static', size: 'xl' });
        modal.componentInstance.personStudentHistoryDto = partHistoryDto;
    }

    addProtocol() {
        const newProtocol = new PersonStudentProtocol();
        newProtocol.protocolDate = new Date();
        newProtocol.protocolNumber = null;
        newProtocol.protocolType = this.studentProtocolType.stateExamination;
        newProtocol.isEditMode = true;
        newProtocol.partId = this.part.id;
        this.part.protocols.push(newProtocol);
    }

    removeProtocol(index: number) {
        this.part.protocols.splice(index, 1);
    }

    openStickerNotesModal() {
        const modal = this.modalService.open(PersonStudentStickerNotesModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.stickerNotes = this.part.stickerNotes;
    }

    updateStudentEvent() {
        return this.studentEventResource.getByAlias('StudentEvent', this.studentEventGraduatedWithDiploma)
            .subscribe(studentEvent => {
                this.part.studentStatus = studentEvent.studentStatus;
                this.part.studentStatusId = studentEvent.studentStatus.id;
                this.part.studentEvent = studentEvent;
                this.part.studentEventId = studentEvent.id;
            });
    }

    getInstitutionSpecialityJointSpecialitiesIds(): number[] {
        if (this.part?.institutionSpeciality?.institutionSpecialityJointSpecialities?.length > 0) {
            return this.part?.institutionSpeciality.institutionSpecialityJointSpecialities.map(e => e.institutionId);
        }
        return [];
    }

    rsdUserForJointSpecialityCanAccessSemester(): boolean {
        if (this.getInstitutionSpecialityJointSpecialitiesIds().includes(this.userDataService.userData.institution.id)) {
            return true;
        }
        return false;
    }

    nextSemester(studentStatusAlias: string) {
        const newSemester = new PersonStudentSemester();
        newSemester.partId = this.part.id;

        newSemester.semesterInstitution = this.part.semesters[0].semesterInstitution;
        newSemester.semesterInstitutionId = this.part.semesters[0].semesterInstitutionId;

        if (studentStatusAlias !== this.studentStatusActive) {

            if (studentStatusAlias === this.studentStatusProcessGraduation
                && this.part.semesters[0].studentStatus?.alias === this.studentStatusActive) {
                const studentEventAlias = this.studentEventGraduatedCourse;

                return this.getStudentEventAlias(studentEventAlias)
                    .subscribe(studentEvent => {
                        newSemester.period = this.part.semesters[0].period;
                        newSemester.periodId = this.part.semesters[0].periodId;
                        newSemester.studentStatus = studentEvent.studentStatus;
                        newSemester.studentStatusId = studentEvent.studentStatus.id;
                        newSemester.studentEvent = studentEvent;
                        newSemester.studentEventId = studentEvent.id;
                        newSemester.lastSemesterStudentStatusAlias = this.part.semesters[0].studentStatus?.alias;
                        this.customSemesterPropertyAdd(newSemester, this.part.semesters[0].studentStatus?.alias, this.part.semesters[0].studentEvent?.alias);
                        this.currentPersonContext.setIsInEdit(true);
                        this.addSemester(newSemester);
                        this.lastActiveSemester = this.isLastActiveSemester();
                    });
            }
            else if (studentStatusAlias === this.studentStatusCompleted
                || (studentStatusAlias === this.studentStatusInterrupted && this.part.semesters[0].studentStatus?.alias === this.studentStatusActive)) {
                return this.getStudentStatusAlias(studentStatusAlias)
                    .subscribe(studentStatus => {
                        newSemester.period = this.part.semesters[0].period;
                        newSemester.periodId = this.part.semesters[0].periodId;
                        newSemester.studentStatus = studentStatus;
                        newSemester.studentStatusId = studentStatus.id;
                        newSemester.lastSemesterStudentStatusAlias = this.part.semesters[0].studentStatus?.alias;
                        this.customSemesterPropertyAdd(newSemester, this.part.semesters[0].studentStatus?.alias, this.part.semesters[0].studentEvent?.alias);
                        this.currentPersonContext.setIsInEdit(true);
                        this.addSemester(newSemester);
                        this.lastActiveSemester = this.isLastActiveSemester();
                    });
            } else {
                return forkJoin([
                    this.getNextPeriod(this.part.semesters[0].period.year, this.part.semesters[0].period.semester),
                    this.getStudentStatusAlias(studentStatusAlias)])
                    .subscribe(([nextPeriod, studentStatus]) => {
                        newSemester.period = nextPeriod;
                        newSemester.periodId = nextPeriod.id;
                        newSemester.studentStatus = studentStatus;
                        newSemester.studentStatusId = studentStatus.id;
                        newSemester.lastSemesterStudentStatusAlias = this.part.semesters[0].studentStatus?.alias;
                        this.customSemesterPropertyAdd(newSemester, this.part.semesters[0].studentStatus?.alias, this.part.semesters[0].studentEvent?.alias);
                        this.currentPersonContext.setIsInEdit(true);
                        this.addSemester(newSemester);
                        this.lastActiveSemester = this.isLastActiveSemester();
                    });
            }
        } else {
            const studentEventAlias = this.part.semesters[0]?.studentStatus?.alias === this.studentStatusCompleted
                ? this.studentEventRestoredRightsAfterDeregistration
                : (this.part.semesters[0]?.studentEvent?.alias === this.studentEventIndividualPlanTwoYears && !this.part.semesters[0].secondFromTwoYearsPlan)
                    ? this.studentEventIndividualPlanTwoYears
                    : (this.part.semesters[0]?.studentStatus?.alias === this.studentStatusActive && this.isLastActiveSemester())
                        ? this.studentEventCompletedCourseFailedExams
                        : this.part.semesters[0]?.studentStatus?.alias === this.studentStatusActive
                            ? this.studentEventNextSemester
                            : this.studentEventNextSemesterAfterBreak;

            return forkJoin([
                this.getNextPeriod(this.part.semesters[0].period.year, this.part.semesters[0].period.semester),
                this.getStudentEventAlias(studentEventAlias)])
                .subscribe(([nextPeriod, studentEvent]) => {
                    newSemester.period = nextPeriod;
                    newSemester.periodId = nextPeriod.id;
                    newSemester.studentStatus = studentEvent.studentStatus;
                    newSemester.studentStatusId = studentEvent.studentStatus.id;
                    newSemester.studentEvent = studentEvent;
                    newSemester.studentEventId = studentEvent.id;
                    newSemester.lastSemesterStudentStatusAlias = this.part.semesters[0].studentStatus?.alias;
                    newSemester.secondFromTwoYearsPlan = (this.part.semesters[0]?.studentEvent?.alias === this.studentEventIndividualPlanTwoYears && !this.part.semesters[0].secondFromTwoYearsPlan);
                    this.customSemesterPropertyAdd(newSemester, this.part.semesters[0].studentStatus?.alias, this.part.semesters[0].studentEvent?.alias);
                    this.currentPersonContext.setIsInEdit(true);
                    this.addSemester(newSemester);
                    this.lastActiveSemester = this.isLastActiveSemester();
                });
        }
    }

    customSemesterPropertyAdd(newSemester: PersonStudentSemester, previousStudentStatus: string, previousStudentEvent: string) {
        if (newSemester.studentStatus.alias !== this.studentStatusActive
            || (newSemester.studentStatus.alias === this.studentStatusActive &&
                (previousStudentStatus === this.studentStatusInterrupted || this.lastActiveSemester))) {

            if ((newSemester.studentStatus.alias === this.studentStatusActive && this.lastActiveSemester)
                || (newSemester.studentStatus.alias === this.studentStatusProcessGraduation && (previousStudentEvent === this.studentEventIndividualPlanTwoSemesters || this.part.semesters[0].secondFromTwoYearsPlan))) {
                newSemester.course = this.part.semesters[0].individualPlanCourse ?? this.part.semesters[0].course;
                newSemester.studentSemester = this.part.semesters[0].individualPlanSemester ?? this.part.semesters[0].studentSemester;
            } else {
                newSemester.course = this.part.semesters[0].course;
                newSemester.studentSemester = this.part.semesters[0].studentSemester;
            }

        } else {
            if (previousStudentEvent === this.studentEventIndividualPlanTwoSemesters || this.part.semesters[0].secondFromTwoYearsPlan) {
                if (this.part.semesters[0].individualPlanSemester === this.semesterType.first) {
                    newSemester.course = this.part.semesters[0].individualPlanCourse;
                    newSemester.studentSemester = this.semesterType.second;
                } else {
                    newSemester.course = this.part.semesters[0].individualPlanCourse + 1;
                    newSemester.studentSemester = this.semesterType.first;
                }
            } else {
                if (this.part.semesters[0].studentSemester === this.semesterType.first) {
                    newSemester.course = this.part.semesters[0].course;
                    newSemester.studentSemester = this.semesterType.second;
                } else {
                    newSemester.course = this.part.semesters[0].course + 1;
                    newSemester.studentSemester = this.semesterType.first;
                }

                if (previousStudentEvent === this.studentEventIndividualPlanTwoYears) {
                    newSemester.individualPlanCourse = newSemester.course + 1;
                    newSemester.individualPlanSemester = newSemester.studentSemester;
                }
            }
        }
    }

    private isLastActiveSemester() {
        const specialityDuration = this.part?.institutionSpeciality?.duration;
        if (specialityDuration % 1 === 0) {
            if ((this.part.semesters[0].course !== specialityDuration || this.part.semesters[0].studentSemester !== this.semesterType.second)
                && (this.part.semesters[0].individualPlanCourse !== specialityDuration || this.part.semesters[0].individualPlanSemester !== this.semesterType.second)) {
                return false;
            } else {
                return true;
            }
        } else {
            if ((this.part.semesters[0].course !== Math.ceil(specialityDuration) || this.part.semesters[0].studentSemester !== this.semesterType.first)
                && (this.part.semesters[0].individualPlanCourse !== Math.ceil(specialityDuration) || this.part.semesters[0].individualPlanSemester !== this.semesterType.first)) {
                return false;
            } else {
                return true;
            }
        }
    }

    customLogicOnUpdate() {
        if (this.stickerValidationInfoComponent && this.part?.studentStatus?.alias === this.studentStatusProcessGraduation) {
            this.stickerValidationInfoComponent.validateStickerInfo();
        }
    }

    customLogicOnDeleteSemester() {
        this.lastActiveSemester = this.isLastActiveSemester();
    }
}