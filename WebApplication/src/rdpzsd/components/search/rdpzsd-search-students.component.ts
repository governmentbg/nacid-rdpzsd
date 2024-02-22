import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PeriodNomenclatureResource } from 'src/nomenclatures/resources/period-nomenclature.resource';
import { PersonStudentSearchFilterDto } from 'src/rdpzsd/dtos/search/person-student-doctoral/person-student-search-filter.dto';
import { CourseType } from 'src/rdpzsd/enums/parts/course.enum';
import { PersonStudentStatusType } from 'src/rdpzsd/enums/search/person-student-status-type.enum';
import { PersonStudentDoctoralSearchResource } from 'src/rdpzsd/resources/search/person-student-doctoral-search.resource';
import { UserDataService } from 'src/users/services/user-data.service';
import { BaseRdpzsdSearchComponent } from './base/base-rdpzsd-search.component';

@Component({
    selector: 'rdpzsd-search-students',
    templateUrl: './rdpzsd-search-students.component.html',
    providers: [
        PersonStudentDoctoralSearchResource
    ]
})
export class RdpzsdSearchStudentsComponent extends BaseRdpzsdSearchComponent<PersonStudentSearchFilterDto> {

    courseType = CourseType;

    constructor(
        protected resource: PersonStudentDoctoralSearchResource<PersonStudentSearchFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource,
        protected router: Router,
        public userDataService: UserDataService,
        public translateService: TranslateService
    ) {
        super(resource, PersonStudentSearchFilterDto, periodNomenclatureResource, router, userDataService, translateService, 'PersonStudentSearch')
    }

    studentStatusChanged(studentStatusEnum: PersonStudentStatusType) {
        if (studentStatusEnum === this.personStudentStatusType.graduated || studentStatusEnum === this.personStudentStatusType.graduatedWithoutDiploma) {
            this.filter.period = null;
            this.filter.periodId = null;
        }
    }
}
