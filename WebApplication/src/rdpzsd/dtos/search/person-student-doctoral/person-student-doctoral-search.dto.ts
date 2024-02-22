import { InstitutionSpeciality } from "src/nomenclatures/models/institution/institution-speciality.model"
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { Period } from "src/nomenclatures/models/others/period.model";
import { StudentEvent } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { Semester } from "src/shared/enums/semester.enum";
import { PersonSearchDto } from "../person-search.dto"
import { YearType } from 'src/rdpzsd/enums/parts/year-type.enum';

export class PersonStudentDoctoralSearchDto extends PersonSearchDto {
    personSemesters: PersonSemesterSearchDto[] = [];
    facultyNumber: string;
}

export class PersonSemesterSearchDto {
    institution: Institution;
    institutionSpeciality: InstitutionSpeciality;
    protocolDate: Date;
    protocolNumber: string;
	yearType: YearType;
    period: Period;
    course: CourseType;
    studentSemester: Semester;
    studentStatus: StudentStatus;
    studentEvent: StudentEvent;
    diplomaYear: number;
}