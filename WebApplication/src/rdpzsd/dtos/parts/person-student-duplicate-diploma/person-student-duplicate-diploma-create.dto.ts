import { PersonStudentDuplicateDiploma } from "src/rdpzsd/models/parts/person-student/person-student-duplicate-diploma.model";
import { StickerDto } from "../person-student-sticker/sticker.dto";

export class PersonStudentDuplicateDiplomaCreateDto {
    stickerDto: StickerDto;
    newDuplicateDiploma: PersonStudentDuplicateDiploma;
}