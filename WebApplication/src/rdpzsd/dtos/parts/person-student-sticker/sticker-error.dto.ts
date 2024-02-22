import { CourseType } from "src/rdpzsd/enums/parts/course.enum";
import { Semester } from "src/shared/enums/semester.enum";

export class StickerErrorDto {
    hasError = false;

    missingStudentSemesters: MissingStudentSemesterDto[] = [];
    otherErrors: CustomStickerErrorDto[] = [];
}

export class CustomStickerErrorDto {
    error: string;
    errorAlt: string;
}

export class MissingStudentSemesterDto {
    course: CourseType;
    studentSemester: Semester;
}