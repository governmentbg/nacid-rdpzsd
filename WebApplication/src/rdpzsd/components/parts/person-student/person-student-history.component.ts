import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PartHistoryDto } from 'src/rdpzsd/dtos/parts/part-history.dto';
import { PersonStudentHistory } from 'src/rdpzsd/models/parts/person-student/history/person-student-history.model';
import { PersonStudent } from 'src/rdpzsd/models/parts/person-student/person-student.model';

@Component({
    selector: 'person-student-history',
    templateUrl: './person-student-history.component.html'
})
export class PersonStudentHistoryComponent {

    @Input() personStudentHistoryDto = new PartHistoryDto<PersonStudent, PersonStudentHistory>();

    constructor(
        private activeModal: NgbActiveModal
    ) {
    }

    close() {
        this.activeModal.close();
    }
}
