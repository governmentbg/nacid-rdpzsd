import { Nomenclature } from "../base/nomenclature.model";

export const ProfessionalBachelor = 'profBachelor';
export const Bachelor = 'bachelor';
export const MasterSecondary = 'masterSecondary';
export const MasterHigh = 'masterHigh';
export const Doctor = 'doctor';

export class EducationalQualification extends Nomenclature {
  alias: string;
}