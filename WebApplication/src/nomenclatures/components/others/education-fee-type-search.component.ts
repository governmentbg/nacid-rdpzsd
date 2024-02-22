import { Component, OnInit } from '@angular/core';
import { EducationFeeTypeFilterDto } from 'src/nomenclatures/dtos/others/education-fee-type-filter.dto';
import { EducationFeeType } from 'src/nomenclatures/models/others/education-fee-type.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
  selector: 'education-fee-type-search',
  templateUrl: './education-fee-type-search.component.html',
  providers: [BaseNomenclatureResource]
})
export class EducationFeeTypeSearchComponent extends BaseNomenclatureComponent<EducationFeeType, EducationFeeTypeFilterDto> implements OnInit  {

  constructor(
    baseNomenclatureResource: BaseNomenclatureResource<EducationFeeType, EducationFeeTypeFilterDto>
  ) {
    super(baseNomenclatureResource, EducationFeeTypeFilterDto, 'EducationFeeType')
  }

  ngOnInit(){
    return this.getData();
  }
}
