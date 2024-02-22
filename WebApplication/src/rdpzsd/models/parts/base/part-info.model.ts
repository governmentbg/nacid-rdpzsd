import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { EntityVersion } from "src/shared/models/entity-version.model";

export class PartInfo extends EntityVersion {
    userId: number;
    userFullname: string;
    actionDate: Date;
    institutionId: number;
    institution: Institution;
    subordinateId: number;
    subordinate: Institution;
}