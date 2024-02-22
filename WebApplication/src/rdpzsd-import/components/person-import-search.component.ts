import { DatePipe } from "@angular/common";
import { Component } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import * as saveAs from "file-saver";
import { UserDataService } from "src/users/services/user-data.service";
import { PersonImportFilterDto } from "../dtos/person-import-filter.dto";
import { PersonImportErrorFile } from "../models/files/person-import-error-file.model";
import { PersonImportFile } from "../models/files/person-import-file.model";
import { PersonImportHistoryErrorFile } from "../models/files/person-import-history-error-file.model";
import { PersonImportHistoryFile } from "../models/files/person-import-history-file.model";
import { PersonImportHistory } from "../models/person-import-history.model";
import { PersonImport } from "../models/person-import.model";
import { RdpzsdImportResource } from "../resources/rdpzsd-import.resource";
import { RdpzsdImportSearchResource } from "../resources/search/rdpzsd-import-search.resource";
import { RdpzsdImportSearchComponent } from "./base/rdpzsd-import-search.component";
import { CreatePersonImportComponent } from "./modals/create-person-import.modal";

@Component({
    selector: 'person-import-search',
    templateUrl: './person-import-search.component.html',
    providers: [
        RdpzsdImportSearchResource,
        RdpzsdImportResource
    ]
})
export class PersonImportSearchComponent extends RdpzsdImportSearchComponent<PersonImport, PersonImportFile, PersonImportErrorFile, PersonImportHistory, PersonImportHistoryFile, PersonImportHistoryErrorFile, PersonImportFilterDto> {

    constructor(
        protected searchResource: RdpzsdImportSearchResource<PersonImport, PersonImportFilterDto>,
        protected resource: RdpzsdImportResource<PersonImport>,
        protected modalService: NgbModal,
        public translateService: TranslateService,
        public userDataService: UserDataService,
        protected datepipe: DatePipe
    ) {
        super(searchResource, resource, modalService, PersonImportFilterDto, userDataService, translateService, 'PersonImport', datepipe);
    }

    downloadImportUans(personImportId: number, createDate: Date) {
        return this.resource.exportFinalExcel(personImportId, 'UanExcel')
            .subscribe((blob: Blob) => {
                const createDateFormated = this.datepipe.transform(createDate, 'dd-MM-yyyy (hh-mm ч.)');
                saveAs(blob, `${createDateFormated}_ЕАН.xlsx`);
            });
    }

    openNewPersonImportModal() {
        const modal = this.modalService.open(CreatePersonImportComponent, { backdrop: 'static', size: 'lg', keyboard: false });

        modal.result.then((personImport) => {
            if (personImport) {
                this.searchResult.result.unshift(personImport);
                this.checkHasWaitingJobItem();
            }
        });
    }
}