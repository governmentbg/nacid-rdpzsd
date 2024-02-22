import { DatePipe } from "@angular/common";
import { HttpEvent, HttpEventType } from "@angular/common/http";
import { Directive, HostListener } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import * as saveAs from "file-saver";
import { catchError, throwError } from "rxjs";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { RdpzsdImportFilterDto } from "src/rdpzsd-import/dtos/rdpzsd-import-filter.dto";
import { ImportState, ImportStateSearch } from "src/rdpzsd-import/enums/import-state.enum";
import { RdpzsdImportHistory } from "src/rdpzsd-import/models/base/rdpzsd-import-history.model";
import { RdpzsdImport } from "src/rdpzsd-import/models/base/rdpzsd-import.model";
import { RdpzsdImportResource } from "src/rdpzsd-import/resources/rdpzsd-import.resource";
import { RdpzsdImportSearchResource } from "src/rdpzsd-import/resources/search/rdpzsd-import-search.resource";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";
import { Level } from "src/shared/enums/level.enum";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";
import { UserDataService } from "src/users/services/user-data.service";

@Directive()
export abstract class RdpzsdImportSearchComponent<TRdpzsdImport extends RdpzsdImport<TFile, TErrorFile, TImportHistory, THistoryFile, THistoryErrorFile>,
    TFile extends RdpzsdAttachedFile,
    TErrorFile extends RdpzsdAttachedFile,
    TImportHistory extends RdpzsdImportHistory<THistoryFile, THistoryErrorFile>,
    THistoryFile extends RdpzsdAttachedFile,
    THistoryErrorFile extends RdpzsdAttachedFile,
    TFilter extends RdpzsdImportFilterDto> {

    searchResult: SearchResultDto<TRdpzsdImport> = new SearchResultDto<TRdpzsdImport>();
    filter: TFilter = this.initializeFilter(this.structuredFilterType);
    loadingData = false;
    loadingDataCount = false;
    dataCount: number = null;
    importState = ImportState;
    importStateSearch = ImportStateSearch;
    level = Level;
    reloadingItems = false;
    downloadingErrorFileId: number = null;
    txtChangeFileId: number = null;
    hasWaitingJobItem = false;

    @HostListener('document:keydown.enter', ['$event'])
    handleKeyboardEvent() {
        this.getData(false, false);
    }

    constructor(
        protected searchResource: RdpzsdImportSearchResource<TRdpzsdImport, TFilter>,
        protected resource: RdpzsdImportResource<TRdpzsdImport>,
        protected modalService: NgbModal,
        protected structuredFilterType: new () => TFilter,
        public userDataService: UserDataService,
        public translateService: TranslateService,
        protected childUrl: string,
        protected datepipe: DatePipe
    ) {
        this.searchResource.init(childUrl + 'Search');
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

        return newFilter;
    }

    clear() {
        this.filter = this.initializeFilter(this.structuredFilterType);
        return this.getData(false, false);
    }

    reloadItems() {
        this.reloadingItems = true;
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

        return this.searchResource
            .getAll(this.filter)
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
                this.reloadingItems = false;
                this.checkHasWaitingJobItem();
            });
    }

    getDataCount() {
        this.loadingDataCount = true;

        return this.searchResource
            .getAllCount(this.filter)
            .subscribe(e => {
                this.dataCount = e;
                this.loadingDataCount = false;
            });
    }

    setForRegistration(id: number, index: number) {
        return this.resource.setForRegistration(id)
            .subscribe(e => {
                this.searchResult.result[index] = e;
                this.checkHasWaitingJobItem();
            });
    }

    changeImportFile(event: any, id: number, index: number) {
        const target = event.target || event.srcElement;
        const files = target.files;
        if (files.length === 1 && id !== this.txtChangeFileId) {
            this.txtChangeFileId = id;
            return this.resource.changeImportFile(files[0], id)
                .pipe(
                    catchError(() => {
                        this.txtChangeFileId = null;
                        return throwError(() => new Error('Error'));
                    })
                )
                .subscribe((importEvent: HttpEvent<TRdpzsdImport>) => {
                    if (importEvent.type === HttpEventType.Response) {
                        this.txtChangeFileId = null;
                        this.searchResult.result[index] = importEvent.body;
                        this.checkHasWaitingJobItem();
                    }
                });
        } else {
            return null;
        }
    }

    delete(id: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personLot.modals.deleteNomenclature';
        modal.componentInstance.acceptButton = 'root.buttons.delete';

        modal.result.then((ok) => {
            if (ok) {
                return this.resource
                    .delete(id)
                    .subscribe((e) => {
                        this.searchResult.result[index] = e;
                        this.checkHasWaitingJobItem();
                    });
            } else {
                return null;
            }
        });
    }

    downloadImportFile(id: number, fileName: string) {
        return this.resource.downloadImportFile(id)
            .subscribe((blob: Blob) => {
                saveAs(blob, fileName);
            });
    }

    downloadImportErrorFile(id: number, fileName: string, createDate: Date) {
        this.downloadingErrorFileId = id;
        return this.resource.downloadImportErrorFile(id)
            .subscribe((blob: Blob) => {
                const createDateFormated = this.datepipe.transform(createDate, 'dd-MM-yyyy (hh-mm ч.)');
                saveAs(blob, `${createDateFormated}_${fileName}`);
                this.downloadingErrorFileId = null;
            });
    }

    checkHasWaitingJobItem() {
        this.hasWaitingJobItem = this.searchResult.result.some(e => e.state == this.importState.draft
            || e.state == this.importState.inProgress
            || e.state == this.importState.waitingRegistration
            || e.state == this.importState.inProgressRegistration);
    }

    ngOnInit() {
        return this.getData(false, true);
    }
}