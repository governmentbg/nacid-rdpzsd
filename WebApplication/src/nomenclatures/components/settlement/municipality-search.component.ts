import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MunicipalityFilterDto } from 'src/nomenclatures/dtos/settlements/municipality-filter.dto';
import { Municipality } from 'src/nomenclatures/models/settlement/municipality.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
  selector: 'municipality-search',
  templateUrl: './municipality-search.component.html',
  providers: [BaseNomenclatureCodeResource]
})
export class MunicipalitySearchComponent extends BaseNomenclatureComponent<Municipality, MunicipalityFilterDto> implements OnInit {

  constructor(
    baseNomenclatureResource: BaseNomenclatureCodeResource<Municipality, MunicipalityFilterDto>,
    public translateService: TranslateService
  ) {
    super(baseNomenclatureResource, MunicipalityFilterDto, 'Municipality')
  }

  ngOnInit() {
    return this.getData();
  }
}
