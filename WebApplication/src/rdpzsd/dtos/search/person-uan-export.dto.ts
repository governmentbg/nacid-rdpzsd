import { Country } from "src/nomenclatures/models/settlement/country.model";
import { GenderType } from "src/rdpzsd/enums/parts/gender-type.enum";

export class PersonUanExportDto{
    lotId: number;
    uan: string;
  
    uin: string;
    foreignerNumber: string;
    personIdn: string;
  
    firstName: string;
    middleName: string;
    lastName: string;
    firstNameAlt: string;
    middleNameAlt: string;
    lastNameAlt: string;
  
    gender: GenderType;
    birthDate: Date;
  
    birthCountry: Country;
    foreignerBirthSettlement: string;
}