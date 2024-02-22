import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { SpecialityImport } from "src/rdpzsd-import/models/speciality-import.model";
import { RdpzsdImportResource } from "src/rdpzsd-import/resources/rdpzsd-import.resource";

@Component({
    selector: 'create-speciality-import',
    templateUrl: './create-speciality-import.modal.html',
    providers: [RdpzsdImportResource]
})
export class CreateSpecialityImportComponent {
    specialityImport = new SpecialityImport();

    constructor(
        private activeModal: NgbActiveModal,
        private resource: RdpzsdImportResource<SpecialityImport>,
        public configuration: Configuration
    ) {
        resource.init('SpecialityImport');
    }

    save() {
        return this.resource.create(this.specialityImport)
            .pipe(
                catchError(() => {
                    this.cancel();
                    return throwError(() => new Error('Error'));
                })
            )
            .subscribe(e => this.activeModal.close(e));
    }

    cancel() {
        this.activeModal.close();
    }
}