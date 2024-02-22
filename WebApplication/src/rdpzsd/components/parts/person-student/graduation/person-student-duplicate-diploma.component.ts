import { Component, EventEmitter, Input, Output } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import * as saveAs from "file-saver";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { StudentEventGraduatedWithDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { PersonStudentDuplicateDiplomaCreateDto } from "src/rdpzsd/dtos/parts/person-student-duplicate-diploma/person-student-duplicate-diploma-create.dto";
import { StickerDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker.dto";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentDuplicateDiploma } from "src/rdpzsd/models/parts/person-student/person-student-duplicate-diploma.model";
import { PersonStudentDuplicateDiplomaResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-duplicate-diploma.resource";
import { PersonStudentStickerResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-sticker.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { FileUploadService } from "src/shared/services/file-upload.service";
import { UserDataService } from "src/users/services/user-data.service";
import { PersonStudentStickerModalComponent } from "./stickers/person-student-sticker.modal";

@Component({
    selector: 'person-student-duplicate-diploma',
    templateUrl: './person-student-duplicate-diploma.component.html',
    providers: [PersonStudentDuplicateDiplomaResource]
})
export class PersonStudentDuplicateDiplomaComponent {

    duplicateDiplomas: PersonStudentDuplicateDiploma[];
    @Input('duplicateDiplomas')
    set duplicateDiplomasSetter(duplicateDiplomas: PersonStudentDuplicateDiploma[]) {
        this.duplicateDiplomas = duplicateDiplomas;
    }

    @Input() partId: number;
    @Input() personLotState: LotState;
    @Input() partState: PartState;
    @Input() institutionId: number;
    @Input() studentEventAlias: string;

    studentStickerState = StudentStickerState;
    partStateType = PartState;
    lotState = LotState;
    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;
    currentYear = new Date().getFullYear();

    constructor(
        public userDataService: UserDataService,
        public currentPersonContextService: CurrentPersonContextService,
        private resource: PersonStudentDuplicateDiplomaResource,
        private modalService: NgbModal,
        private fileUploadService: FileUploadService,
        public configuration: Configuration,
        private router: Router,
        private stickerResource: PersonStudentStickerResource
    ) {
    }

    edit(index: number) {
        this.duplicateDiplomas[index].originalModel = JSON.parse(JSON.stringify(this.duplicateDiplomas[index]));
        this.duplicateDiplomas[index].isEditMode = true;
        this.currentPersonContextService.setIsInEdit(true);
    }

    save(index: number) {
        if (this.duplicateDiplomas[index].id) {
            return this.resource
                .update(this.duplicateDiplomas[index])
                .subscribe(updatedDuplicateDiploma => {
                    this.duplicateDiplomas[index] = updatedDuplicateDiploma;
                    this.duplicateDiplomas[index].originalModel = null;
                    this.duplicateDiplomas[index].isEditMode = false;
                    this.currentPersonContextService.setIsInEdit(false);
                });
        } else {
            return null;
        }
    }

    cancel(index: number) {
        if (this.duplicateDiplomas[index].id) {
            this.duplicateDiplomas[index] = JSON.parse(JSON.stringify(this.duplicateDiplomas[index].originalModel));
            this.duplicateDiplomas[index].originalModel = null;
            this.duplicateDiplomas[index].isEditMode = false;
        }

        this.currentPersonContextService.setIsInEdit(false);
    }

    sendForStickerDuplicate() {
        return this.sendForStickerDuplicateModal()
            .subscribe(() => {
                this.succesfullSendForSticker();
                this.router.navigate(['/rdpzsdStickers']);
            });
    }

    private sendForStickerDuplicateModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(PersonStudentStickerModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.title = 'rdpzsd.personStudent.stickers.modalSendDuplicateForStickerTitle';
            modal.componentInstance.requiredYear = true;
            modal.componentInstance.requiredNote = true;

            modal.result.then((stickerDto: StickerDto) => {
                if (stickerDto && stickerDto.stickerYear) {
                    const newDuplicateDiploma = new PersonStudentDuplicateDiploma();
                    newDuplicateDiploma.duplicateStickerYear = stickerDto.stickerYear;
                    newDuplicateDiploma.duplicateDiplomaDate = null;
                    newDuplicateDiploma.duplicateDiplomaNumber = null;
                    newDuplicateDiploma.duplicateRegistrationDiplomaNumber = null;
                    newDuplicateDiploma.duplicateStickerState = this.studentStickerState.sendForSticker;
                    newDuplicateDiploma.file = null;
                    newDuplicateDiploma.isValid = true;
                    newDuplicateDiploma.partId = this.partId;

                    const createDuplicateDiplomaDto = new PersonStudentDuplicateDiplomaCreateDto();
                    createDuplicateDiplomaDto.newDuplicateDiploma = newDuplicateDiploma;
                    createDuplicateDiplomaDto.stickerDto = stickerDto;

                    this.resource
                        .create(createDuplicateDiplomaDto)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Send for sticker error'));
                            })
                        )
                        .subscribe((duplicateDiploma: PersonStudentDuplicateDiploma) => {
                            observer.next(duplicateDiploma);
                            observer.complete();
                        });
                } else {
                    observer.complete();
                }
            });
        });
    }

    allDuplicatesAreInvalid() {
        return this.duplicateDiplomas.every(e => e.duplicateStickerState === this.studentStickerState.recieved && !e.isValid);
    }

    downloadFile(index: number) {
        return this.downloadFileModal(index)
            .subscribe((blob: Blob) => {
                if (blob) {
                    saveAs(blob, this.duplicateDiplomas[index].file.name);
                }
            });
    }

    invalid(index: number) {
        const diplomaDuplicate = this.duplicateDiplomas[index];

        return this.invalidModal(diplomaDuplicate)
            .subscribe((duplicateDiploma: PersonStudentDuplicateDiploma) => {
                if (duplicateDiploma) {
                    this.duplicateDiplomas[index] = duplicateDiploma;
                }
            });
    }

    private invalidModal(diplomaDuplicate: PersonStudentDuplicateDiploma) {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personLot.modals.invalidDiploma';

            modal.result.then((ok) => {
                if (ok) {
                    this.resource
                        .invalid(diplomaDuplicate.id)
                        .pipe(
                            catchError(() => {
                                observer.next(null);
                                observer.complete();
                                return throwError(() => new Error('Student duplicate diploma invalid.'));
                            })
                        )
                        .subscribe((duplicateDiploma: PersonStudentDuplicateDiploma) => {
                            observer.next(duplicateDiploma);
                            observer.complete();
                        });
                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }

    private downloadFileModal(index: number) {
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
                            const duplicateDiploma = this.duplicateDiplomas[index];

                            const fileUrl = `${this.configuration.restUrl}FileStorage?key=${duplicateDiploma.file.key}&fileName=${duplicateDiploma.file.name}&dbId=${duplicateDiploma.file.dbId}`;

                            this.fileUploadService
                                .getFile(fileUrl, duplicateDiploma.file.mimeType)
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

    private succesfullSendForSticker() {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.modalSuccessfullSendForStickerTitle';
        modal.componentInstance.declineButton = 'root.buttons.ok';
        modal.componentInstance.declineButtonClass = 'btn-sm btn-primary';
        modal.componentInstance.infoOnly = true;
    }

    forPrintDuplicateModal(duplicateDiplomaId: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmForPrintModal';

        modal.result.then((ok) => {
            if (ok) {
                return this.stickerResource
                    .forPrintDuplicate(duplicateDiplomaId)
                    .subscribe(() => {
                        this.router.navigate(['/rdpzsdStickers']);
                    });
            } else {
                return null;
            }
        });
    }

    recievedDuplicateModal(duplicateDiplomaId: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmRecievedModal';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure'

        modal.result.then((ok) => {
            if (ok) {
                return this.stickerResource
                    .recievedDuplicate(duplicateDiplomaId)
                    .subscribe(() => {
                        this.router.navigate(['/rdpzsdStickers']);
                    });
            } else {
                return null;
            }
        });
    }
}