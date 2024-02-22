import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PartHistoryDto } from 'src/rdpzsd/dtos/parts/part-history.dto';
import { PersonSecondaryHistory } from 'src/rdpzsd/models/parts/person-secondary/history/person-secondary-history.model';
import { PersonSecondary } from 'src/rdpzsd/models/parts/person-secondary/person-secondary.model';

@Component({
  selector: 'person-secondary-history',
  templateUrl: './person-secondary-history.component.html',
})
export class PersonSecondaryHistoryComponent {
  
  @Input() personSecondaryHistoryDto = new PartHistoryDto<PersonSecondary, PersonSecondaryHistory>();

  constructor(
      private activeModal: NgbActiveModal
  ) {
  }

  close() {
      this.activeModal.close();
  }
}
