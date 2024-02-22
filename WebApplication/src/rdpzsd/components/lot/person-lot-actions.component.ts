import { Component, Input } from "@angular/core";
import { PersonLotActionDto } from "src/rdpzsd/dtos/lot/person-lot-action.dto";
import { PersonLotActionType } from "src/rdpzsd/enums/person-lot-action-type.enum";
import { PersonLotResource } from "src/rdpzsd/resources/person-lot.resource";

@Component({
    selector: 'person-lot-actions',
    templateUrl: './person-lot-actions.component.html'
})
export class PersonLotActionsComponent {

    personLotActionDtos: PersonLotActionDto[] = [];
    personLotActionType = PersonLotActionType;

    loadingData = false;

    personLotId: number;
    @Input('personLotId')
    set personLotIdSetter(personLotId: number) {
        if (personLotId) {
            this.loadingData = true;
            this.personLotId = personLotId;
            this.resource.getLotActions(personLotId)
                .subscribe(e => {
                    this.personLotActionDtos = e;
                    this.loadingData = false;
                });
        } else {
            this.personLotId = null;
            this.personLotActionDtos = [];
        }
    }

    constructor(
        private resource: PersonLotResource
    ) {
    }
}