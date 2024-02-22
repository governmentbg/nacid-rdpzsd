import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SettlementFilterDto } from 'src/nomenclatures/dtos/settlements/settlement-filter.dto';
import { Settlement } from 'src/nomenclatures/models/settlement/settlement.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { SettlementChangeService } from 'src/shared/services/settlements/settlement-change.service';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
  selector: 'settlement-search',
  templateUrl: './settlement-search.component.html',
  providers: [BaseNomenclatureCodeResource]
})
export class SettlementSearchComponent extends BaseNomenclatureComponent<Settlement, SettlementFilterDto> implements OnInit {

  constructor(
    baseNomenclatureResource: BaseNomenclatureCodeResource<Settlement, SettlementFilterDto>,
    public translateService: TranslateService,
    public settlementChangeService: SettlementChangeService
  ) {
    super(baseNomenclatureResource, SettlementFilterDto, 'Settlement')
  }

  ngOnInit() {
    return this.getData();
  }
}
