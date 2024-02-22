import { AttestationType } from "src/rdpzsd/enums/parts/attestation-type.enum";
import { YearType } from "src/rdpzsd/enums/parts/year-type.enum";
import { BasePersonSemester } from "../base/base-person-semester.model";

export class BasePersonDoctoralSemester extends BasePersonSemester {
    protocolDate: Date;
    protocolNumber: number;
    yearType: YearType;
    attestationType: AttestationType;
}

export class PersonDoctoralSemester extends BasePersonDoctoralSemester {
}