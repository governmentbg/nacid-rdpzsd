import { Country } from "src/nomenclatures/models/settlement/country.model";
import { PersonLot } from "src/rdpzsd/models/person-lot.model";

export class PersonLotDto {
    personLot: PersonLot;
    personBasicNames: string;
    personBasicNamesAlt: string;
    citizenship: Country;
    secondCitizenship: Country;
    hasActualStudentOrDoctoral: boolean;
    personSecondaryFromRso: boolean;
}