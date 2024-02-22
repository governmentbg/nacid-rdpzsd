import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StudentEventFilterDto } from 'src/nomenclatures/dtos/student-status/student-event-filter.dto';
import { StudentEvent } from 'src/nomenclatures/models/student-status/student-event.model';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';

@Component({
    selector: 'student-event-search',
    templateUrl: './student-event-search.component.html',
    providers: [BaseNomenclatureResource]
})
export class StudentEventSearchComponent extends BaseNomenclatureComponent<StudentEvent, StudentEventFilterDto> implements OnInit {

    constructor(
        baseNomenclatureResource: BaseNomenclatureResource<StudentEvent, StudentEventFilterDto>,
        public translateService: TranslateService
    ) {
        super(baseNomenclatureResource, StudentEventFilterDto, 'StudentEvent')
    }

    ngOnInit() {
        return this.getData();
    }
}
