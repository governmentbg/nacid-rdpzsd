import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Injectable()
export class RdpzsdImportSearchResource<TRdpzsdImport, TRdpzsdImportFilterDto> {

    url = `${this.configuration.restUrl}`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    getAll(filter: TRdpzsdImportFilterDto) {
        return this.http.post<SearchResultDto<TRdpzsdImport>>(`${this.url}`, filter);
    }

    getAllCount(filter: TRdpzsdImportFilterDto) {
        return this.http.post<number>(`${this.url}/Count`, filter);
    }
}