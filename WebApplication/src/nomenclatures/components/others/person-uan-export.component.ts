import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import * as saveAs from 'file-saver';
import { catchError, Observable, Observer, throwError } from 'rxjs';
import { PersonUanExportFilterDto } from 'src/rdpzsd/dtos/search/person-uan-export-filter.dto';
import { PersonUanExportDto } from 'src/rdpzsd/dtos/search/person-uan-export.dto';
import { PersonLotNewFilterType } from 'src/rdpzsd/enums/search/person-lot-new-filter-type.enum';
import { PersonLotSearchResource } from 'src/rdpzsd/resources/search/person-lot-search.resource';
import { MessageModalComponent } from 'src/shared/components/message-modal/message-modal.component';
import { SearchResultDto } from 'src/shared/dtos/search-result.dto';

@Component({
  selector: 'person-uan-export',
  templateUrl: './person-uan-export.component.html',
})
export class PersonUanExportComponent implements OnInit {
  filter: PersonUanExportFilterDto = new PersonUanExportFilterDto();
  searchResult: SearchResultDto<PersonUanExportDto> = new SearchResultDto<PersonUanExportDto>();
  personNewFilterType = PersonLotNewFilterType;
  searched = false;
  loadingData = false;
  dataCount: number = null;
  loadingDataCount = false;

  @Input() searchType: string;

  constructor(
    private personLotSearchResource: PersonLotSearchResource,
    private modalService: NgbModal,
    public translateService: TranslateService
  ) { }

  getData(loadMore: boolean, triggerLoadingDataIndicator = true) {
    this.filter.searchType = this.searchType;
    
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
      .getAllUanExport(this.filter)
      .subscribe(e => {
        this.searchResult = e;
        this.loadingData = false;
      });
  }

  getDataCount() {
    this.loadingDataCount = true;

    return this.personLotSearchResource
        .getAllUanCount(this.filter)
        .subscribe(e => {
            this.dataCount = e;
            this.loadingDataCount = false;
        });
  }

  private exportModal(){
    return new Observable((observer: Observer<any>) => {
      const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
      modal.componentInstance.text = 'rdpzsd.personLot.modals.downloadingData';
      modal.componentInstance.warning = 'rdpzsd.personLot.modals.warning';
      modal.componentInstance.acceptButton = 'root.buttons.yesGenerate';
      modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

      modal.result.then((ok) => {
          if (ok) {
              this.personLotSearchResource
                  .exportExcelUan(this.filter)
                  .pipe(
                      catchError(() => {
                          observer.next(null);
                          observer.complete();
                          return throwError(() => new Error('Export UAN error'));
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
      });
    });
  }

  export() {
    return this.exportModal()
      .subscribe((blob: Blob) => {
        if(blob) {
          const date = new Date();
          saveAs(blob, `ЕАН${date.getFullYear()}${date.getMonth() + 1}${date.getDate()}${date.getHours()}${date.getMinutes()}${date.getSeconds()}.xlsx`);
        }
      });
  }

  changeFilterType(filterType: PersonLotNewFilterType) {
    this.clear(false);
    this.filter.filterType = filterType;
    this.getData(false);
  }

  initializeFilter() {
    const filterType = this.filter.filterType;
    var filter = new PersonUanExportFilterDto();
    filter.filterType = filterType;
    return filter;
  }

  clear(reloadData = true) {
    this.searched = false;
    this.filter = this.initializeFilter();
    if (reloadData) {
      this.getData(false, false);
    }
  }

  ngOnInit() {
    this.getData(false, true);
  }
}
