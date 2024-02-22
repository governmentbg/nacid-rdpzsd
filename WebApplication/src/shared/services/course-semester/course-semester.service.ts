import { Injectable } from "@angular/core";
import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { Semester } from "src/shared/enums/semester.enum";

export class CourseSemesterDto {
    name: string;

    course: CourseType;
    semester: Semester;

    constructor(name: string, course: CourseType, semester: Semester) {
        this.name = name;
        this.course = course;
        this.semester = semester;
    }
}

@Injectable()
export class CourseSemesterCollectionService {
    items: CourseSemesterDto[] = [];

    semester = Semester;

    constructCollection() {
        this.items = [];

        for (let course in CourseType) {
            if (isNaN(Number(course))) {
                var courseEnum: CourseType = CourseType[course as keyof typeof CourseType];
                const courseSemesterFirst = new CourseSemesterDto(`enums.courseTypeSemester.${course}Course_first`, courseEnum, this.semester.first);
                this.items.push(courseSemesterFirst);
                const courseSemesterSecond = new CourseSemesterDto(`enums.courseTypeSemester.${course}Course_second`, courseEnum, this.semester.second);
                this.items.push(courseSemesterSecond);
            }
        }
    }
}