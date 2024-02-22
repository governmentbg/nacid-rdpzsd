import { Component, Input } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { Period } from "src/nomenclatures/models/others/period.model";
import { StudentEvent, StudentEventDeceased, StudentEventDeductedNoDefense, StudentEventLeftOwnWill, StudentEventRelocation, StudentEventRelocationAbroad, StudentEventRemovalConviction, StudentEventRemovalIncorrectData, StudentEventRemovalNonFulfillment } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentSemester } from "src/rdpzsd/models/parts/person-student/person-student-semester.model";
import { PersonSemesterResource } from "src/rdpzsd/resources/parts/person-semester.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { Level } from "src/shared/enums/level.enum";
import { Semester } from "src/shared/enums/semester.enum";
import { CourseSemesterCollectionService, CourseSemesterDto } from "src/shared/services/course-semester/course-semester.service";
import { FileUploadService } from "src/shared/services/file-upload.service";
import { InstitutionChangeService } from "src/shared/services/institutions/institution-change.service";
import { StudentStatusChangeService } from "src/shared/services/student-statuses/student-status-change.service";
import { UserDataService } from "src/users/services/user-data.service";
import { BasePersonSemesterComponent } from "../base/base-person-semester.component";

@Component({
    selector: 'tr[person-student-semester]',
    templateUrl: './person-student-semester.component.html',
    providers: [
        BaseNomenclatureResource,
        PersonSemesterResource
    ]
})
export class PersonStudentSemesterComponent extends BasePersonSemesterComponent<PersonStudentSemester> {

    courseSemesterCollection: CourseSemesterDto[] = [];

    specialityDuration: number;
    @Input('specialityDuration')
    set specialityDurationSetter(specialityDuration: number) {
        this.specialityDuration = specialityDuration;

        if (!this.partId) {
            const courseSemester = new CourseSemesterDto('enums.courseTypeSemester.firstCourse_first', CourseType.first, Semester.first);
            this.semester.courseSemester = courseSemester;
            this.semester.course = CourseType.first;
            this.semester.studentSemester = Semester.first;
        }

        if (specialityDuration % 1 === 0) {
            if ((this.semester.course !== specialityDuration || this.semester.studentSemester !== this.semesterType.second)
                && (this.semester.individualPlanCourse !== specialityDuration || this.semester.individualPlanSemester !== this.semesterType.second)) {
                this.studentCompletedEvents = [StudentEventRelocation, StudentEventLeftOwnWill, StudentEventRelocationAbroad,
                    StudentEventDeductedNoDefense, StudentEventRemovalIncorrectData, StudentEventRemovalNonFulfillment,
                    StudentEventRemovalConviction, StudentEventDeceased];
            }

            this.courseSemesterCollection = this.courseSemesterCollectionService.items.filter(e => e.course <= specialityDuration);
        } else {
            if ((this.semester.course !== Math.ceil(specialityDuration) || this.semester.studentSemester !== this.semesterType.first)
                && (this.semester.individualPlanCourse !== Math.ceil(specialityDuration) || this.semester.individualPlanSemester !== this.semesterType.first)) {
                this.studentCompletedEvents = [StudentEventRelocation, StudentEventLeftOwnWill, StudentEventRelocationAbroad,
                    StudentEventDeductedNoDefense, StudentEventRemovalIncorrectData, StudentEventRemovalNonFulfillment,
                    StudentEventRemovalConviction, StudentEventDeceased];
            }

            this.courseSemesterCollection = this.courseSemesterCollectionService.items
                .filter(e => (e.course < specialityDuration) || (e.course === Math.ceil(specialityDuration) && e.semester === Semester.first));
        }

        if (this.semester.course > 1 && !this.isRegulatedSpeciality) {
            const lastCourseSemester = this.courseSemesterCollection[this.courseSemesterCollection.length - 1];

            if (lastCourseSemester.course >= this.semester.course + 1 && this.semester.studentSemester === this.semesterType.first) {
                this.studentActiveEvents = [this.studentEventNextSemester, this.studentEventWentStudentMobility, this.studentEventIndividualPlanTwoYears, this.studentEventIndividualPlanTwoSemesters];
                this.studentActiveAfterBreakEvents = [this.studentEventNextSemesterAfterBreak, this.studentEventWentStudentMobility, this.studentEventIndividualPlanTwoYears, this.studentEventIndividualPlanTwoSemesters, this.studentEventSemesterRepetition];
            } else {
                this.studentActiveEvents = [this.studentEventNextSemester, this.studentEventWentStudentMobility, this.studentEventIndividualPlanTwoSemesters];
                this.studentActiveAfterBreakEvents = [this.studentEventNextSemesterAfterBreak, this.studentEventWentStudentMobility, this.studentEventIndividualPlanTwoSemesters, this.studentEventSemesterRepetition];
            }
        }

        if (this.partId > 0) {
            this.courseSemesterCollection = this.courseSemesterCollectionService.items
                .filter(e => e.course < this.semester.course || (e.course == this.semester.course && e.semester <= this.semester.studentSemester));
        }
    }

    @Input() lastActiveSemester = false;
    @Input() isRegulatedSpeciality = false;
    @Input() stickerState: StudentStickerState;
    @Input() previousPeriod: Period;

    courseType = CourseType;
    semesterType = Semester;
    level = Level;
    studentStickerState = StudentStickerState;

