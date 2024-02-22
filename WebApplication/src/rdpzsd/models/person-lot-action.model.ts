import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { PersonLotActionType } from "src/rdpzsd/enums/person-lot-action-type.enum";
import { EntityVersion } from "src/shared/models/entity-version.model";

export class PersonLotAction extends EntityVersion {
    lotId: number;
    userId: number;
    userFullname: string;
    actionDate: Date;
    institutionId: number;
    institution: Institution;
    subordinateId: number;
    subordinate: Institution;
    actionType: PersonLotActionType;
    note: string;
}