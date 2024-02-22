import { Component, EventEmitter, Input, Output } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Configuration } from "src/app/configuration/configuration";
import { StudentStatusProcessGraduation } from "src/nomenclatures/models/student-status/student-status.model";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { StudentProtocolType } from "src/rdpzsd/enums/parts/student-protocol-type.enum";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentProtocol } from "src/rdpzsd/models/parts/person-student/person-student-protocol.model";
import { PersonStudentProtocolResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-protocol.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { UserDataService } from "src/users/services/user-data.service";

@Component({
    selector: 'person-student-protocol',
    templateUrl: './person-student-protocol.component.html',
    providers: [PersonStudentProtocolResource]
})
export class PersonStudentProtocolComponent {

    studentStatusProcessGraduation = StudentStatusProcessGraduation;

    protocols: PersonStudentProtocol[];
    @Input('protocols')
    set protocolsSetter(protocols: PersonStudentProtocol[]) {
        this.protocols = protocols;
    }

    @Input() personLotState: LotState;
    @Input() partState: PartState;
    @Input() institutionId: number;
    @Input() stickerState: StudentStickerState;
    @Input() studentStatusAlias: string;

    @Output() addProtocolEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() removeProtocolEvent: EventEmitter<number> = new EventEmitter<number>();

    studentStickerState = StudentStickerState;
    partStateType = PartState;
    lotState = LotState;
    studentProtocolType = StudentProtocolType;

    constructor(
        public userDataService: UserDataService,
        public currentPersonContextService: CurrentPersonContextService,
        private resource: PersonStudentProtocolResource,
        private modalService: NgbModal,
        public configuration: Configuration
    ) {
    }

    edit(index: number) {
        this.protocols[index].originalModel = JSON.parse(JSON.stringify(this.protocols[index]));
        this.protocols[index].isEditMode = true;
        this.currentPersonContextService.setIsInEdit(true);
    }

    save(index: number) {
        if (this.protocols[index].id) {
            return this.resource
                .update(this.protocols[index])
                .subscribe(updatedProtocol => {
                    this.protocols[index] = updatedProtocol;
                    this.protocols[index].originalModel = null;
                    this.protocols[index].isEditMode = false;
                    this.currentPersonContextService.setIsInEdit(false);
                });
        } else {
            return this.resource
                .create(this.protocols[index])
                .subscribe(newProtocol => {
                    this.protocols[index] = newProtocol;
                    this.protocols[index].originalModel = null;
                    this.protocols[index].isEditMode = false;
                    this.currentPersonContextService.setIsInEdit(false);
                });
        }
    }

    delete(index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personLot.modals.deleteProtocol';
        modal.componentInstance.acceptButton = 'root.buttons.delete';

        modal.result.then((ok) => {
            if (ok) {
                return this.resource
                    .delete(this.protocols[index].id, this.protocols[index].partId)
                    .subscribe(() => {
                        this.removeProtocolEvent.emit(index);
                        this.currentPersonContextService.setIsInEdit(false);
                    });
            } else {
                return null;
            }
        });
    }

    cancel(index: number) {
        if (this.protocols[index].id) {
            this.protocols[index] = JSON.parse(JSON.stringify(this.protocols[index].originalModel));
            this.protocols[index].originalModel = null;
            this.protocols[index].isEditMode = false;
        } else {
            this.removeProtocolEvent.emit(index);
        }

        this.currentPersonContextService.setIsInEdit(false);
    }

    addProtocol() {
        this.currentPersonContextService.setIsInEdit(true);
        this.addProtocolEvent.emit();
    }
}