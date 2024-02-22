import { Component, EventEmitter, Input, Output } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { StudentEventGraduatedWithoutDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatusProcessGraduation } from "src/nomenclatures/models/student-status/student-status.model";
import { StickerDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker.dto";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentStickerResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-sticker.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { UserDataService } from "src/users/services/user-data.service";
import { PersonStudentStickerModalComponent } from "./person-student-sticker.modal";

@Component({
    selector: 'person-student-sticker-actions',
    templateUrl: './person-student-sticker-actions.component.html'
})
export class PersonStudentStickerActionsComponent {

    @Input() partId: number;
    @Input() currentStickerState: StudentStickerState;
    @Input() studentStatusAlias: string;
    @Input() studentEventAlias: string;
    @Input() institutionId: number;

    @Output() currentStickerStateChange: EventEmitter<StudentStickerState> = new EventEmitter<StudentStickerState>();
    @Output() stickerYearChange: EventEmitter<number> = new EventEmitter<number>();

    studentStatusProcessGraduation = StudentStatusProcessGraduation;

    studentEventGraduatedWithoutDiploma = StudentEventGraduatedWithoutDiploma;
    studentStickerState = StudentStickerState;

    constructor(
        private modalService: NgbModal,
        private stickerResource: PersonStudentStickerResource,
        private router: Router,
        public userDataService: UserDataService,
        public currentPersonContextService: CurrentPersonContextService
    ) {
    }

    sendForSticker() {
        return this.sendForStickerModal()
            .subscribe(studentStickerState => {
                this.currentStickerState = studentStickerState;
                this.currentStickerStateChange.emit(this.currentStickerState);
                this.succesfullSendForSticker();
                this.router.navigate(['/rdpzsdStickers']);
            });
    }

    reissueSticker() {
        return this.reissueStickerModal()
            .subscribe(studentStickerState => {
                this.currentStickerState = studentStickerState;
                this.currentStickerStateChange.emit(this.currentStickerState);
                this.succesfullSendForSticker();
                this.router.navigate(['/rdpzsdStickers']);
            });
    }

    returnForEdit(partId: number) {
        return this.returnForEditModal(partId)
            .subscribe((studentStickerState: StudentStickerState) => {
                this.currentStickerState = studentStickerState;
                this.currentStickerStateChange.emit(this.currentStickerState);
                this.router.navigate(['/rdpzsdStickers']);
            });
    }

    private returnForEditModal(partId: number) {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(PersonStudentStickerModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.title = 'rdpzsd.personStudent.stickers.modalReturnForEditTitle';
            modal.componentInstance.requiredNote = true;
            modal.componentInstance.requiredYear = false;
            modal.componentInstance.showYear = false;

            modal.result.then((stickerDto: StickerDto) => {
                if (stickerDto) {
                    this.stickerResource
                        .returnForEdit(partId, stickerDto)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Return sticker for edit error'));
                            })
                        )
                        .subscribe(studentStickerState => {
                            observer.next(studentStickerState);
                            observer.complete();
                        });
                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }

    private forPrintModal(partId: number) {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmForPrintModal';
            modal.componentInstance.acceptButton = 'root.buttons.yesSure'

            modal.result.then((ok) => {
                if (ok) {
                    this.stickerResource
                        .forPrint(partId)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('For print sticker error'));
                            })
                        )
                        .subscribe(personStudent => {
                            observer.next(personStudent);
                            observer.complete();
                        });
                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }

    forPrint(partId: number) {
        return this.forPrintModal(partId)
            .subscribe((personStudent) => {
                this.currentStickerState = personStudent.studentStickerState;
                this.currentStickerStateChange.emit(this.currentStickerState);
                this.router.navigate(['/rdpzsdStickers']);
            });
    }

    private recievedModal(partId: number) {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmRecievedModal';
            modal.componentInstance.acceptButton = 'root.buttons.yesSure'

            modal.result.then((ok) => {
                if (ok) {
                    this.stickerResource
                        .recieved(partId)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Received sticker error'));
                            })
                        )
                        .subscribe(studentStickerState => {
                            observer.next(studentStickerState);
                            observer.complete();
                        });
                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }

    receive(partId: number) {
        return this.recievedModal(partId)
            .subscribe((studentStickerState: StudentStickerState) => {
                this.currentStickerState = studentStickerState;
                this.currentStickerStateChange.emit(this.currentStickerState);
                this.router.navigate(['/rdpzsdStickers']);
            });
    }

    private sendForStickerModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(PersonStudentStickerModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.title = 'rdpzsd.personStudent.stickers.modalSendForStickerTitle';
            modal.componentInstance.requiredYear = true;

            modal.result.then((stickerDto: StickerDto) => {
                if (stickerDto) {
                    this.stickerResource
                        .sendForSticker(this.partId, stickerDto)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Send for sticker error'));
                            })
                        )
                        .subscribe(studentStickerState => {
                            this.stickerYearChange.emit(stickerDto.stickerYear);
                            observer.next(studentStickerState);
                            observer.complete();
                        });
                } else {
                    observer.complete();
                }
            });
        });
    }

    private reissueStickerModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(PersonStudentStickerModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.title = 'rdpzsd.personStudent.stickers.modalReissueStickerTitle';
            modal.componentInstance.requiredYear = true;
            modal.componentInstance.requiredNote = true;

            modal.result.then((stickerDto: StickerDto) => {
                if (stickerDto) {
                    this.stickerResource
                        .reissueSticker(this.partId, stickerDto)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Reissue sticker error'));
                            })
                        )
                        .subscribe(studentStickerState => {
                            this.stickerYearChange.emit(stickerDto.stickerYear);
                            observer.next(studentStickerState);
                            observer.complete();
                        });
                } else {
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
}