    constructor(
        public userDataService: UserDataService,
        public studentStatusChangeService: StudentStatusChangeService,
        protected resource: PersonSemesterResource<PersonStudentSemester>,
        protected studentStatusResource: BaseNomenclatureResource<StudentStatus, NomenclatureFilterDto>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource,
        protected modalService: NgbModal,
        public courseSemesterCollectionService: CourseSemesterCollectionService,
        public translateService: TranslateService,
        public currentUserContext: CurrentPersonContextService,
        private fileUploadService: FileUploadService,
        public institutionChangeService: InstitutionChangeService,
        public configuration: Configuration
    ) {
        super(userDataService, studentStatusChangeService, resource, 'PersonStudentSemester', modalService, currentUserContext);
    }

    studentEventChanged(studentEvent: StudentEvent) {
        this.semester.relocatedFromPart = null;
        this.semester.relocatedFromPartId = null;

        if (!(studentEvent?.alias === this.studentEventAfterRelocation || studentEvent?.alias === this.studentEventAfterRelocationAbroad)) {
            this.semester.semesterRelocatedFile = null;
        }

        if (studentEvent?.alias === this.studentEventInitialRegistration) {
            this.semester.course = this.courseType.first;
            this.semester.studentSemester = this.semesterType.first;
            this.semester.courseSemester = new CourseSemesterDto(`enums.courseTypeSemester.${this.courseType[this.semester.course]}Course_${this.semesterType[this.semester.studentSemester]}`, this.semester.course, this.semester.studentSemester);
        } else if (studentEvent?.alias === this.studentEventIndividualPlanTwoYears || studentEvent?.alias === this.studentEventIndividualPlanTwoSemesters) {
            this.constructIndividualPlan(studentEvent?.alias);
        } else {
            if (this.semester.studentStatus?.alias === this.studentStatusInterrupted && this.semester.lastSemesterStudentStatusAlias !== this.studentStatusInterrupted && !this.lastActiveSemester) {
                this.constructStudentEventInterrupted(studentEvent?.alias);
            }

            this.semester.individualPlanCourse = null;
            this.semester.individualPlanSemester = null;
        }

        this.studentStatusChangeService.studentEventChange(this.semester, studentEvent, 'studentEvent', 'studentStatus');
    }

    customSemesterAdditionalSetter() {
        if (this.semester.course && this.semester.studentSemester) {
            this.semester.courseSemester = new CourseSemesterDto(`enums.courseTypeSemester.${this.courseType[this.semester.course]}Course_${this.semesterType[this.semester.studentSemester]}`, this.semester.course, this.semester.studentSemester);
        } else {
            this.semester.courseSemester = null;
        }
    }

    constructIndividualPlan(studentEventAlias: string) {
        if (studentEventAlias === this.studentEventIndividualPlanTwoYears) {
            this.semester.individualPlanCourse = this.semester.course + 1;
            this.semester.individualPlanSemester = this.semester.studentSemester;
        } else if (studentEventAlias === this.studentEventIndividualPlanTwoSemesters) {
            if (this.semester.studentSemester === this.semesterType.first) {
                this.semester.individualPlanCourse = this.semester.course;
                this.semester.individualPlanSemester = this.semesterType.second;
            } else if (this.semester.studentSemester === this.semesterType.second) {
                this.semester.individualPlanCourse = this.semester.course + 1;
                this.semester.individualPlanSemester = this.semesterType.first;
            }
        }
    }

    constructStudentEventInterrupted(studentEventInterruptedAlias: string) {
        if (studentEventInterruptedAlias === this.studentEventUnpaidFee && this.semester?.studentEvent?.alias !== this.studentEventUnpaidFee) {
            this.periodNomenclatureResource.getNextPeriod(this.semester.period?.year, this.semester.period?.semester)
                .subscribe(e => {
                    this.semester.period = e;
                    this.semester.periodId = e.id;
                    if (this.semester.studentSemester === this.semesterType.first) {
                        this.semester.studentSemester = this.semesterType.second;
                    } else if (this.semester.studentSemester === this.semesterType.second) {
                        this.semester.studentSemester = this.semesterType.first;
                        this.semester.course = this.semester.course + 1;
                    }

                    this.semester.courseSemester = new CourseSemesterDto(`enums.courseTypeSemester.${this.courseType[this.semester.course]}Course_${this.semesterType[this.semester.studentSemester]}`, this.semester.course, this.semester.studentSemester);
                });
        } else if (studentEventInterruptedAlias !== this.studentEventUnpaidFee && this.semester?.studentEvent?.alias === this.studentEventUnpaidFee) {
            this.periodNomenclatureResource.getPreviousPeriod(this.semester.period?.year, this.semester.period?.semester)
                .subscribe(e => {
                    this.semester.period = e;
                    this.semester.periodId = e.id;
                    if (this.semester.studentSemester === this.semesterType.first) {
                        this.semester.studentSemester = this.semesterType.second;
                        this.semester.course = this.semester.course - 1;
                    } else if (this.semester.studentSemester === this.semesterType.second) {
                        this.semester.studentSemester = this.semesterType.first;
                    }

                    this.semester.courseSemester = new CourseSemesterDto(`enums.courseTypeSemester.${this.courseType[this.semester.course]}Course_${this.semesterType[this.semester.studentSemester]}`, this.semester.course, this.semester.studentSemester);
                });
        }
    }

    getInstitutionSpecialityJointSpecialitiesIds(): number[] {
        if (this.institutionSpeciality?.institutionSpecialityJointSpecialities?.length > 0) {
            return this.institutionSpeciality.institutionSpecialityJointSpecialities.map(e => e.institutionId);
        }
        return [];
    }

    rsdUserForJointSpecialityCanAccessSemester(): boolean {
        if (this.getInstitutionSpecialityJointSpecialitiesIds().includes(this.userDataService.userData.institution.id)) {
            return true;
        }
        return false;
    }

    getRelocationFile() {
        return this.fileUploadService.getFileToAnchorUrl(this.semester.semesterRelocatedFile);
    }
}