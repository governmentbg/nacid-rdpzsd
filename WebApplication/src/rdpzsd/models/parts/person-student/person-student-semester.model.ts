import { Period } from "src/nomenclatures/models/others/period.model";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { Semester } from "src/shared/enums/semester.enum";
import { CourseSemesterDto } from "src/shared/services/course-semester/course-semester.service";
import { BasePersonSemester } from "../base/base-person-semester.model";
import { Institution } from './../../../../nomenclatures/models/institution/institution.model';

export class BasePersonStudentSemester extends BasePersonSemester {
    periodId: number;
    period: Period;

    course: CourseType;
    studentSemester: Semester;
    // Specific field which can be true only if StudentEvent is TwoYearsForOne and its the second period from this event
    secondFromTwoYearsPlan = false;

    individualPlanCourse: CourseType;
    individualPlanSemester: Semester;

	semesterInstitutionId: number;
	semesterInstitution: Institution;
}

export class PersonStudentSemester extends BasePersonStudentSemester {
    // Client only
    courseSemester: CourseSemesterDto;
}