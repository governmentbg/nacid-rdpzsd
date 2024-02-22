import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { PersonStudentStickerNote } from "src/rdpzsd/models/parts/person-student/person-student-sticker-note.model";

@Component({
    selector: 'person-student-sticker-notes-modal',
    templateUrl: './person-student-sticker-notes.modal.html'
})
export class PersonStudentStickerNotesModalComponent {

    @Input() stickerNotes: PersonStudentStickerNote[] = [];

    constructor(
        private activeModal: NgbActiveModal
    ) {
    }

    sortNotes() {
        return this.stickerNotes.sort((a, b) => a.actionDate > b.actionDate ? -1 : 1);
    }

    close() {
        this.activeModal.close();
    }
}