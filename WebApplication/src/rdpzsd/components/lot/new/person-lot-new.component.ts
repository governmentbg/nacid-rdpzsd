import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { Configuration } from 'src/app/configuration/configuration';
import { NomenclatureCodeFilterDto } from 'src/nomenclatures/dtos/nomenclature-code-filter.dto';
import { NomenclatureFilterDto } from 'src/nomenclatures/dtos/nomenclature-filter.dto';
import { Country } from 'src/nomenclatures/models/settlement/country.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { BaseNomenclatureResource } from 'src/nomenclatures/resources/base/base-nomenclature.resource';
import { PersonLotNewFilterDto } from 'src/rdpzsd/dtos/search/person-lot-new-filter.dto';
import { PersonLotNewFilterType } from 'src/rdpzsd/enums/search/person-lot-new-filter-type.enum';
import { PersonBasic } from 'src/rdpzsd/models/parts/person-basic/person-basic.model';
import { PersonLotResource } from 'src/rdpzsd/resources/person-lot.resource';
import { MessageModalComponent } from 'src/shared/components/message-modal/message-modal.component';
import { SettlementChangeService } from 'src/shared/services/settlements/settlement-change.service';
import { UserDataService } from 'src/users/services/user-data.service';
import { BasePersonBasicComponent } from '../../parts/person-basic/base/base-person-basic.component';

@Component({
    selector: 'person-lot-new',
    templateUrl: './person-lot-new.component.html',
    providers: [BaseNomenclatureCodeResource]
})
export class PersonLotNewComponent extends BasePersonBasicComponent {

    personBasic = new PersonBasic();
    personLotNewFilterDto = new PersonLotNewFilterDto();
    personLotNewFilterType = PersonLotNewFilterType;

    constructor(
        private router: Router,
        private personLotResource: PersonLotResource,
        private modalService: NgbModal,
        public settlementChangeService: SettlementChangeService,
        public translateService: TranslateService,
        public userDataService: UserDataService,
        private baseNomenclatureCodeResource: BaseNomenclatureCodeResource<Country, NomenclatureCodeFilterDto>,
        public configuration: Configuration
    ) {
        super(translateService, userDataService);
        if (this.router.getCurrentNavigation().extras?.state?.personLotNewFilterDto) {
            this.personLotNewFilterDto = this.router.getCurrentNavigation().extras?.state?.personLotNewFilterDto;
            this.personBasic.foreignerNumber = this.personLotNewFilterDto.foreignerNumber;
            this.personBasic.uin = this.personLotNewFilterDto.uin;
            this.personBasic.birthCountry = this.personLotNewFilterDto.birthCountry;
            this.personBasic.birthCountryId = this.personLotNewFilterDto.birthCountryId;
            this.personBasic.birthDate = this.personLotNewFilterDto.birthDate;
            this.personBasic.citizenship = this.personLotNewFilterDto.birthCountry;
            this.personBasic.citizenshipId = this.personLotNewFilterDto.birthCountryId;

            if (this.personBasic.uin) {
                this.setBirthDateGenderByUin(this.personBasic.uin, this.personBasic);
                this.baseNomenclatureCodeResource.getByCode('Country', 'BG')
                    .subscribe((country) => {
                        this.personBasic.birthCountry = country;
                        this.personBasic.birthCountryId = country.id;
                        this.personBasic.residenceCountry = country;
                        this.personBasic.residenceCountryId = country.id;
                        this.personBasic.citizenship = country;
                        this.personBasic.citizenshipId = country.id;
                    });
            }
        } else {
            this.router.navigate(['/rdpzsdNewSearch']);
        }
    }

    citizenshipChange(citizenship: Country) {
        if(citizenship === null){
            this.personBasic.secondCitizenship = null;
            this.personBasic.secondCitizenshipId = null;
        } else {
            this.personBasic.citizenship = citizenship;
            this.personBasic.citizenshipId = citizenship.id;
        }
    }

    createLot() {
        return this.personLotResource.createLot(this.personBasic)
            .subscribe(lot => {

                if (this.userDataService.isRsdUser() && !this.personBasic.uin && !this.personBasic.foreignerNumber) {
                    const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
                    modal.componentInstance.text = 'rdpzsd.personLot.modals.pendingApproval';
                    modal.componentInstance.declineButton = 'root.buttons.ok';
                    modal.componentInstance.declineButtonClass = 'btn-sm btn-outline-primary';
                    modal.componentInstance.infoOnly = true;
                }

                this.router.navigate(['/rdpzsdNewSearch']);
            });
    }

    cancel() {
        this.router.navigate(['/rdpzsdNewSearch']);
    }
}
