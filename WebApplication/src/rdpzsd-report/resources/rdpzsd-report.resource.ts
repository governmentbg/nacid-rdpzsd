import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";

@Injectable()
export class RdpzsdReportResource<TFilter> {

    url = `${this.configuration.restUrl}`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}Report`;
    }

    exportReportExcel(filter: TFilter): Observable<Blob> {
        return this.http.post(`${this.url}/Excel`, filter, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
            );
    }
}