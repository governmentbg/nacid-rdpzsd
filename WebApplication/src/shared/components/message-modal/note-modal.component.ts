import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NoteDto } from 'src/shared/dtos/note.dto';

@Component({
    templateUrl: './note-modal.component.html',
})
export class NoteModalComponent {

    @Input() noteDto: NoteDto = new NoteDto();
    @Input() readOnly: boolean = false;

    constructor(private activeModal: NgbActiveModal) {
    }

    send() {
        this.activeModal.close(true);
    }

    cancel() {
        this.activeModal.close(false);
    }
}
