import { Component, OnInit } from '@angular/core';
import { NomenclatureCodeFilterDto } from 'src/nomenclatures/dtos/nomenclature-code-filter.dto';
import { Country } from 'src/nomenclatures/models/settlement/country.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
  selector: 'country-search',
  templateUrl: './country-search.component.html',
  providers: [BaseNomenclatureCodeResource]
})
export class CountrySearchComponent extends BaseNomenclatureComponent<Country, NomenclatureCodeFilterDto> implements OnInit {

  constructor(
    baseNomenclatureResource: BaseNomenclatureCodeResource<Country, NomenclatureCodeFilterDto>
  ) {
    super(baseNomenclatureResource, NomenclatureCodeFilterDto, 'Country')
  }

  ngOnInit() {
    return this.getData();
  }
}
