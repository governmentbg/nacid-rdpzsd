import { DatePipe } from "@angular/common";
import { Directive, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import * as saveAs from "file-saver";
import { Configuration } from "src/app/configuration/configuration";
import { ImportState } from "src/rdpzsd-import/enums/import-state.enum";
import { RdpzsdImportHistory } from "src/rdpzsd-import/models/base/rdpzsd-import-history.model";
import { RdpzsdImport } from "src/rdpzsd-import/models/base/rdpzsd-import.model";
import { RdpzsdImportResource } from "src/rdpzsd-import/resources/rdpzsd-import.resource";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";
import { FileUploadService } from "src/shared/services/file-upload.service";

@Directive()
export abstract class RdpzsdImportComponent<TRdpzsdImport extends RdpzsdImport<TFile, TErrorFile, TImportHistory, THistoryFile, THistoryErrorFile>,
    TFile extends RdpzsdAttachedFile,
    TErrorFile extends RdpzsdAttachedFile,
    TImportHistory extends RdpzsdImportHistory<THistoryFile, THistoryErrorFile>,
    THistoryFile extends RdpzsdAttachedFile,
    THistoryErrorFile extends RdpzsdAttachedFile> implements OnInit {

    rdpzsdImport: TRdpzsdImport = null;
    importState = ImportState;
    loadingData = false;

    constructor(
        protected configuration: Configuration,
        protected route: ActivatedRoute,
        protected resource: RdpzsdImportResource<TRdpzsdImport>,
        protected fileUploadService: FileUploadService,
        protected childUrl: string,
        protected datepipe: DatePipe
    ) {
        this.resource.init(childUrl);
    }

    downloadFile(key: string, fileName: string, dbId: number, mimeType: string, isError: boolean, createDate: Date) {
        const fileUrl = `${this.configuration.restUrl}FileStorage?key=${key}&fileName=${fileName}&dbId=${dbId}`;

        return this.fileUploadService.getFile(fileUrl, mimeType)
            .subscribe((blob: Blob) => {
                if (isError) {
                    const createDateFormated = this.datepipe.transform(createDate, 'dd-MM-yyyy (hh-mm ч.)');
                    fileName = `${createDateFormated}_${fileName}`;
                }

                saveAs(blob, fileName);
            });
    }

    private getData(id: number) {
        this.resource.get(id)
            .subscribe(e => {
                this.rdpzsdImport = e;
                this.loadingData = false;
            });
    }

    ngOnInit() {
        this.route.params.subscribe(p => {
            this.getData(p.id);
        });
    }
}