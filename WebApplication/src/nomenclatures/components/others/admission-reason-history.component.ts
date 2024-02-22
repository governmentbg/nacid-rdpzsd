import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AdmissionReasonHistory } from 'src/nomenclatures/models/others/admission-reason-history.model';

@Component({
  selector: 'admission-reason-history',
  templateUrl: './admission-reason-history.component.html',
})
export class AdmissionReasonHistoryComponent {

  @Input() admissionReasonHistories: AdmissionReasonHistory[] = [];

  constructor(
    private activeModal: NgbActiveModal
  ) { }

  close(){
    this.activeModal.close();
  }

}
