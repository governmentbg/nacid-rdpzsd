import { Component, OnInit } from '@angular/core';
import { NomenclatureCodeFilterDto } from 'src/nomenclatures/dtos/nomenclature-code-filter.dto';
import { District } from 'src/nomenclatures/models/settlement/district.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
  selector: 'district-search',
  templateUrl: './district-search.component.html',
  providers: [BaseNomenclatureCodeResource]
})
export class DistrictSearchComponent extends BaseNomenclatureComponent<District, NomenclatureCodeFilterDto> implements OnInit {

  constructor(
    baseNomenclatureResource: BaseNomenclatureCodeResource<District, NomenclatureCodeFilterDto>
  ) {
    super(baseNomenclatureResource, NomenclatureCodeFilterDto, 'District')
  }

  ngOnInit() {
    return this.getData();
  }
}
