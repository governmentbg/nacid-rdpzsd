import { Component, Input, ViewChild } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AdmissionReasonStudentType } from "src/nomenclatures/enums/admission-reason/admission-reason-student-type.enum";
import { Doctor, EducationalQualification, MasterHigh } from "src/nomenclatures/models/institution/educational-qualification.model";
import { StudentStatusGraduated } from "src/nomenclatures/models/student-status/student-status.model";
import { PreviousEducationType } from "src/rdpzsd/enums/parts/previous-education-type.enum";
import { PreviousHighSchoolEducationType } from "src/rdpzsd/enums/parts/previous-high-school-education-type.enum";
import { StudentProtocolType } from "src/rdpzsd/enums/parts/student-protocol-type.enum";
import { PersonStudentInfo } from "src/rdpzsd/models/parts/person-student/person-student-info.model";
import { PersonStudentSemester } from "src/rdpzsd/models/parts/person-student/person-student-semester.model";
import { PersonStudent } from "src/rdpzsd/models/parts/person-student/person-student.model";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { ValidityDirective } from "src/shared/directives/validity-directive/validity.directive";
import { Level } from "src/shared/enums/level.enum";
import { InstitutionChangeService } from "src/shared/services/institutions/institution-change.service";
import { UserDataService } from "src/users/services/user-data.service";
import { PersonPreviousEducationComponent } from "../common/person-previous-education.component";
import { InstitutionSpecialityJointSpeciality } from './../../../../nomenclatures/models/institution/institution-speciality-joint-speciality.model';
import { CourseType } from 'src/rdpzsd/enums/parts/course.enum';
import { Semester } from "src/shared/enums/semester.enum";

@Component({
    selector: 'person-student',
    templateUrl: './person-student.component.html'
})
export class PersonStudentComponent extends ValidityDirective {

    educationalQualificationFilter: EducationalQualification = null;
    studentType = AdmissionReasonStudentType;

    isSecondaryFromRso: boolean;
    isHighSchoolNotFromRegister: boolean;

    personStudent: PersonStudent = new PersonStudent();
    @Input('personStudent')
    set personStudentSetter(personStudent: PersonStudent) {
        this.personStudent = personStudent;
        if (personStudent.peType === this.previousEducationType.highSchool && personStudent.peHighSchoolType && personStudent.peHighSchoolType !== this.previousHighSchoolEducationType.fromRegister) {
            this.isHighSchoolNotFromRegister = true;
        }
    }

    @ViewChild('personPreviousEducation') personPreviousEducationComponent: PersonPreviousEducationComponent<PersonStudent, PersonStudentInfo, PersonStudentSemester>;

    level = Level;
    masterHigh = MasterHigh;
    doctor = Doctor;
    previousEducationType = PreviousEducationType;
    previousHighSchoolEducationType = PreviousHighSchoolEducationType;
    studentProtocolType = StudentProtocolType;

    studentStatusGraduated = StudentStatusGraduated;

    courseEnum = CourseType;
    semesterEnum = Semester;

    constructor(
        public institutionChangeService: InstitutionChangeService,
        public translateService: TranslateService,
        public userDataService: UserDataService,
        public currentPersonContext: CurrentPersonContextService
    ) {
        super();
    }

    educationalQualificationFilterChange(educationalQualification: EducationalQualification) {
        this.personStudent.institutionSpeciality = null;
        this.personStudent.institutionSpecialityId = null;

        if (educationalQualification && educationalQualification.alias === this.masterHigh) {
            this.personStudent.peType = this.previousEducationType.highSchool;

            if (this.personPreviousEducationComponent != null) {
                this.personPreviousEducationComponent.fromRegisterSelected(this.currentPersonContext.singleGraduatedPersonStudent);
            }

            if (this.personStudent.pePartId) {
                this.personStudent.peHighSchoolType = PreviousHighSchoolEducationType.fromRegister;
            } else {
                this.personStudent.peHighSchoolType = null;
            }
        } else {
            this.personStudent.peType = this.previousEducationType.secondary;
            this.personStudent.pePart = null;
            this.personStudent.pePartId = null;
        }
    }
    institutionSpecialityJointSpecialitiesByLocation(jointSpecialities: any[], location: number): InstitutionSpecialityJointSpeciality[] {
        return jointSpecialities.filter(e => e.location == location);
    }
    institutionSpecialityChanged() {
        if (this.personStudent?.institutionSpeciality?.isJointSpeciality && this.personStudent?.institutionSpeciality?.institutionSpecialityJointSpecialities?.length > 0) {
            let firstSemester = this.personStudent.semesters.find(e => e.course == this.courseEnum.first && e.studentSemester == this.semesterEnum.first)
            if (firstSemester) {
                firstSemester.semesterInstitution = this.personStudent.institution;
                firstSemester.semesterInstitutionId = this.personStudent.institutionId;
            }
        }
    }

    setSecondaryFromRso(isSecondaryFromRso: boolean) {
        this.isSecondaryFromRso = isSecondaryFromRso;
    }

    setHighSchoolNotFromRegister(isHighSchoolNotFromRegister: boolean) {
        this.isHighSchoolNotFromRegister = isHighSchoolNotFromRegister;
    }
}