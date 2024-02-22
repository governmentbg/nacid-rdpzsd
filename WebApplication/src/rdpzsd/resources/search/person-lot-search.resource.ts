import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { PersonApprovalFilterDto } from "src/rdpzsd/dtos/search/person-approval-filter.dto";
import { PersonApprovalSearchDto } from "src/rdpzsd/dtos/search/person-approval-search.dto";
import { PersonLotNewFilterDto } from "src/rdpzsd/dtos/search/person-lot-new-filter.dto";
import { PersonSearchDto } from "src/rdpzsd/dtos/search/person-search.dto";
import { PersonUanExportFilterDto } from "src/rdpzsd/dtos/search/person-uan-export-filter.dto";
import { PersonUanExportDto } from "src/rdpzsd/dtos/search/person-uan-export.dto";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Injectable()
export class PersonLotSearchResource {

    url = `${this.configuration.restUrl}PersonLotSearch`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    getAllNew(filter: PersonLotNewFilterDto) {
        return this.http.post<SearchResultDto<PersonSearchDto>>(`${this.url}/New`, filter);
    }

    getAllNewCount(filter: PersonLotNewFilterDto) {
        return this.http.post<number>(`${this.url}/NewCount`, filter);
    }

    getAllForApproval(filter: PersonApprovalFilterDto) {
        return this.http.post<SearchResultDto<PersonApprovalSearchDto>>(`${this.url}/Approval`, filter);
    }

    getAllForApprovalCount(filter: PersonApprovalFilterDto) {
        return this.http.post<number>(`${this.url}/ApprovalCount`, filter);
    }

    getAllUanExport(filter: PersonUanExportFilterDto) {
        return this.http.post<SearchResultDto<PersonUanExportDto>>(`${this.url}/Uan`, filter);
    }

    getAllUanCount(filter: PersonUanExportFilterDto){
        return this.http.post<number>(`${this.url}/UanCount`, filter);
    }

    exportExcel(filter: PersonLotNewFilterDto): Observable<Blob> {
        return this.http.post(`${this.url}/Excel`, filter, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
            );
    }

    exportExcelUan(filter: PersonUanExportFilterDto): Observable<Blob> {
        return this.http.post(`${this.url}/ExcelUan`, filter, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
            );
    }
}