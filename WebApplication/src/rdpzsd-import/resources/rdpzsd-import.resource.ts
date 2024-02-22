import { HttpClient, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";

@Injectable()
export class RdpzsdImportResource<TRdpzsdImport> {

    url = `${this.configuration.restUrl}`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    get(id: number) {
        return this.http.get<TRdpzsdImport>(`${this.url}/${id}`);
    }

    create(rdpzsdImport: TRdpzsdImport) {
        return this.http.post<TRdpzsdImport>(`${this.url}/Create`, rdpzsdImport);
    }

    setForRegistration(id: number) {
        return this.http.get<TRdpzsdImport>(`${this.url}/${id}/SetForRegistration`);
    }

    delete(id: number) {
        return this.http.delete<TRdpzsdImport>(`${this.url}/${id}`);
    }

    changeImportFile(txtFile: File, rdpzsdImportId: number): Observable<HttpEvent<TRdpzsdImport>> {
        const formData = new FormData();
        formData.append('file', txtFile, txtFile.name);

        return this.http.post<TRdpzsdImport>(`${this.url}FileUpload/${rdpzsdImportId}`, formData,
            { reportProgress: true, observe: 'events' });
    }

    downloadImportFile(id: number): Observable<Blob> {
        return this.http.get(`${this.url}/${id}/DownloadImportFile`,
            { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'text/plain;charset=utf-8' }))
            );
    }

    downloadImportErrorFile(id: number): Observable<Blob> {
        return this.http.get(`${this.url}/${id}/DownloadImportErrorFile`,
            { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
            );
    }

    exportFinalExcel(id: number, childRoute: string) {
        return this.http.get(`${this.url}/${id}/${childRoute}`, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
            );
    }
}