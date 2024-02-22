import { Component, Input } from '@angular/core';
import { ControlContainer, NgForm } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { PersonBasic } from 'src/rdpzsd/models/parts/person-basic/person-basic.model';
import { Level } from 'src/shared/enums/level.enum';
import { SettlementChangeService } from 'src/shared/services/settlements/settlement-change.service';

@Component({
    selector: 'person-basic-additional-tabs',
    templateUrl: './person-basic-additional-tabs.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class PersonBasicAdditionalTabsComponent {

    @Input() personBasic = new PersonBasic();
    @Input() isEditMode = true;

    activeTab = 'BirthPlace';
    level = Level;

    constructor(
        public translateService: TranslateService,
        public settlementChangeService: SettlementChangeService
    ) {
    }

    birthSettlementClicked() {
        if(this.isEditMode) {
            this.personBasic.birthMunicipalityId = null;
            this.personBasic.birthMunicipality = null;
            this.personBasic.birthDistrictId = null;
            this.personBasic.birthDistrict = null;
        }
    }
}
