import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PersonApprovalFilterDto } from 'src/rdpzsd/dtos/search/person-approval-filter.dto';
import { PersonApprovalSearchDto } from 'src/rdpzsd/dtos/search/person-approval-search.dto';
import { ApprovalState } from 'src/rdpzsd/enums/approval-state.enum';
import { LotState } from 'src/rdpzsd/enums/lot-state.enum';
import { PersonLotSearchResource } from 'src/rdpzsd/resources/search/person-lot-search.resource';
import { SearchResultDto } from 'src/shared/dtos/search-result.dto';
import { Level } from 'src/shared/enums/level.enum';
import { InstitutionChangeService } from 'src/shared/services/institutions/institution-change.service';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
    selector: 'rdpzsd-search-approval',
    templateUrl: './rdpzsd-search-approval.component.html',
})
export class RdpzsdSearchApprovalComponent implements OnInit {
    filter: PersonApprovalFilterDto = new PersonApprovalFilterDto();
    searchResult: SearchResultDto<PersonApprovalSearchDto> = new SearchResultDto<PersonApprovalSearchDto>();
    level = Level;
    loadingData = false;
    lotState = LotState;
    approvalState = ApprovalState;
    dataCount: number = null;
    loadingDataCount = false;

    @HostListener('document:keydown.enter', ['$event'])
    handleKeyboardEvent() {
        this.getData(false, false);
    }

    constructor(
        private personLotSearchResource: PersonLotSearchResource,
        private router: Router,
        public userDataService: UserDataService,
        public translateService: TranslateService,
        public institutionChangeService: InstitutionChangeService
    ) { }

    initializeFilter() {
        var filter = new PersonApprovalFilterDto();
        return filter;
    }

    clear() {
        this.filter = this.initializeFilter();
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

        return this.personLotSearchResource
            .getAllForApproval(this.filter)
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
            });
    }

    getDataCount() {
        this.loadingDataCount = true;

        return this.personLotSearchResource
            .getAllForApprovalCount(this.filter)
            .subscribe(e => {
                this.dataCount = e;
                this.loadingDataCount = false;
            });
    }

    getStudentForApproval(uan: string) {
        this.router.navigate(['/personLot', uan, 'Basic']);
    }

    changeStateType(stateType: ApprovalState) {
        this.filter = this.initializeFilter();
        this.filter.state = stateType;
        return this.getData(false, true);
    }

    ngOnInit() {
        return this.getData(false, true);
    }
}
