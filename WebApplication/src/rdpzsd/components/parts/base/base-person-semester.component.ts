import { Directive, EventEmitter, Input, Output } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { AdmissionReason } from "src/nomenclatures/models/others/admission-reason.model";
import { StudentEventAfterRelocation, StudentEventAfterRelocationAbroad, StudentEventArrivedStudentMobility, StudentEventAttestation, StudentEventChangeEducationFeeType, StudentEventCompletedCourseFailedExams, StudentEventDeceased, StudentEventDeductedNoDefense, StudentEventDeductedWithDefense, StudentEventExtendedTerm, StudentEventGraduatedCourse, StudentEventIndividualPlanTwoSemesters, StudentEventIndividualPlanTwoYears, StudentEventInitialRegistration, StudentEventLeftOwnWill, StudentEventNextSemester, StudentEventNextSemesterAfterBreak, StudentEventPostponementFailedExam, StudentEventPostponementIllness, StudentEventPostponementOwnWill, StudentEventPostponementPregnancy, StudentEventRelocation, StudentEventRelocationAbroad, StudentEventRemovalConviction, StudentEventRemovalIncorrectData, StudentEventRemovalNonFulfillment, StudentEventRestoredRightsAfterDeregistration, StudentEventSemesterRepetition, StudentEventUnpaidFee, StudentEventWentStudentMobility } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatusActive, StudentStatusCompleted, StudentStatusGraduated, StudentStatusInterrupted, StudentStatusProcessGraduation } from "src/nomenclatures/models/student-status/student-status.model";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { BasePersonSemester } from "src/rdpzsd/models/parts/base/base-person-semester.model";
import { PersonSemesterResource } from "src/rdpzsd/resources/parts/person-semester.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { StudentStatusChangeService } from "src/shared/services/student-statuses/student-status-change.service";
import { UserDataService } from "src/users/services/user-data.service";
import { InstitutionSpeciality } from 'src/nomenclatures/models/institution/institution-speciality.model';

@Directive()
export abstract class BasePersonSemesterComponent<TSemester extends BasePersonSemester> {

    @Output() removeSemester: EventEmitter<void> = new EventEmitter<void>();
    @Output() updateSemester: EventEmitter<TSemester> = new EventEmitter<TSemester>();

    semester: TSemester;
    @Input('semester')
    set semesterSetter(semester: TSemester) {
        this.semester = semester;

        if (!this.semester.id) {
            this.isEditMode = true;
        }

        this.customSemesterAdditionalSetter();
    }

    @Input() personLotState: LotState;
    @Input() partState: PartState;
    @Input() partId: number;
    @Input() institutionId: number;
    @Input() educationalQualificationId: number;
    @Input() semesterIndex: number;
    @Input() enablePeriodAndEventEdit = false;
    @Input() institutionSpeciality: InstitutionSpeciality;

    @Input() personStudentStatusAlias: string;

    admissionReason: AdmissionReason;
    @Input('admissionReason')
    set admissionReasonSetter(admissionReason: AdmissionReason) {
        this.admissionReason = admissionReason;

        if (!this.partId) {
            this.semester.educationFeeType = null;
            this.semester.educationFeeTypeId = null;

            if (this.admissionReason?.admissionReasonEducationFees?.length === 1) {
                this.semester.educationFeeType = this.admissionReason?.admissionReasonEducationFees[0]?.educationFeeType;
                this.semester.educationFeeTypeId = this.admissionReason?.admissionReasonEducationFees[0]?.educationFeeTypeId;
            }
        }
    }

    lotState = LotState;
    isEditMode = false;
    partStateType = PartState;
    originalModel: TSemester = null;

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

    studentEventGraduatedCourse = StudentEventGraduatedCourse;
    studentEventDeductedWithDefense = StudentEventDeductedWithDefense;

    studentEventPostponementFailedExam = StudentEventPostponementFailedExam;
    studentEventpostponementOwnWill = StudentEventPostponementOwnWill;
    studentEventPostponementIllness = StudentEventPostponementIllness;
    studentEventPostponementPregnancy = StudentEventPostponementPregnancy;

    studentEventUnpaidFee = StudentEventUnpaidFee;

    studentEventRelocation = StudentEventRelocation;


    studentCompletedEvents: string[] = [StudentEventRelocation, StudentEventLeftOwnWill, StudentEventRelocationAbroad, StudentEventDeductedNoDefense,
        StudentEventRemovalIncorrectData, StudentEventRemovalNonFulfillment, StudentEventRemovalConviction, StudentEventDeceased];

    studentProcessGraduationEvents: string[] = [StudentEventPostponementFailedExam, StudentEventPostponementOwnWill, StudentEventPostponementIllness, StudentEventPostponementPregnancy];

    studentInitialActiveEvents: string[] = [StudentEventInitialRegistration, StudentEventAfterRelocation, StudentEventAfterRelocationAbroad,
        StudentEventArrivedStudentMobility, StudentEventRestoredRightsAfterDeregistration];

    studentActiveAfterBreakEvents: string[] = [StudentEventNextSemesterAfterBreak, StudentEventWentStudentMobility, StudentEventSemesterRepetition];

    studentActiveEvents: string[] = [StudentEventNextSemester, StudentEventAttestation, StudentEventChangeEducationFeeType, StudentEventExtendedTerm, StudentEventWentStudentMobility];

    constructor(
        public userDataService: UserDataService,
        public studentStatusChangeService: StudentStatusChangeService,
        protected resource: PersonSemesterResource<TSemester>,
        protected resourceUrl: string,
        protected modalService: NgbModal,
        public currentUserContext: CurrentPersonContextService
    ) {
        this.resource.init(this.resourceUrl);
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.semester)) as TSemester;
        this.isEditMode = true;
        this.currentUserContext.setIsInEdit(true);
    }

    save() {
        if (this.semester.id) {
            return this.resource
                .update(this.semester)
                .subscribe(updatedSemester => {
                    this.semester = updatedSemester;
                    this.originalModel = null;
                    this.isEditMode = false;
                    this.currentUserContext.setIsInEdit(false);
                    this.updateSemester.emit(this.semester);
                });
        } else {
            return this.resource
                .create(this.semester)
                .subscribe(newSemester => {
                    this.semester = newSemester;
                    this.originalModel = null;
                    this.isEditMode = false;
                    this.currentUserContext.setIsInEdit(false);
                    this.updateSemester.emit(this.semester);
                });
        }
    }

    delete() {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personLot.modals.deleteSemester';
        modal.componentInstance.acceptButton = 'root.buttons.delete';

        modal.result.then((ok) => {
            if (ok) {
                return this.resource
                    .delete(this.semester.id, this.partId)
                    .subscribe(() => {
                        this.removeSemester.emit();
                        this.currentUserContext.setIsInEdit(false);
                    });
            } else {
                return null;
            }
        });
    }

    cancel() {
        if (this.semester.id) {
            this.semester = JSON.parse(JSON.stringify(this.originalModel)) as TSemester;
            this.updateSemester.emit(this.semester);
            this.originalModel = null;
        } else {
            this.removeSemester.emit();
        }

        this.isEditMode = false;
        this.currentUserContext.setIsInEdit(false);
    }

    customSemesterAdditionalSetter() {

    }
}