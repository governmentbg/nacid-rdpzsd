import { HttpClient, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Configuration } from "src/app/configuration/configuration";
import { RdpzsdAttachedFile } from "../models/rdpzsd-attached-file.model";

@Injectable({
  providedIn: 'root',
})
export class FileUploadService {

  constructor(
    private http: HttpClient,
    private configuration: Configuration) { }

  uploadFile(file: File, fileStorageUrl: string): Observable<HttpEvent<RdpzsdAttachedFile>> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post<RdpzsdAttachedFile>(fileStorageUrl, formData,
      { reportProgress: true, observe: 'events' });
  }

  getFile(fileUrl: string, mimeType: string): Observable<Blob> {
    return this.http.get(fileUrl,
      { responseType: 'blob' })
      .pipe(
        map(response => new Blob([response], { type: mimeType }))
      );
  }

  getNewImage(file: RdpzsdAttachedFile): Observable<string> {
    return this.http.post(`${this.configuration.restUrl}FileStorage/Image`, file, { responseType: 'text' });
  }
  getFileToAnchorUrl(file: RdpzsdAttachedFile) {
	const fileUrl = `${this.configuration.restUrl}FileStorage?key=${file.key}&fileName=${file.name}&dbId=${file.dbId}`;
			
	return this.getFile(fileUrl, file.mimeType)
			.subscribe((blob: Blob) => {
				var url  = window.URL.createObjectURL(blob);
				window.open(url);
			});
  }
}
