import { EntityVersion } from "src/shared/models/entity-version.model";
import { EducationalForm } from "./educational-form.model";
import { InstitutionSpecialityJointSpeciality } from "./institution-speciality-joint-speciality.model";
import { InstitutionSpecialityLanguage } from "./institution-speciality-language.model";
import { Institution } from "./institution.model";
import { NationalStatisticalInstitute } from "./national-statistical-institute.model";
import { Speciality } from "./speciality.model";

export class InstitutionSpeciality extends EntityVersion {
  institutionId: number;
  institution: Institution;
  specialityId: number;
  speciality: Speciality;
  educationalFormId: number;
  educationalForm: EducationalForm;
  nsiRegionId: number;
  nsiRegion: NationalStatisticalInstitute;
  nationalStatisticalInstituteId: number;
  nationalStatisticalInstitute: NationalStatisticalInstitute;
  duration: number;
  isAccredited: boolean;
  isForCadets: boolean;
  isActive: boolean;
  isJointSpeciality: boolean;
  organizationSpecialityLanguages: InstitutionSpecialityLanguage[] = [];
  institutionSpecialityJointSpecialities: InstitutionSpecialityJointSpeciality[] = [];
}