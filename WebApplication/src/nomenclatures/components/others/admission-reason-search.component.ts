import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { Configuration } from 'src/app/configuration/configuration';
import { AdmissionReasonFilterDto } from 'src/nomenclatures/dtos/others/admission-reason-filter.dto';
import { AdmissionReasonStudentType } from 'src/nomenclatures/enums/admission-reason/admission-reason-student-type.enum';
import { CountryUnion } from 'src/nomenclatures/enums/country/country-union.enum';
import { AdmissionReasonEducationFee } from 'src/nomenclatures/models/others/admission-reason-education-fee.model';
import { AdmissionReason } from 'src/nomenclatures/models/others/admission-reason.model';
import { AdmissionReasonCitizenship } from 'src/nomenclatures/models/others/citizenship-country.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { MessageModalComponent } from 'src/shared/components/message-modal/message-modal.component';
import { UserDataService } from 'src/users/services/user-data.service';
import { BaseEditableNomenclatureComponent } from '../base/base-editable-nomenclature.component';
import { AdmissionReasonHistoryComponent } from './admission-reason-history.component';

@Component({
  selector: 'admission-reason-search',
  templateUrl: './admission-reason-search.component.html',
  providers: [BaseNomenclatureResource]
})
export class AdmissionReasonSearchComponent extends BaseEditableNomenclatureComponent<AdmissionReason, AdmissionReasonFilterDto> implements OnInit {

  countryUnion = CountryUnion;
  admissionReasonStudentType = AdmissionReasonStudentType;

  constructor(
    public userDataService: UserDataService,
    public translateService: TranslateService,
    public configuration: Configuration,
    modalService: NgbModal,
    baseNomenclatureResource: BaseNomenclatureResource<AdmissionReason, AdmissionReasonFilterDto>
  ) {
    super(baseNomenclatureResource, AdmissionReasonFilterDto, 'AdmissionReason', modalService, AdmissionReason)
  }

  ngOnInit() {
    return this.getData();
  }

  addCountry(index: number) {
    const admissionReasonCitizenship = new AdmissionReasonCitizenship()
    this.searchResult.result[index].admissionReasonCitizenships.push(admissionReasonCitizenship);
  }

  removeCountry(indexAdmissionReason: number, indexAdmissionReasonCitizenship: number) {
    this.searchResult.result[indexAdmissionReason].admissionReasonCitizenships.splice(indexAdmissionReasonCitizenship, 1);
  }

  addAdmissionReasonEducationFee(index: number) {
    const newAdmissionReasonEducationFee = new AdmissionReasonEducationFee();
    this.searchResult.result[index].admissionReasonEducationFees.push(newAdmissionReasonEducationFee);
  }

  removeAdmissionReasonEducationFee(indexAdmissionReason: number, indexEducationFeeType: number) {
    if (this.searchResult.result[indexAdmissionReason].admissionReasonEducationFees[indexEducationFeeType].educationFeeTypeId) {
      const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
      modal.componentInstance.text = 'rdpzsd.personLot.modals.deleteNomenclature';
      modal.componentInstance.acceptButton = 'root.buttons.delete';
      modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

      modal.result.then((ok) => {
        if (ok) {
          return this.searchResult.result[indexAdmissionReason].admissionReasonEducationFees.splice(indexEducationFeeType, 1);
        }

        return null;
      });
    }
    else {
      this.searchResult.result[indexAdmissionReason].admissionReasonEducationFees.splice(indexEducationFeeType, 1);
    }
  }

  validateEducationFeeType(id: number, indexAdmissionReason: number, indexEducationFeeType: number) {
    const hasDuplicate = this.searchResult.result[indexAdmissionReason].admissionReasonEducationFees
      .find(e => e.educationFeeTypeId == id);

    if (hasDuplicate) {
      this.searchResult.result[indexAdmissionReason].admissionReasonEducationFees[indexEducationFeeType].educationFeeType = null;
    } else {
      this.searchResult.result[indexAdmissionReason].admissionReasonEducationFees[indexEducationFeeType].educationFeeTypeId = id;
    }
  }

  openAdmissionReasonHistory(index: number) {
    const modal = this.modalService.open(AdmissionReasonHistoryComponent, { backdrop: 'static', size: 'xl' });
    modal.componentInstance.admissionReasonHistories = this.searchResult.result[index].admissionReasonHistories;
  }

  edit(item: AdmissionReason) {
    super.edit(item);
    if (!item.admissionReasonEducationFees.length) {
      item.admissionReasonEducationFees.push(new AdmissionReasonEducationFee());
    }
    item.originalItem.admissionReasonEducationFees = [...item.admissionReasonEducationFees];
  }
}
