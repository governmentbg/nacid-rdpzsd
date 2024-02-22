import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PersonLotResource } from "src/rdpzsd/resources/person-lot.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { NoteModalComponent } from "src/shared/components/message-modal/note-modal.component";
import { NoteDto } from "src/shared/dtos/note.dto";
import { UserDataService } from "src/users/services/user-data.service";

@Component({
    selector: 'lot-actions',
    templateUrl: './lot-actions.component.html',
    styleUrls: ['./lot-actions.styles.css']
})
export class LotActionsComponent {

    @Input() personLotId: number;
    @Input() personLotState: LotState;
    @Input() personLotCreateUserId: number;

    lotState = LotState;

    constructor(
        private modalService: NgbModal,
        private personLotResource: PersonLotResource,
        private router: Router,
        public currentPersonContextService: CurrentPersonContextService,
        public userDataService: UserDataService
    ) {
    }

    cancelPendingApproval() {
        return this.cancelPendingApprovalModal()
            .subscribe((ok: boolean) => {
                if (ok) {
                    this.router.navigate(['/rdpzsdApproval']);
                }
            });
    }

    approve() {
        return this.approveModal()
            .subscribe((ok: boolean) => {
                if (ok) {
                    this.router.navigate(['/rdpzsdApproval']);
                }
            });
    }

    sendForApproval() {
        return this.personLotResource
            .sendForApproval(this.personLotId)
            .subscribe(() => {
                const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
                modal.componentInstance.text = 'rdpzsd.personLot.modals.pendingApproval';
                modal.componentInstance.declineButton = 'root.buttons.ok';
                modal.componentInstance.declineButtonClass = 'btn-sm btn-outline-primary';
                modal.componentInstance.infoOnly = true;

                this.router.navigate(['/rdpzsdNewSearch']);
            });
    }

    private cancelPendingApprovalModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(NoteModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            const lotNote = new NoteDto();
            modal.componentInstance.noteDto = lotNote;

            modal.result.then((ok) => {
                if (ok) {
                    this.personLotResource
                        .cancelPendingApproval(this.personLotId, lotNote)
                        .pipe(
                            catchError(() => {
                                observer.next(false);
                                observer.complete();
                                return throwError(() => new Error('Cancel pending approval error'));
                            })
                        )
                        .subscribe(() => {
                            observer.next(true);
                            observer.complete();
                        });
                } else {
                    observer.next(false);
                    observer.complete();
                }
            });
        });
    }

    private approveModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personLot.modals.approve';
            modal.componentInstance.acceptButton = 'root.buttons.yesSure'
            modal.componentInstance.declineButton = 'root.buttons.cancel';

            modal.result.then((ok) => {
                if (ok) {
                    this.personLotResource
                        .approve(this.personLotId)
                        .pipe(
                            catchError(() => {
                                observer.next(false);
                                observer.complete();
                                return throwError(() => new Error('Approve error'));
                            })
                        )
                        .subscribe(() => {
                            observer.next(true);
                            observer.complete();
                        });
                } else {
                    observer.next(false);
                    observer.complete();
                }
            });
        });
    }
}