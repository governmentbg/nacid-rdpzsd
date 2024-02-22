import { EntityVersion } from "src/shared/models/entity-version.model";
import { Language } from "../language.model";

export class InstitutionSpecialityLanguage extends EntityVersion {
  institutionSpecialityId: number;
  languageId: number;
  language: Language;
}