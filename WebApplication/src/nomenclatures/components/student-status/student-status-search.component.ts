import { Component, OnInit } from '@angular/core';
import { NomenclatureFilterDto } from 'src/nomenclatures/dtos/nomenclature-filter.dto';
import { StudentStatus } from 'src/nomenclatures/models/student-status/student-status.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
    selector: 'student-status-search',
    templateUrl: './student-status-search.component.html',
    providers: [BaseNomenclatureResource]
})
export class StudentStatusSearchComponent extends BaseNomenclatureComponent<StudentStatus, NomenclatureFilterDto> implements OnInit {

    constructor(
        baseNomenclatureResource: BaseNomenclatureResource<StudentStatus, NomenclatureFilterDto>
    ) {
        super(baseNomenclatureResource, NomenclatureFilterDto, 'StudentStatus')
    }

    ngOnInit() {
        return this.getData();
    }
}
