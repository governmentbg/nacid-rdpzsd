import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PartHistoryDto } from 'src/rdpzsd/dtos/parts/part-history.dto';
import { PersonDoctoralHistory } from 'src/rdpzsd/models/parts/person-doctoral/history/person-doctoral-history.model';
import { PersonDoctoral } from 'src/rdpzsd/models/parts/person-doctoral/person-doctoral.model';

@Component({
    selector: 'person-doctoral-history',
    templateUrl: './person-doctoral-history.component.html'
})
export class PersonDoctoralHistoryComponent {

    @Input() personDoctoralHistoryDto = new PartHistoryDto<PersonDoctoral, PersonDoctoralHistory>();

    constructor(
        private activeModal: NgbActiveModal
    ) {
    }

    close() {
        this.activeModal.close();
    }
}
