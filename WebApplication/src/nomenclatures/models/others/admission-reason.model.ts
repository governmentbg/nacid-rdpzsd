import { AdmissionReasonStudentType } from "src/nomenclatures/enums/admission-reason/admission-reason-student-type.enum";
import { CountryUnion } from "src/nomenclatures/enums/country/country-union.enum";
import { Nomenclature } from "../base/nomenclature.model";
import { AdmissionReasonEducationFee } from "./admission-reason-education-fee.model";
import { AdmissionReasonHistory } from "./admission-reason-history.model";
import { AdmissionReasonCitizenship } from "./citizenship-country.model";

export class AdmissionReason extends Nomenclature {
  oldCode: string;
  shortName: string;
  shortNameAlt: string;
  description: string;
  admissionReasonStudentType: AdmissionReasonStudentType;
  countryUnion: CountryUnion;
  admissionReasonEducationFees: AdmissionReasonEducationFee[] = [];
  admissionReasonHistories: AdmissionReasonHistory[] = [];
  admissionReasonCitizenships: AdmissionReasonCitizenship[] = [];

  constructor(){
    super();

    this.admissionReasonEducationFees.push(new AdmissionReasonEducationFee());
  }
}