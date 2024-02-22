import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { SchoolFilterDto } from 'src/nomenclatures/dtos/others/school-filter.dto';
import { SchoolOwnershipType } from 'src/nomenclatures/enums/school/school-ownership-type.enum';
import { SchoolState } from 'src/nomenclatures/enums/school/school-state.enum';
import { SchoolType } from 'src/nomenclatures/enums/school/school-type.enum';
import { School } from 'src/nomenclatures/models/others/school.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { SettlementChangeService } from 'src/shared/services/settlements/settlement-change.service';
import { UserDataService } from 'src/users/services/user-data.service';
import { BaseEditableNomenclatureComponent } from '../base/base-editable-nomenclature.component';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';
import { SchoolCreateModalComponent } from './school-create-modal.component';

@Component({
    selector: 'school-search',
    templateUrl: './school-search.component.html',
    providers: [BaseNomenclatureResource]
})
export class SchoolSearchComponent extends BaseEditableNomenclatureComponent<School, SchoolFilterDto> implements OnInit {

    schoolType = SchoolType;
    schoolOwnershipType = SchoolOwnershipType;
    schoolState = SchoolState;

    constructor(
        baseNomenclatureResource: BaseNomenclatureResource<School, SchoolFilterDto>,
        modalService: NgbModal,
        public translateService: TranslateService,
        public settlementChangeService: SettlementChangeService,
        public userDataService: UserDataService
    ) {
        super(baseNomenclatureResource, SchoolFilterDto, 'School', modalService, School)
    }

    add() {
        let newItem = new this.nomenclature();
        newItem.isActive = true;
        newItem.isEditMode = true;

        const modal = this.modalService.open(SchoolCreateModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.school = newItem;
        modal.componentInstance.searchResult = this.searchResult;
    }

    ngOnInit() {
        return this.getData();
    }
}
