import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PartState } from 'src/rdpzsd/enums/part-state.enum';
import { PartInfo } from 'src/rdpzsd/models/parts/base/part-info.model';
import { BasePersonSecondary } from 'src/rdpzsd/models/parts/person-secondary/base/base-person-secondary.model';
import { PersonSecondaryRecognitionDocumentHistory } from './../../../models/parts/person-secondary/history/person-secondary-recognition-document-history.model';

@Component({
  selector: 'person-secondary-history-data',
  templateUrl: './person-secondary-history-data.component.html',
})
export class PersonSecondaryHistoryDataComponent<TPart extends BasePersonSecondary<TPartInfo, PersonSecondaryRecognitionDocumentHistory>, TPartInfo extends PartInfo> {

  @Input() personSecondary: TPart;
  @Input() isCollapsed = true;

  partState = PartState;

  constructor(
      private activeModal: NgbActiveModal
  ) {
  }

  close() {
      this.activeModal.close();
  }
}
