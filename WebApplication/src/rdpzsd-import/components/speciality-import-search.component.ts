import { DatePipe } from "@angular/common";
import { Component } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { UserDataService } from "src/users/services/user-data.service";
import { SpecialityImportFilterDto } from "../dtos/speciality-import.filter.dto";
import { SpecialityImportErrorFile } from "../models/files/speciality-import-error-file.model";
import { SpecialityImportFile } from "../models/files/speciality-import-file.model";
import { SpecialityImportHistoryErrorFile } from "../models/files/speciality-import-history-error-file.model";
import { SpecialityImportHistoryFile } from "../models/files/speciality-import-history-file.model";
import { SpecialityImportHistory } from "../models/speciality-import.history.model";
import { SpecialityImport } from "../models/speciality-import.model";
import { RdpzsdImportResource } from "../resources/rdpzsd-import.resource";
import { RdpzsdImportSearchResource } from "../resources/search/rdpzsd-import-search.resource";
import { RdpzsdImportSearchComponent } from "./base/rdpzsd-import-search.component";
import { CreateSpecialityImportComponent } from "./modals/create-speciality-import.modal";

@Component({
    selector: 'speciality-import-search',
    templateUrl: './speciality-import-search.component.html',
    providers: [
        RdpzsdImportSearchResource,
        RdpzsdImportResource
    ]
})
export class SpecialityImportSearchComponent extends RdpzsdImportSearchComponent<SpecialityImport, SpecialityImportFile, SpecialityImportErrorFile, SpecialityImportHistory, SpecialityImportHistoryFile, SpecialityImportHistoryErrorFile, SpecialityImportFilterDto> {

    constructor(
        protected searchResource: RdpzsdImportSearchResource<SpecialityImport, SpecialityImportFilterDto>,
        protected resource: RdpzsdImportResource<SpecialityImport>,
        protected modalService: NgbModal,
        public translateService: TranslateService,
        public userDataService: UserDataService,
        protected datepipe: DatePipe
    ) {
        super(searchResource, resource, modalService, SpecialityImportFilterDto, userDataService, translateService, 'SpecialityImport', datepipe);
    }

    openNewSpecialityImportModal() {
        const modal = this.modalService.open(CreateSpecialityImportComponent, { backdrop: 'static', size: 'lg', keyboard: false });

        modal.result.then((specialityImport) => {
            if (specialityImport) {
                this.searchResult.result.unshift(specialityImport);
                this.checkHasWaitingJobItem();
            }
        });
    }
}