import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NomenclatureFilterDto } from 'src/nomenclatures/dtos/nomenclature-filter.dto';
import { Period } from 'src/nomenclatures/models/others/period.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { Semester } from 'src/shared/enums/semester.enum';
import { UserDataService } from 'src/users/services/user-data.service';
import { BaseEditableNomenclatureComponent } from '../base/base-editable-nomenclature.component';

@Component({
    selector: 'period-search',
    templateUrl: './period-search.component.html',
    providers: [BaseNomenclatureResource]
})
export class PeriodSearchComponent extends BaseEditableNomenclatureComponent<Period, NomenclatureFilterDto> implements OnInit {

    semester = Semester;

    constructor(
        public userDataService: UserDataService,
        modalService: NgbModal,
        baseNomenclatureResource: BaseNomenclatureResource<Period, NomenclatureFilterDto>
    ) {
        super(baseNomenclatureResource, NomenclatureFilterDto, 'Period', modalService, Period)
    }

    add() {
        let newItem = new this.nomenclature();
        newItem.isActive = true;
        newItem.isEditMode = true;
        newItem.year = new Date().getFullYear();

        this.searchResult.result.unshift(newItem);
    }

    ngOnInit() {
        return this.getData();
    }
}
