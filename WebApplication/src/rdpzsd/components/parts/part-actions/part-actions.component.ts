import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { PartHistoryDto } from "src/rdpzsd/dtos/parts/part-history.dto";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { PartInfo } from "src/rdpzsd/models/parts/base/part-info.model";
import { Part } from "src/rdpzsd/models/parts/base/part.model";
import { PartResource } from "src/rdpzsd/resources/parts/part.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { Injector } from '@angular/core'
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { catchError, Observable, Observer, throwError } from "rxjs";

@Component({
    selector: 'part-actions',
    templateUrl: './part-actions.component.html',
    providers: [
        PartResource
    ]
})
export class PartActionsComponent<TEntity extends Part<TPartInfo>, TPartInfo extends PartInfo, THistory, TFilter extends FilterDto> implements OnInit {

    restUrl: string;
    @Input('restUrl')
    set restUrlSetter(restUrl: string) {
        if (restUrl) {
            this.restUrl = restUrl;
            this.partResource.init(this.restUrl);
        }
    }

    @Input() model: TEntity;
    @Input() isEditMode = false;
    @Input() invalidForm = false;
    @Input() fromModal = false;
    @Input() lotId: number;
    @Input() hideEditActions = false;
    @Input() enableErase = false;

    @Output() modelChange: EventEmitter<TEntity> = new EventEmitter<TEntity>();
    @Output() isEditModeChange: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() showHistoryEvent: EventEmitter<PartHistoryDto<TEntity, THistory>> = new EventEmitter<PartHistoryDto<TEntity, THistory>>();

    originalModel: TEntity = null;
    partState = PartState;

    partHistoryDto: PartHistoryDto<TEntity, THistory> = new PartHistoryDto();

    currentPersonContextService: CurrentPersonContextService = null;

    constructor(
        private injector: Injector,
        private partResource: PartResource<TEntity, TPartInfo, THistory, TFilter>,
        private activeModal: NgbActiveModal,
        private modalService: NgbModal,
    ) {
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.model)) as TEntity;
        this.changeIsEditMode(true);
    }

    save() {
        if (this.model.id) {
            return this.partResource
                .update(this.model)
                .subscribe(e => {
                    if (!this.fromModal) {
                        this.changeModel(e);
                        this.originalModel = null;
                        this.changeIsEditMode(false);
                        this.getHistory();
                    } else {
                        this.activeModal.close(e);
                    }
                })
        } else {
            this.model.id = this.lotId;

            if(this.restUrl === 'PersonDoctoral'){
                const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
                modal.componentInstance.text = 'rdpzsd.personLot.modals.validateProtocolDate';
                modal.componentInstance.acceptButton = 'root.buttons.confirmAndSave';
                modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';
              
                return modal.result.then((ok) => {
                    if (ok) {
                        return this.partResource
                        .create(this.model)
                        .subscribe(e => {
                            if (!this.fromModal) {
                                this.changeModel(e);
                                this.originalModel = null;
                                this.changeIsEditMode(false);
                                this.getHistory();
                            } else {
                                this.activeModal.close(e);
                            }
                        });
                    }
              
                    return null;
                });
            }
                
            return this.partResource
                    .create(this.model)
                    .subscribe(e => {
                        if (!this.fromModal) {
                            this.changeModel(e);
                            this.originalModel = null;
                            this.changeIsEditMode(false);
                            this.getHistory();
                        } else {
                            this.activeModal.close(e);
                        }
                    });
        }
    }

    cancel() {
        this.changeModel(JSON.parse(JSON.stringify(this.originalModel)) as TEntity);
        this.originalModel = null;
        this.changeIsEditMode(false);
    }

    erase() {
        return this.approveEraseModal()
            .subscribe((ok: boolean) => {
                if (ok) {
                    this.changeModel(null);
                    this.originalModel = null;
                    this.changeIsEditMode(false);
                }
            });
    }

    private approveEraseModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personLot.modals.erasePart';
            modal.componentInstance.acceptButton = 'root.buttons.erase';
            modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

            modal.result.then((ok) => {
                if (ok) {
                    this.partResource
                        .erase(this.model.id)
                        .pipe(
                            catchError(() => {
                                observer.next(false);
                                observer.complete();
                                return throwError(() => new Error('Erase error'));
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

    showHistory() {
        this.showHistoryEvent.emit(this.partHistoryDto);
    }

    getHistory() {
        this.partResource
            .getHistory(this.model.id)
            .subscribe(e => {
                this.partHistoryDto = e;
            });
    }

    private changeIsEditMode(isEditMode: boolean) {
        this.isEditMode = isEditMode;
        if (!this.fromModal) {
            this.currentPersonContextService.setIsInEdit(isEditMode);
        }
        this.isEditModeChange.emit(this.isEditMode);
    }

    private changeModel(model: TEntity) {
        this.model = model;
        this.modelChange.emit(model);
    }

    ngOnInit() {
        if (!this.fromModal) {
            this.currentPersonContextService = this.injector.get<CurrentPersonContextService>(CurrentPersonContextService);
        }

        if (this.model?.id) {
            this.getHistory();
        }
    }
}