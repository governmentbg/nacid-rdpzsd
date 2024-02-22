import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { PersonImport } from "src/rdpzsd-import/models/person-import.model";
import { RdpzsdImportResource } from "src/rdpzsd-import/resources/rdpzsd-import.resource";

@Component({
    selector: 'create-person-import',
    templateUrl: './create-person-import.modal.html',
    providers: [RdpzsdImportResource]
})
export class CreatePersonImportComponent {
    personImport = new PersonImport();

    constructor(
        private activeModal: NgbActiveModal,
        private resource: RdpzsdImportResource<PersonImport>,
        public configuration: Configuration
    ) {
        resource.init('PersonImport');
    }

    save() {
        return this.resource.create(this.personImport)
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