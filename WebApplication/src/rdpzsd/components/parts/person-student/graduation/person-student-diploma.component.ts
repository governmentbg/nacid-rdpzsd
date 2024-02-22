import { Component, EventEmitter, Input, Output } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import * as saveAs from "file-saver";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { StudentEventGraduatedWithDiploma, StudentEventGraduatedWithoutDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { PersonStudentDiploma } from "src/rdpzsd/models/parts/person-student/person-student-diploma.model";
import { PersonStudentDiplomaResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-diploma.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { FileUploadService } from "src/shared/services/file-upload.service";
import { UserDataService } from "src/users/services/user-data.service";

@Component({
    selector: 'person-student-diploma',
    templateUrl: './person-student-diploma.component.html',
    providers: [PersonStudentDiplomaResource]
})
export class PersonStudentDiplomaComponent {

    studentEventGraduatedWithoutDiploma = StudentEventGraduatedWithoutDiploma;
    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;

    @Input() diploma: PersonStudentDiploma = null;
    originalModel: PersonStudentDiploma = null;
    isEditMode = false;

    @Input() partId: number;
    @Input() personLotState: LotState;
    @Input() partState: PartState;
    @Input() institutionId: number;
    @Input() studentEventAlias: string;

    @Output() updateStudentEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() updateDiplomaEvent: EventEmitter<PersonStudentDiploma> = new EventEmitter<PersonStudentDiploma>();

    partStateType = PartState;
    lotState = LotState;

    constructor(
        public userDataService: UserDataService,
        public currentPersonContextService: CurrentPersonContextService,
        private resource: PersonStudentDiplomaResource,
        private modalService: NgbModal,
        private fileUploadService: FileUploadService,
        public configuration: Configuration
    ) {
    }

    addDiploma() {
        this.edit();
        this.diploma = new PersonStudentDiploma();
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.diploma));
        this.isEditMode = true;
        this.currentPersonContextService.setIsInEdit(true);
    }

    save() {
        if (this.diploma?.id) {
            return this.resource
                .update(this.diploma)
                .subscribe(updatedDiploma => {
                    this.diploma = updatedDiploma;
                    this.originalModel = null;
                    this.isEditMode = false;
                    this.currentPersonContextService.setIsInEdit(false);
                    this.updateDiplomaEvent.emit(updatedDiploma);
                });
        } else {
            this.diploma.id = this.partId;

            return this.resource
                .create(this.diploma)
                .subscribe(newDiploma => {
                    this.updateStudentEvent.emit();
                    this.diploma = newDiploma;
                    this.originalModel = null;
                    this.isEditMode = false;
                    this.currentPersonContextService.setIsInEdit(false);
                    this.updateDiplomaEvent.emit(newDiploma);
                });
        }
    }

    cancel() {
        if (this.diploma?.id) {
            this.diploma = JSON.parse(JSON.stringify(this.originalModel));
            this.originalModel = null;
        } else {
            this.diploma = null;
        }

        this.isEditMode = false;
        this.currentPersonContextService.setIsInEdit(false);
    }

    invalid() {
        return this.invalidModal()
            .subscribe((diploma: PersonStudentDiploma) => {
                if (diploma) {
                    this.diploma = diploma;
                    this.updateDiplomaEvent.emit(diploma);
                }
            });
    }

    downloadFile() {
        return this.downloadFileModal()
            .subscribe((blob: Blob) => {
                if (blob) {
                    saveAs(blob, this.diploma.file.name);
                }
            });
    }

    private invalidModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personLot.modals.invalidDiploma';

            modal.result.then((ok) => {
                if (ok) {
                    this.resource
                        .invalid(this.diploma.id)
                        .pipe(
                            catchError(() => {
                                observer.next(null);
                                observer.complete();
                                return throwError(() => new Error('Student diploma invalid.'));
                            })
                        )
                        .subscribe((diploma: PersonStudentDiploma) => {
                            observer.next(diploma);
                            observer.complete();
                        });
                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }

    private downloadFileModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.warning = 'rdpzsd.personLot.modals.warning';
            modal.componentInstance.warningText = 'rdpzsd.personLot.modals.personalData';
            modal.componentInstance.text = 'rdpzsd.personLot.modals.exportDiplomaFile';
            modal.componentInstance.acceptButton = 'root.buttons.yesSure';
            modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

            modal.result.then((ok) => {
                if (ok) {
                    const secondModal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
                    secondModal.componentInstance.warning = 'rdpzsd.personLot.modals.warning';
                    secondModal.componentInstance.text = 'rdpzsd.personLot.modals.downloadingData';
                    secondModal.componentInstance.acceptButton = 'root.buttons.yesGenerate';
                    secondModal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

                    secondModal.result.then((ok) => {
                        if (ok) {
                            const fileUrl = `${this.configuration.restUrl}FileStorage?key=${this.diploma.file.key}&fileName=${this.diploma.file.name}&dbId=${this.diploma.file.dbId}`;

                            this.fileUploadService
                                .getFile(fileUrl, this.diploma.file.mimeType)
                                .pipe(
                                    catchError(() => {
                                        observer.next(null);
                                        observer.complete();
                                        return throwError(() => new Error('Student diploma download.'));
                                    })
                                )
                                .subscribe((blob: Blob) => {
                                    observer.next(blob);
                                    observer.complete();
                                });
                        } else {
                            observer.next(null);
                            observer.complete();
                        }
                    });
                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }
}