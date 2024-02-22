import { Component, Input } from "@angular/core";
import { Period } from "src/nomenclatures/models/others/period.model";
import { StudentEvent, StudentEventGraduatedWithDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus, StudentStatusActive, StudentStatusGraduated, StudentStatusInterrupted } from "src/nomenclatures/models/student-status/student-status.model";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { Semester } from "src/shared/enums/semester.enum";

@Component({
    selector: 'person-semester-subheader',
    templateUrl: './person-semester-subheader.component.html'
})
export class PersonSemesterSubheaderComponent {

    courseType = CourseType;
    semesterType = Semester;
    studentStatusInterrupted = StudentStatusInterrupted;
    studentStatusActive = StudentStatusActive;
    studentStatusGraduated = StudentStatusGraduated;

    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;

    @Input() period: Period = null;
    @Input() studentStatus: StudentStatus = null;
    @Input() studentEvent: StudentEvent = null;
    @Input() studentSemester: Semester = null;
    @Input() studentCourse: CourseType = null;
}
