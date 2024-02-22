import { Component } from "@angular/core";
import { forkJoin } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { StudentEvent } from "src/nomenclatures/models/student-status/student-event.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { PersonDoctoralFilterDto } from "src/rdpzsd/dtos/parts/person-doctoral-filter.dto";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { PreviousEducationType } from "src/rdpzsd/enums/parts/previous-education-type.enum";
import { PreviousHighSchoolEducationType } from "src/rdpzsd/enums/parts/previous-high-school-education-type.enum";
import { YearType } from "src/rdpzsd/enums/parts/year-type.enum";
import { PersonDoctoralHistoryInfo } from "src/rdpzsd/models/parts/person-doctoral/history/person-doctoral-history-info.model";
import { PersonDoctoralHistory } from "src/rdpzsd/models/parts/person-doctoral/history/person-doctoral-history.model";
import { PersonDoctoralInfo } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral-info.model";
import { PersonDoctoralSemester } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral-semester.model";
import { PersonDoctoral } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral.model";
import { PartResource } from "src/rdpzsd/resources/parts/part.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { UserDataService } from "src/users/services/user-data.service";
import { BaseStudentDoctoralPartsComponent } from "../base/base-student-doctoral-parts.component";

@Component({
    selector: 'person-doctoral-parts',
    templateUrl: './person-doctoral-parts.component.html',
    providers: [
        PartResource,
        BaseNomenclatureResource
    ]
})
export class PersonDoctoralPartsComponent extends BaseStudentDoctoralPartsComponent<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonDoctoralFilterDto>{

    yearType = YearType;

    constructor(
        public userDataService: UserDataService,
        protected multiPartResource: PartResource<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralFilterDto>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        protected periodNomenclatureResource: PeriodNomenclatureResource,
        public currentPersonContext: CurrentPersonContextService,
        public configuration: Configuration
    ) {
        super(userDataService, multiPartResource, studentEventResource, periodNomenclatureResource, 'PersonDoctoral');
    }

    addPart() {
        return forkJoin([
            this.getStudentEventAlias(this.studentEventInitialRegistration)])
            .subscribe(([studentEvent]) => {
                const part = new PersonDoctoral();
                part.lotId = this.personLotId;
                part.state = PartState.actual;
                part.peType = PreviousEducationType.highSchool;
                part.pePart = this.currentPersonContext.singleGraduatedMasterPersonStudent;
                part.pePartId = this.currentPersonContext.singleGraduatedMasterPersonStudent?.id;

                if (part.pePartId) {
                    part.peHighSchoolType = PreviousHighSchoolEducationType.fromRegister;
                    part.peEducationalQualification = part.pePart?.institutionSpeciality?.speciality?.educationalQualification;
                    part.peEducationalQualificationId = part.pePart?.institutionSpeciality?.speciality?.educationalQualificationId;
                    part.peInstitution = part.pePart?.institution;
                    part.peInstitutionId = part.pePart?.institution?.id;
                    part.peInstitutionSpeciality = part.pePart?.institutionSpeciality;
                    part.peInstitutionSpecialityId = part.pePart?.institutionSpeciality?.id;
                    part.peResearchArea = part.pePart?.institutionSpeciality?.speciality?.researchArea;
                    part.peResearchAreaId = part.pePart?.institutionSpeciality?.speciality?.researchAreaId;
                    part.peSubordinate = part.pePart?.subordinate;
                    part.peSubordinateId = part.pePart?.subordinate?.id;
                    part.peDiplomaDate = part.pePart?.diploma?.diplomaDate;
                    part.peDiplomaNumber = part.pePart?.diploma?.diplomaNumber;
                }

                if (this.userDataService.isRsdUser()) {
                    part.institution = new Institution();
                    part.institutionId = this.userDataService.userData.institution.id;
                    part.institution.name = this.userDataService.userData.institution.name;
                    part.institution.nameAlt = this.userDataService.userData.institution.nameAlt;
                    part.institution.shortName = this.userDataService.userData.institution.shortName;
                    part.institution.shortNameAlt = this.userDataService.userData.institution.shortNameAlt;
                    part.institution.organizationType = this.userDataService.userData.institution.organizationType;
                }

                const semester = new PersonDoctoralSemester();
                semester.studentStatus = studentEvent.studentStatus;
                semester.studentStatusId = studentEvent.studentStatus.id;
                semester.studentEvent = studentEvent;
                semester.studentEventId = studentEvent.id;
                semester.protocolDate = new Date();
                semester.yearType = this.yearType.firstYear;
                part.semesters.unshift(semester);

                this.currentPersonContext.setIsInEdit(true);
                this.parts.unshift(part);
            });
    }
}