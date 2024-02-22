import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PeriodNomenclatureResource } from 'src/nomenclatures/resources/period-nomenclature.resource';
import { PersonDoctoralSearchFilterDto } from 'src/rdpzsd/dtos/search/person-student-doctoral/person-doctoral-search-filter.dto';
import { YearType } from 'src/rdpzsd/enums/parts/year-type.enum';
import { PersonStudentDoctoralSearchResource } from 'src/rdpzsd/resources/search/person-student-doctoral-search.resource';
import { UserDataService } from 'src/users/services/user-data.service';
import { BaseRdpzsdSearchComponent } from './base/base-rdpzsd-search.component';

@Component({
    selector: 'rdpzsd-search-doctorals',
    templateUrl: './rdpzsd-search-doctorals.component.html',
    providers: [
        PersonStudentDoctoralSearchResource
    ]
})
export class RdpzsdSearchDoctoralsComponent extends BaseRdpzsdSearchComponent<PersonDoctoralSearchFilterDto> {

	yearType = YearType;

    constructor(
        protected resource: PersonStudentDoctoralSearchResource<PersonDoctoralSearchFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource,
        protected router: Router,
        public userDataService: UserDataService,
        public translateService: TranslateService
    ) {
        super(resource, PersonDoctoralSearchFilterDto, periodNomenclatureResource, router, userDataService, translateService, 'PersonDoctoralSearch')
    }
}
