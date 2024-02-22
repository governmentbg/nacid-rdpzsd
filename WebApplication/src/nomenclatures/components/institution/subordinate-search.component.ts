import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { InstitutionFilterDto } from 'src/nomenclatures/dtos/institution/institution-filter.dto';
import { OrganizationType } from 'src/nomenclatures/enums/institution/organization-type.enum';
import { OwnershipType } from 'src/nomenclatures/enums/institution/ownership-type.enum';
import { Institution } from 'src/nomenclatures/models/institution/institution.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { Level } from 'src/shared/enums/level.enum';
import { SettlementChangeService } from 'src/shared/services/settlements/settlement-change.service';
import { UserDataService } from 'src/users/services/user-data.service';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
  selector: 'subordinate-search',
  templateUrl: './subordinate-search.component.html',
  providers: [BaseNomenclatureCodeResource]
})
export class SubordinateSearchComponent extends BaseNomenclatureComponent<Institution, InstitutionFilterDto> implements OnInit {

  organizationType = OrganizationType;
  ownershipType = OwnershipType;
  level = Level;

  constructor(
    baseNomenclatureResource: BaseNomenclatureCodeResource<Institution, InstitutionFilterDto>,
    public translateService: TranslateService,
    public settlementChangeService: SettlementChangeService,
    public userDataService: UserDataService
  ) {
    super(baseNomenclatureResource, InstitutionFilterDto, 'Institution')
  }

  clear() {
    this.filter = this.initializeFilter(this.structuredFilterType);
    this.filter.level = Level.second;
    return this.getData(false);
  }

  ngOnInit() {
    this.filter.level = Level.second;
    return this.getData();
  }
}
