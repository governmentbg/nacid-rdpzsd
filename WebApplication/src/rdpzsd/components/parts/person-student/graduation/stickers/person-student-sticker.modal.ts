import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { StickerDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker.dto";

@Component({
    selector: 'person-student-sticker-modal',
    templateUrl: './person-student-sticker.modal.html'
})
export class PersonStudentStickerModalComponent {

    stickerDto = new StickerDto();
    currentYear = (new Date()).getFullYear();

    @Input() title: string;
    @Input() showYear = true;
    @Input() requiredYear = false;
    @Input() requiredNote = false;

    constructor(
        private activeModal: NgbActiveModal
    ) {
        this.stickerDto.stickerYear = this.currentYear;
    }

    send() {
        this.activeModal.close(this.stickerDto);
    }

    close() {
        this.activeModal.close();
    }
}