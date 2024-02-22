import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, Output } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { PartHistoryDto } from "src/rdpzsd/dtos/parts/part-history.dto";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonBasicHistory } from "src/rdpzsd/models/parts/person-basic/history/person-basic-history.model";
import { PersonBasicInfo } from "src/rdpzsd/models/parts/person-basic/person-basic-info.model";
import { PersonBasic } from "src/rdpzsd/models/parts/person-basic/person-basic.model";
import { PartResource } from "src/rdpzsd/resources/parts/part.resource";
import { PersonLotResource } from "src/rdpzsd/resources/person-lot.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { UserDataService } from "src/users/services/user-data.service";
import { PersonBasicHistoryComponent } from "./person-basic-history.component";

@Component({
    selector: 'person-basic-part',
    templateUrl: './person-basic-part.component.html',
    providers: [
        PartResource
    ],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class PersonBasicPartComponent {

    entity: PersonBasic = new PersonBasic();
    loadingData = false;
    partState = PartState;
    lotState = LotState;
    stickerState = StudentStickerState;

    @Input() personLotState: LotState;
    @Output() personLotStateChange: EventEmitter<LotState> = new EventEmitter<LotState>();
    @Input() personLotCreateUserId: number;

    personLotId: number;
    @Input('personLotId')
    set personLotIdSetter(personLotId: number) {
        if (personLotId) {
            this.loadingData = true;
            this.personLotId = personLotId;
            this.singlePartResource
                .getSinglePart(personLotId)
                .subscribe(e => {
                    this.entity = e;
                    this.currentPersonContextService.setFromPersonBasic(e, this.personLotState);
                    this.loadingData = false;
                });
        }
    }

    isValidPersonBasicForm = false;
    isEditMode = false;

    constructor(
        public currentPersonContextService: CurrentPersonContextService,
        private singlePartResource: PartResource<PersonBasic, PersonBasicInfo, PersonBasicHistory, FilterDto>,
        private modalService: NgbModal,
        private router: Router,
        private cdr: ChangeDetectorRef,
        private personLotResource: PersonLotResource,
        public userDataService: UserDataService,
        public configuration: Configuration
    ) {
        singlePartResource.init('PersonBasic');
    }

    modelChange(entity: PersonBasic) {
        this.entity = entity;

        if (this.personLotState === this.lotState.erased) {
            this.personLotResource.getLotState(this.personLotId)
                .subscribe(lotState => {
                    this.currentPersonContextService.setFromPersonBasic(entity, lotState);
                    this.personLotStateChange.emit(lotState);
                });
        } else {
            this.currentPersonContextService.setFromPersonBasic(entity, this.personLotState);
        }
    }

    showHistory(partHistoryDto: PartHistoryDto<PersonBasic, PersonBasicHistory>) {
        const modal = this.modalService.open(PersonBasicHistoryComponent, { backdrop: 'static', size: 'xl' });
        modal.componentInstance.personBasicHistoryDto = partHistoryDto;
        modal.componentInstance.uan = this.currentPersonContextService.uan;
    }

    eraseLot() {
        return this.eraseLotModal()
            .subscribe((ok: boolean) => {
                if (ok) {
                    this.router.navigate(['/rdpzsdNewSearch']);
                }
            });
    }

    ngAfterViewChecked() {
        this.cdr.detectChanges();
    }

    private eraseLotModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.warning = 'rdpzsd.personLot.modals.eraseLot';
            modal.componentInstance.text = 'rdpzsd.personLot.modals.eraseLotConfirm';
            modal.componentInstance.acceptButton = 'root.buttons.delete';
            modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

            modal.result.then((ok) => {
                if (ok) {
                    this.personLotResource
                        .erase(this.personLotId)
                        .pipe(
                            catchError(() => {
                                observer.next(false);
                                observer.complete();
                                return throwError(() => new Error('Erase lot error'));
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