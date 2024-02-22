import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { StickerErrorDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker-error.dto";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { Semester } from "src/shared/enums/semester.enum";

@Component({
    selector: 'person-student-sticker-error-modal',
    templateUrl: './person-student-sticker-error.modal.html'
})
export class PersonStudentStickerErrorModalComponent {

    @Input() stickerErrorDto = new StickerErrorDto();

    courseType = CourseType;
    semesterType = Semester;

    constructor(
        public translateService: TranslateService,
        private activeModal: NgbActiveModal
    ) { }

    close() {
        this.activeModal.close();
    }
}