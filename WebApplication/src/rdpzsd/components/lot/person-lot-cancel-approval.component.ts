import { Component, Input } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { PersonLotActionType } from "src/rdpzsd/enums/person-lot-action-type.enum";
import { PersonLotAction } from "src/rdpzsd/models/person-lot-action.model";
import { PersonLotResource } from "src/rdpzsd/resources/person-lot.resource";
import { NoteModalComponent } from "src/shared/components/message-modal/note-modal.component";
import { NoteDto } from "src/shared/dtos/note.dto";

@Component({
    selector: 'person-lot-cancel-approval',
    templateUrl: './person-lot-cancel-approval.component.html'
})
export class PersonLotCancelApprovalComponent {

    personLotAction: PersonLotAction = null;

    personLotId: number;
    @Input('personLotId')
    set personLotIdSetter(personLotId: number) {
        if (personLotId) {
            this.personLotId = personLotId;
            this.resource.getLatestPersonLotActionByType(personLotId, this.personLotActionType.cancelApproval)
                .subscribe(e => {
                    this.personLotAction = e;
                });
        } else {
            this.personLotId = null;
        }
    }

    personLotActionType = PersonLotActionType;

    constructor(
        private modalService: NgbModal,
        private resource: PersonLotResource
    ) {
    }

    showLotNote() {
        const noteDto = new NoteDto();
        noteDto.date = this.personLotAction.actionDate;
        noteDto.note = this.personLotAction.note;
        noteDto.userFullname = this.personLotAction.userFullname;
        const modal = this.modalService.open(NoteModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.readOnly = true;
        modal.componentInstance.noteDto = noteDto;
    }
}