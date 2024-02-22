import { Component, Input } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { StickerErrorDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker-error.dto";
import { PersonStudentStickerResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-sticker.resource";
import { PersonStudentStickerErrorModalComponent } from "./person-student-sticker-error.modal";

@Component({
    selector: 'person-student-sticker-validation-info',
    templateUrl: './person-student-sticker-validation-info.component.html'
})
export class PersonStudentStickerValidationInfoComponent {

    stickerErrorDto = new StickerErrorDto();

    partId: number;
    @Input('partId')
    set partIdSetter(partId: number) {
        this.partId = partId;
        this.validateStickerInfo();
    }

    constructor(
        private resource: PersonStudentStickerResource,
        private modalService: NgbModal
    ) {
    }

    openStickerErrorModal(event: any) {
        event.stopPropagation();
        const modal = this.modalService.open(PersonStudentStickerErrorModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.stickerErrorDto = this.stickerErrorDto;
    }

    validateStickerInfo() {
        this.resource.validatePersonStudentStickerInfo(this.partId)
            .subscribe(e => this.stickerErrorDto = e);
    }
}