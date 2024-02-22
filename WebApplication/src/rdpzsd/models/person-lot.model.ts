import { EntityVersion } from "src/shared/models/entity-version.model";
import { LotState } from "../enums/lot-state.enum";

export class PersonLot extends EntityVersion {
    uan: string;
    state: LotState;
    createUserId: number;
    createDate: Date;
    createInstitutionId: number;
    createSubordinateId: number;
}