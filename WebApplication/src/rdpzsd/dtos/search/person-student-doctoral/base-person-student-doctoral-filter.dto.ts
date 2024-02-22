import { EducationalForm } from "src/nomenclatures/models/institution/educational-form.model";
import { EducationalQualification } from "src/nomenclatures/models/institution/educational-qualification.model";
import { InstitutionSpeciality } from "src/nomenclatures/models/institution/institution-speciality.model";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { Country } from "src/nomenclatures/models/settlement/country.model";
import { PersonStudentStatusType } from "src/rdpzsd/enums/search/person-student-status-type.enum";
import { FilterDto } from "src/shared/dtos/filter.dto";

export class BasePersonStudentDoctoralFilterDto extends FilterDto {
    studentStatus: PersonStudentStatusType = PersonStudentStatusType.activeInterrupted;
    uan: string;
    uin: string;
    foreignerNumber: string;
    idn: string;
    fullName: string;
    email: string;
    birthCountryId: number;
    birthCountry: Country;
    citizenshipId: number;
    citizenship: Country;
    institutionId: number;
    institution: Institution;
    institutionSpecialityId: number;
    institutionSpeciality: InstitutionSpeciality;
    educationalQualificationId: number;
    educationalQualification: EducationalQualification;
    educationalFormId: number;
    educationalForm: EducationalForm;
}