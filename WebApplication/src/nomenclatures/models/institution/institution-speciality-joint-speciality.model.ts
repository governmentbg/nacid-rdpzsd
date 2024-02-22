import { Institution } from 'src/nomenclatures/models/institution/institution.model';

export class InstitutionSpecialityJointSpeciality {
	institutionSpecialityId: number;
	location: InstitutionSpecialityJointSpecialityLocation;
	institutionId: number;
	institution: Institution;
	institutionByParentId: number;
	institutionByParent: Institution;
	foreignInstitutionName: string;
	foreignInstitutionByParentName: string;
}
export enum InstitutionSpecialityJointSpecialityLocation {
	Bulgaria = 1,
	Abroad = 2
}