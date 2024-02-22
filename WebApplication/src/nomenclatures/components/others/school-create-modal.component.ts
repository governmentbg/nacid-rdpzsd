import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { SchoolFilterDto } from 'src/nomenclatures/dtos/others/school-filter.dto';
import { SchoolOwnershipType } from 'src/nomenclatures/enums/school/school-ownership-type.enum';
import { SchoolState } from 'src/nomenclatures/enums/school/school-state.enum';
import { SchoolType } from 'src/nomenclatures/enums/school/school-type.enum';
import { School } from 'src/nomenclatures/models/others/school.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { SearchResultDto } from 'src/shared/dtos/search-result.dto';
import { SettlementChangeService } from 'src/shared/services/settlements/settlement-change.service';

@Component({
  selector: 'school-create-modal',
  templateUrl: './school-create-modal.component.html',
  providers: [BaseNomenclatureResource]
})
export class SchoolCreateModalComponent {

  school: School

  schoolType = SchoolType;
  schoolOwnershipType = SchoolOwnershipType;
  schoolState = SchoolState;

  searchResult: SearchResultDto<School>;

  constructor(
    public translateService: TranslateService,
    private baseNomenclatureResource: BaseNomenclatureResource<School, SchoolFilterDto>,
    private activeModal: NgbActiveModal,
    public settlementChangeService: SettlementChangeService,
  ) { }

  changeSchoolState(state: SchoolState){
    if(state !== this.schoolState.renamed){
      this.school.parentId = null;
      this.school.parent = null;
    }
  }

  save(item: School) {
    item.originalItem = null;
    
    return this.baseNomenclatureResource.create('School', item)
        .subscribe(result => {
          this.searchResult.result[this.searchResult.result.indexOf(item, 0)] = result;
          this.activeModal.close();
      });
  }

  cancel() {
    this.activeModal.close();
  }
}
