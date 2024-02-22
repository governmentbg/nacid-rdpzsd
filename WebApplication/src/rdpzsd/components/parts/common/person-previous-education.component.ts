import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { Configuration } from "src/app/configuration/configuration";
import { OrganizationType } from "src/nomenclatures/enums/institution/organization-type.enum";
import { Doctor } from "src/nomenclatures/models/institution/educational-qualification.model";
import { InstitutionSpeciality } from "src/nomenclatures/models/institution/institution-speciality.model";
import { StudentEventGraduatedWithDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatusGraduated } from "src/nomenclatures/models/student-status/student-status.model";
import { PreviousEducationType } from "src/rdpzsd/enums/parts/previous-education-type.enum";
import { PreviousHighSchoolEducationType } from "src/rdpzsd/enums/parts/previous-high-school-education-type.enum";
import { BasePersonSemester } from "src/rdpzsd/models/parts/base/base-person-semester.model";
import { BasePersonStudentDoctoral } from "src/rdpzsd/models/parts/base/base-person-student-doctoral.model";
import { PartInfo } from "src/rdpzsd/models/parts/base/part-info.model";
import { PersonSecondary } from "src/rdpzsd/models/parts/person-secondary/person-secondary.model";
import { PersonStudent } from "src/rdpzsd/models/parts/person-student/person-student.model";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { Level } from "src/shared/enums/level.enum";
import { InstitutionChangeService } from "src/shared/services/institutions/institution-change.service";

@Component({
    selector: 'person-previous-education',
    templateUrl: './person-previous-education.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class PersonPreviousEducationComponent<TPart extends BasePersonStudentDoctoral<TPartInfo, TSemester>, TPartInfo extends PartInfo, TSemester extends BasePersonSemester> {

    @Input() part: TPart;
    @Input() isEditMode = false;
    @Input() disabledPeType: boolean = false;
    @Input() isDoctoral: boolean = false;

    @Output() personSecondaryEventEmitter: EventEmitter<PersonSecondary> = new EventEmitter<PersonSecondary>();
    @Output() secondaryFromRsoEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() highSchoolNotFromRegisterEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();

    organizationType = OrganizationType;
    previousEducationType = PreviousEducationType;
    previousHighSchoolEducationType = PreviousHighSchoolEducationType;
    doctor = Doctor;
    level = Level;
    studentStatusGraduated = StudentStatusGraduated;
    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;

    personSecondary: PersonSecondary = null;

    constructor(
        public institutionChangeService: InstitutionChangeService,
        public currentPersonContext: CurrentPersonContextService,
        public configuration: Configuration
    ) {
    }

    peTypeChanged(peType: PreviousEducationType) {
        if (peType === PreviousEducationType.highSchool) {
            this.fromRegisterSelected(this.currentPersonContext.singleGraduatedPersonStudent);

            if (this.part.pePartId || this.currentPersonContext.graduatedPersonStudentsCount > 1) {
                this.part.peHighSchoolType = this.previousHighSchoolEducationType.fromRegister;
            } else {
                this.part.peHighSchoolType = null;
            }
        } else {
            this.part.peHighSchoolType = null;
            this.fromRegisterSelected(null);
        }

        this.setHighSchoolNotFromRegisterEmitter();
        this.setPersonSecondaryEmitter(this.personSecondary);
    }

    peHighSchoolTypeChanged(peHighSchoolType: PreviousHighSchoolEducationType) {
        if (peHighSchoolType === this.previousHighSchoolEducationType.fromRegister) {
            if (this.isDoctoral) {
                this.fromRegisterSelected(this.currentPersonContext.singleGraduatedMasterPersonStudent);
            } else {
                this.fromRegisterSelected(this.currentPersonContext.singleGraduatedPersonStudent);
            }
        } else {
            this.fromRegisterSelected(null);
        }

        this.setHighSchoolNotFromRegisterEmitter();
    }

    fromRegisterSelected(personStudent: PersonStudent) {
        this.part.peEducationalQualification = personStudent?.institutionSpeciality?.speciality?.educationalQualification;
        this.part.peEducationalQualificationId = personStudent?.institutionSpeciality?.speciality?.educationalQualificationId;
        this.part.pePart = personStudent;
        this.part.pePartId = personStudent?.id;
        this.part.peInstitution = personStudent?.institution;
        this.part.peInstitutionId = personStudent?.institution?.id;
        this.part.peInstitutionSpeciality = personStudent?.institutionSpeciality;
        this.part.peInstitutionSpecialityId = personStudent?.institutionSpeciality?.id;
        this.part.peResearchArea = personStudent?.institutionSpeciality?.speciality?.researchArea;
        this.part.peResearchAreaId = personStudent?.institutionSpeciality?.speciality?.researchAreaId;
        this.part.peSubordinate = personStudent?.subordinate;
        this.part.peSubordinateId = personStudent?.subordinate?.id;
        this.part.peAcquiredSpeciality = null;
        this.part.peCountry = null;
        this.part.peCountryId = null;
        this.part.peInstitutionName = null;
        this.part.peSpecialityMissingInRegister = null;
        this.part.peRecognizedSpeciality = null;
        this.part.peSpecialityName = null;
        this.part.peRecognitionDate = null;
        this.part.peRecognitionNumber = null;
        this.part.peRecognitionDocument = null;
        this.part.peAcquiredForeignEducationalQualification = null;
        this.part.peAcquiredForeignEducationalQualificationId = null;
        this.part.peDiplomaDate = personStudent?.diploma?.diplomaDate;
        this.part.peDiplomaNumber = personStudent?.diploma?.diplomaNumber;
    }

    peInstitutionSpecialitySpecialityChanged(peInstitutionSpeciality: InstitutionSpeciality) {
        this.part.peInstitutionSpecialityId = peInstitutionSpeciality?.id;
        this.part.peResearchArea = peInstitutionSpeciality?.speciality?.researchArea;
        this.part.peResearchAreaId = peInstitutionSpeciality?.speciality.researchAreaId;
        this.part.peEducationalQualification = peInstitutionSpeciality?.speciality?.educationalQualification;
        this.part.peEducationalQualificationId = peInstitutionSpeciality?.speciality?.educationalQualificationId;

        if (peInstitutionSpeciality && peInstitutionSpeciality.institution?.parentId) {
            this.institutionChangeService.subordinateChange(this.part, peInstitutionSpeciality.institution, 'peSubordinate', 'peInstitution')
        }
    }

    specialityFromRegisterChanged() {
        this.part.peInstitutionSpeciality = null;
        this.part.peInstitutionSpecialityId = null;
        this.part.peSpecialityName = null;
        this.part.peResearchArea = null;
        this.part.peResearchAreaId = null;
        this.part.peEducationalQualification = null;
        this.part.peEducationalQualificationId = null;
    }

    setHighSchoolNotFromRegisterEmitter() {
        if(this.part.peType === this.previousEducationType.highSchool && this.part.peHighSchoolType && this.part.peHighSchoolType !== this.previousHighSchoolEducationType.fromRegister) {
            this.highSchoolNotFromRegisterEmitter.emit(true);
        } else {
            this.highSchoolNotFromRegisterEmitter.emit(false);
        }
    }

    setPersonSecondaryEmitter(personSecondary: PersonSecondary) {
        this.personSecondary = personSecondary;

        if(this.part.peType === this.previousEducationType.secondary && personSecondary && !personSecondary.fromRso){
            this.secondaryFromRsoEmitter.emit(true);
        } else {
            this.secondaryFromRsoEmitter.emit(false);
        }
    }
}