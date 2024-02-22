import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PartHistoryDto } from 'src/rdpzsd/dtos/parts/part-history.dto';
import { PersonBasicHistory } from 'src/rdpzsd/models/parts/person-basic/history/person-basic-history.model';
import { PersonBasic } from 'src/rdpzsd/models/parts/person-basic/person-basic.model';

@Component({
    selector: 'person-basic-history',
    templateUrl: './person-basic-history.component.html'
})
export class PersonBasicHistoryComponent {

    @Input() personBasicHistoryDto = new PartHistoryDto<PersonBasic, PersonBasicHistory>();
    @Input() uan: string;

    constructor(
        private activeModal: NgbActiveModal
    ) {
    }

    close() {
        this.activeModal.close();
    }
}
