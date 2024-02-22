import { Component } from "@angular/core";
import { forkJoin } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { StudentEvent, StudentEventGraduatedWithDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { PersonStudentFilterDto } from "src/rdpzsd/dtos/parts/person-student-filter.dto";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { PreviousEducationType } from "src/rdpzsd/enums/parts/previous-education-type.enum";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentHistoryInfo } from "src/rdpzsd/models/parts/person-student/history/person-student-history-info.model";
import { PersonStudentHistory } from "src/rdpzsd/models/parts/person-student/history/person-student-history.model";
import { PersonStudentInfo } from "src/rdpzsd/models/parts/person-student/person-student-info.model";
import { PersonStudentSemester } from "src/rdpzsd/models/parts/person-student/person-student-semester.model";
import { PersonStudent } from "src/rdpzsd/models/parts/person-student/person-student.model";
import { PartResource } from "src/rdpzsd/resources/parts/part.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { Semester } from "src/shared/enums/semester.enum";
import { CourseSemesterDto } from "src/shared/services/course-semester/course-semester.service";
import { UserDataService } from "src/users/services/user-data.service";
import { BaseStudentDoctoralPartsComponent } from "../base/base-student-doctoral-parts.component";

@Component({
    selector: 'person-student-parts',
    templateUrl: './person-student-parts.component.html',
    providers: [
        PartResource,
        BaseNomenclatureResource
    ]
})
export class PersonStudentPartsComponent extends BaseStudentDoctoralPartsComponent<PersonStudent, PersonStudentInfo, PersonStudentSemester, PersonStudentHistory, PersonStudentHistoryInfo, PersonStudentFilterDto>{

    constructor(
        public userDataService: UserDataService,
        protected multiPartResource: PartResource<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentFilterDto>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        protected periodNomenclatureResource: PeriodNomenclatureResource,
        public currentPersonContext: CurrentPersonContextService,
        public configuration: Configuration
    ) {
        super(userDataService, multiPartResource, studentEventResource, periodNomenclatureResource, 'PersonStudent')
    }

    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;
    studentStickerState = StudentStickerState;

    addPart() {
        return forkJoin([
            this.getStudentEventAlias(this.studentEventInitialRegistration)])
            .subscribe(([studentEvent]) => {
                const part = new PersonStudent();
                part.lotId = this.personLotId;
                part.state = PartState.actual;
                part.peType = PreviousEducationType.secondary;

                if (this.userDataService.isRsdUser()) {
                    part.institution = new Institution();
                    part.institutionId = this.userDataService.userData.institution.id;
                    part.institution.name = this.userDataService.userData.institution.name;
                    part.institution.nameAlt = this.userDataService.userData.institution.nameAlt;
                    part.institution.shortName = this.userDataService.userData.institution.shortName;
                    part.institution.shortNameAlt = this.userDataService.userData.institution.shortNameAlt;
                    part.institution.organizationType = this.userDataService.userData.institution.organizationType;
                }

                const semester = new PersonStudentSemester();
                semester.studentStatus = studentEvent.studentStatus;
                semester.studentStatusId = studentEvent.studentStatus.id;
                semester.studentEvent = studentEvent;
                semester.studentEventId = studentEvent.id;
                semester.course = CourseType.first;
                semester.studentSemester = Semester.first;
                semester.period = this.periodNomenclatureResource?.latestPeriod;
                semester.periodId = this.periodNomenclatureResource?.latestPeriod?.id;
                const courseSemester = new CourseSemesterDto('enums.courseTypeSemester.firstCourse_first', CourseType.first, Semester.first);
                semester.courseSemester = courseSemester;
                semester.semesterInstitution = part.institution;
                semester.semesterInstitutionId = part.institutionId;
                part.semesters.unshift(semester);

                this.currentPersonContext.setIsInEdit(true);
                this.parts.unshift(part);
            });
    }
}