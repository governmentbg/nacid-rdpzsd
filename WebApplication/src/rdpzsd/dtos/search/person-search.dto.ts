import { Country } from "src/nomenclatures/models/settlement/country.model";
import { Settlement } from "src/nomenclatures/models/settlement/settlement.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { LotState } from "../../enums/lot-state.enum";
import { GenderType } from "../../enums/parts/gender-type.enum";
import { PersonSemesterSearchDto } from "./person-student-doctoral/person-student-doctoral-search.dto";

export class PersonSearchDto {
    id: number;
    uan: string;
    state: LotState;

    uin: string;
    foreignerNumber: string;
    personIdn: string;

    fullName: string;
    fullNameAlt: string;

    gender: GenderType;
    birthDate: Date;

    email: string;
    phoneNumber: string;

    birthCountry: Country;
    birthSettlement: Settlement;
    citizenship: Country;
    secondCitizenship: Country;

    hasSpeciality: boolean;
    hasSecondary: boolean;

    studentStatuses: StudentStatus[] = [];
    personSemesters: PersonSemesterSearchDto[] = [];
}