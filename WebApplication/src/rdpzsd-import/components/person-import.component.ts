import { DatePipe } from "@angular/common";
import { Component } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Configuration } from "src/app/configuration/configuration";
import { FileUploadService } from "src/shared/services/file-upload.service";
import { PersonImportErrorFile } from "../models/files/person-import-error-file.model";
import { PersonImportFile } from "../models/files/person-import-file.model";
import { PersonImportHistoryErrorFile } from "../models/files/person-import-history-error-file.model";
import { PersonImportHistoryFile } from "../models/files/person-import-history-file.model";
import { PersonImportHistory } from "../models/person-import-history.model";
import { PersonImport } from "../models/person-import.model";
import { RdpzsdImportResource } from "../resources/rdpzsd-import.resource";
import { RdpzsdImportComponent } from "./base/rdpzsd-import.component";

@Component({
    selector: 'person-import',
    templateUrl: './person-import.component.html',
    providers: [
        RdpzsdImportResource
    ]
})
export class PersonImportComponent extends RdpzsdImportComponent<PersonImport, PersonImportFile, PersonImportErrorFile, PersonImportHistory, PersonImportHistoryFile, PersonImportHistoryErrorFile> {

    constructor(
        protected configuration: Configuration,
        protected route: ActivatedRoute,
        protected resource: RdpzsdImportResource<PersonImport>,
        protected fileUploadService: FileUploadService,
        private router: Router,
        protected datepipe: DatePipe
    ) {
        super(configuration, route, resource, fileUploadService, 'PersonImport', datepipe);
    }

    goBack() {
        this.router.navigate(['/personImport']);
    }
}