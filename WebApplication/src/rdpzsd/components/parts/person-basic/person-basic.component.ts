import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { Configuration } from "src/app/configuration/configuration";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PersonBasic } from "src/rdpzsd/models/parts/person-basic/person-basic.model";
import { UserDataService } from "src/users/services/user-data.service";
import { BasePersonBasicComponent } from "./base/base-person-basic.component";

@Component({
    selector: 'person-basic',
    templateUrl: './person-basic.component.html'
})
export class PersonBasicComponent extends BasePersonBasicComponent implements AfterViewInit {

    @Input() personLotId: number = null;
    @Input() personLotState: LotState;
    @Input() personBasic: PersonBasic = new PersonBasic();
    @Input() isEditMode = false;
    @Input() isValidForm = false;
    @Output() isValidFormChange = new EventEmitter<boolean>();

    lotState = LotState;

    @ViewChild('form') form: NgForm;

    constructor(
        public translateService: TranslateService,
        public userDataService: UserDataService,
        public configuration: Configuration
    ) {
        super(translateService, userDataService);
    }

    ngAfterViewInit() {
        this.form.valueChanges
            .subscribe(() => {
                if (this.isValidForm !== this.form.valid) {
                    this.isValidForm = this.form.valid;
                    this.isValidFormChange.emit(this.form.valid);
                }
            });
    }
}