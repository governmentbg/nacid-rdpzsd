import { Component, Input } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AdmissionReasonStudentType } from "src/nomenclatures/enums/admission-reason/admission-reason-student-type.enum";
import { Doctor } from "src/nomenclatures/models/institution/educational-qualification.model";
import { PreviousEducationType } from "src/rdpzsd/enums/parts/previous-education-type.enum";
import { PreviousHighSchoolEducationType } from "src/rdpzsd/enums/parts/previous-high-school-education-type.enum";
import { PersonDoctoral } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral.model";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { ValidityDirective } from "src/shared/directives/validity-directive/validity.directive";
import { Level } from "src/shared/enums/level.enum";
import { InstitutionChangeService } from "src/shared/services/institutions/institution-change.service";
import { UserDataService } from "src/users/services/user-data.service";

@Component({
    selector: 'person-doctoral',
    templateUrl: './person-doctoral.component.html'
})
export class PersonDoctoralComponent extends ValidityDirective {

    doctorAlias = Doctor;
    studentType = AdmissionReasonStudentType;
    previousHighSchoolEducationType = PreviousHighSchoolEducationType;
    previousEducationType = PreviousEducationType;

    isHighSchoolNotFromRegister: boolean;

    personDoctoral: PersonDoctoral = new PersonDoctoral();
    @Input('personDoctoral')
    set personDoctoralSetter(personDoctoral: PersonDoctoral){
        this.personDoctoral = personDoctoral;
        if(personDoctoral.peType === this.previousEducationType.highSchool && personDoctoral.peHighSchoolType && personDoctoral.peHighSchoolType !== this.previousHighSchoolEducationType.fromRegister) {
            this.isHighSchoolNotFromRegister = true;
        }
    }

    level = Level;

    constructor(
        public institutionChangeService: InstitutionChangeService,
        public translateService: TranslateService,
        public userDataService: UserDataService,
        public currentPersonContext: CurrentPersonContextService,
    ) {
        super();
    }

    calculateEndDate(date: Date) {
        if (date && this.personDoctoral?.institutionSpeciality?.duration) {
            this.personDoctoral.endDate = new Date(date);
            this.personDoctoral.endDate.setFullYear(this.personDoctoral.endDate.getFullYear() + this.personDoctoral?.institutionSpeciality?.duration);
        } else {
            this.personDoctoral.endDate = null;
        }
    }

    setHighSchoolNotFromRegister(isHighSchoolNotFromRegister: boolean) {
        this.isHighSchoolNotFromRegister = isHighSchoolNotFromRegister;
    }
}