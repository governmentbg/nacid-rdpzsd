import { Component, HostListener, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import * as saveAs from "file-saver";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { StudentStatusActive, StudentStatusInterrupted } from "src/nomenclatures/models/student-status/student-status.model";
import { PersonLotNewFilterDto } from "src/rdpzsd/dtos/search/person-lot-new-filter.dto";
import { PersonSearchDto } from "src/rdpzsd/dtos/search/person-search.dto";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PersonLotNewFilterType } from "src/rdpzsd/enums/search/person-lot-new-filter-type.enum";
import { PersonLotSearchResource } from "src/rdpzsd/resources/search/person-lot-search.resource";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";
import { UserDataService } from "src/users/services/user-data.service";

@Component({
    selector: 'rdpzsd-search-new',
    templateUrl: './rdpzsd-search-new.component.html'
})
export class RdpzsdSearchNewComponent implements OnInit {

    filter: PersonLotNewFilterDto = new PersonLotNewFilterDto();
    searchResult: SearchResultDto<PersonSearchDto> = new SearchResultDto<PersonSearchDto>();
    lotState = LotState;
    personLotNewFilterType = PersonLotNewFilterType;
    searched = false;
    dataCount: number = null;
    studentStatusInterrupted = StudentStatusInterrupted;
    studentStatusActive = StudentStatusActive;
    loadingData = false;
    loadingDataCount = false;

    @ViewChild('searchForm') form: NgForm;

    disableUan = false;
    disableUin = false;
    disableFn = false;

    @HostListener('document:keydown.enter', ['$event'])
    handleKeyboardEvent() {
        if (this.form.valid && (this.form.controls.uan.value || this.form.controls.uin.value || this.form.controls.foreignerNumber.value)) {
            this.getData(false, false, false);
        }
    }

    constructor(
        private personLotSearchResource: PersonLotSearchResource,
        private router: Router,
        private modalService: NgbModal,
        public userDataService: UserDataService,
        public translateService: TranslateService
    ) {
    }

    initializeFilter() {
        const filterType = this.filter.filterType;
        var filter = new PersonLotNewFilterDto();
        filter.filterType = filterType;
        return filter;
    }

    clear(reloadData = true) {
        this.searched = false;
        this.disableFn = false;
        this.disableUan = false;
        this.disableUin = false;
        this.filter = this.initializeFilter();
        if (reloadData) {
            this.getData(false, true, true);
        }
    }

    changeFilterType(filterType: PersonLotNewFilterType) {
        this.clear(false);
        this.filter.filterType = filterType;
        this.getData(false, true, true);
    }

    getData(loadMore: boolean, showNotMapped: boolean, triggerLoadingDataIndicator: boolean) {
        if (loadMore) {
            this.filter.limit = this.filter.limit + this.searchResult.result.length;
        } else {
            this.filter.limit = 15;
        }

        this.loadingData = triggerLoadingDataIndicator;
        this.filter.showNotMapped = showNotMapped;
        this.dataCount = null;

        return this.personLotSearchResource
            .getAllNew(this.filter)
            .subscribe(e => {
                this.searchResult = e;
                this.searched = !showNotMapped;
                this.loadingData = false;
            });
    }

    private exportModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.warning = 'rdpzsd.personLot.modals.warning';
            modal.componentInstance.warningText = 'rdpzsd.personLot.modals.personalData';
            modal.componentInstance.text = 'rdpzsd.personLot.modals.exportData';
            modal.componentInstance.acceptButton = 'root.buttons.yesSure';
            modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

            modal.result.then((ok) => {
                if (ok) {
                    const secondModal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
                    secondModal.componentInstance.warning = 'rdpzsd.personLot.modals.warning';
                    secondModal.componentInstance.text = 'rdpzsd.personLot.modals.downloadingData';
                    secondModal.componentInstance.acceptButton = 'root.buttons.yesGenerate';
                    secondModal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

                    secondModal.result.then((ok) => {
                        if (ok) {
                            this.personLotSearchResource
                                .exportExcel(this.filter)
                                .pipe(
                                    catchError(() => {
                                        observer.next(null);
                                        observer.complete();
                                        return throwError(() => new Error('Export person basic excel error'));
                                    })
                                )
                                .subscribe((blob: Blob) => {
                                    observer.next(blob);
                                    observer.complete();
                                });
                        } else {
                            observer.next(null);
                            observer.complete();
                        }
                    })

                } else {
                    observer.next(null);
                    observer.complete();
                }
            });
        });
    }

    export() {
        return this.exportModal()
            .subscribe((blob: Blob) => {
                if (blob) {
                    const date = new Date();
                    saveAs(blob, `Основни данни${date.getFullYear()}${date.getMonth() + 1}${date.getDate()}${date.getHours()}${date.getMinutes()}${date.getSeconds()}.xlsx`);
                }
            });
    }

    getDataCount() {
        this.loadingDataCount = true;

        return this.personLotSearchResource
            .getAllNewCount(this.filter)
            .subscribe(e => {
                this.dataCount = e;
                this.loadingDataCount = false;
            });
    }

    createInitialLot() {
        if (!this.filter.uin) {
            this.filter.uin = null;
        }

        if (!this.filter.foreignerNumber) {
            this.filter.foreignerNumber = null;
        }

        this.router.navigateByUrl('/rdpzsd/new', { state: { personLotNewFilterDto: this.filter } });
    }

    getPersonLot(uan: string) {
        this.router.navigate(['/personLot', uan, 'Basic']);
    }

    checkInputs() {
        this.searched = false;

        this.filter.uin && this.filter.uin !== '' ?
            (this.disableFn = true, this.disableUan = true) :
            this.filter.uin === '' && !this.filter.uan && !this.filter.foreignerNumber ?
                (this.disableFn = false, this.disableUan = false) : null;

        this.filter.uan && this.filter.uan !== '' ?
            (this.disableUin = true, this.disableFn = true) :
            this.filter.uan === '' && !this.filter.uin && !this.filter.foreignerNumber ?
                (this.disableUin = false, this.disableFn = false) : null;

        this.filter.foreignerNumber && this.filter.foreignerNumber !== '' ?
            (this.disableUan = true, this.disableUin = true) :
            this.filter.foreignerNumber === '' && !this.filter.uin && !this.filter.uan ?
                (this.disableUan = false, this.disableUin = false) : null;
    }

    ngOnInit() {
        return this.getData(false, true, true);
    }
}