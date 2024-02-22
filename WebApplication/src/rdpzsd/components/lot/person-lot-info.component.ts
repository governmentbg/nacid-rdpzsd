import { Component } from '@angular/core';
import { LotState } from 'src/rdpzsd/enums/lot-state.enum';
import { CurrentPersonContextService } from 'src/rdpzsd/services/current-person-context.service';

@Component({
    selector: 'person-lot-info',
    templateUrl: './person-lot-info.component.html'
})
export class PersonLotInfoComponent {

    lotState = LotState;

    constructor(public currentPersonContextService: CurrentPersonContextService) {
    }

    personLotState = LotState;
}
