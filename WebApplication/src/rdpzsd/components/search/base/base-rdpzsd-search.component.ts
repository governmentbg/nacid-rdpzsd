import { Directive, HostListener, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Doctor } from 'src/nomenclatures/models/institution/educational-qualification.model';
import { InstitutionSpeciality } from 'src/nomenclatures/models/institution/institution-speciality.model';
import { Institution } from 'src/nomenclatures/models/institution/institution.model';
import { StudentEventGraduatedWithoutDiploma } from 'src/nomenclatures/models/student-status/student-event.model';
import { StudentStatusActive, StudentStatusGraduated, StudentStatusInterrupted } from 'src/nomenclatures/models/student-status/student-status.model';
import { PeriodNomenclatureResource } from 'src/nomenclatures/resources/period-nomenclature.resource';
import { BasePersonStudentDoctoralFilterDto } from 'src/rdpzsd/dtos/search/person-student-doctoral/base-person-student-doctoral-filter.dto';
import { PersonStudentDoctoralSearchDto } from 'src/rdpzsd/dtos/search/person-student-doctoral/person-student-doctoral-search.dto';
import { LotState } from 'src/rdpzsd/enums/lot-state.enum';
import { PersonDoctoralStatusType, PersonGraduatedStudentStatusType, PersonStudentStatusType } from 'src/rdpzsd/enums/search/person-student-status-type.enum';
import { PersonStudentDoctoralSearchResource } from 'src/rdpzsd/resources/search/person-student-doctoral-search.resource';
import { MigrationNoEmail, MigrationNoPhoneNumber } from 'src/shared/constraints/global-constraints';
import { SearchResultDto } from 'src/shared/dtos/search-result.dto';
import { Level } from 'src/shared/enums/level.enum';
import { UserDataService } from 'src/users/services/user-data.service';

@Directive()
export abstract class BaseRdpzsdSearchComponent<TFilter extends BasePersonStudentDoctoralFilterDto> implements OnInit {

    isGraduated = false;
    @Input('isGraduated')
    set isGraduatedSetter(isGraduated: boolean) {
        this.isGraduated = isGraduated;
        this.filter = this.initializeFilter(this.structuredFilterType);
    }

    level = Level;
    personStudentStatusType = PersonStudentStatusType;
    personDoctoralStatusType = PersonDoctoralStatusType;
    personGraduatedStudentStatusType = PersonGraduatedStudentStatusType;
    doctor = Doctor;
    filter: TFilter = null;
    searchResult: SearchResultDto<PersonStudentDoctoralSearchDto> = new SearchResultDto<PersonStudentDoctoralSearchDto>();
    lotState = LotState;
    loadingData = false;
    loadingDataCount = false;
    dataCount: number = null;
    studentStatusInterrupted = StudentStatusInterrupted;
    studentStatusActive = StudentStatusActive;
    studentStatusGraduated = StudentStatusGraduated;
    studentEventGraduatedWithoutDiploma = StudentEventGraduatedWithoutDiploma;

    migrationNoPhoneNumber = MigrationNoPhoneNumber;
    migrationNoEmail = MigrationNoEmail;

    @HostListener('document:keydown.enter', ['$event'])
    handleKeyboardEvent() {
        this.getData(false, false);
    }

    constructor(
        protected resource: PersonStudentDoctoralSearchResource<TFilter>,
        protected structuredFilterType: new () => TFilter,
        public periodNomenclatureResource: PeriodNomenclatureResource,
        protected router: Router,
        public userDataService: UserDataService,
        public translateService: TranslateService,
        protected childUrl: string
    ) {
        this.resource.init(childUrl);
    }

    initializeFilter(C: { new(): TFilter }) {
        var newFilter = new C();

        if (this.userDataService.isRsdUser()) {
            newFilter.institution = new Institution();
            newFilter.institutionId = this.userDataService.userData.institution.id;
            newFilter.institution.name = this.userDataService.userData.institution.name;
            newFilter.institution.nameAlt = this.userDataService.userData.institution.nameAlt;
            newFilter.institution.shortName = this.userDataService.userData.institution.shortName;
            newFilter.institution.shortNameAlt = this.userDataService.userData.institution.shortNameAlt;
            newFilter.institution.organizationType = this.userDataService.userData.institution.organizationType;
        }

        if (this.isGraduated) {
            newFilter.studentStatus = this.personStudentStatusType.graduated;
        }

        return newFilter;
    }

    clear() {
        this.filter = this.initializeFilter(this.structuredFilterType);
        return this.getData(false, false);
    }

    getData(loadMore: boolean, triggerLoadingDataIndicator = true) {
        if (triggerLoadingDataIndicator) {
            this.loadingData = true;
        }

        if (loadMore) {
            this.filter.limit = this.filter.limit + this.searchResult.result.length;
        } else {
            this.filter.limit = 15;
        }

        this.dataCount = null;

        return this.resource
            .getAll(this.filter)
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
            });
    }

    getDataCount() {
        this.loadingDataCount = true;

        return this.resource
            .getAllCount(this.filter)
            .subscribe(e => {
                this.dataCount = e;
                this.loadingDataCount = false;
            });
    }

    getStudentLot(uan: string, activeTab: string) {
        this.router.navigate(['/personLot', uan, activeTab]);
    }

    institutionSpecialityChanged(institutionSpeciality: InstitutionSpeciality) {
        this.filter.institutionSpeciality = institutionSpeciality;
        this.filter.institutionSpecialityId = institutionSpeciality?.id;
        this.filter.educationalForm = institutionSpeciality?.educationalForm;
        this.filter.educationalFormId = institutionSpeciality?.educationalFormId;
        this.filter.educationalQualification = institutionSpeciality?.speciality?.educationalQualification;
        this.filter.educationalQualificationId = institutionSpeciality?.speciality?.educationalQualificationId;
    }

    ngOnInit() {
        return this.getData(false, true);
    }
}
