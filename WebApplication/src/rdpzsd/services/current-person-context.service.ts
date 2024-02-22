import { Injectable } from "@angular/core";
import { MasterHigh, MasterSecondary } from "src/nomenclatures/models/institution/educational-qualification.model";
import { Country } from "src/nomenclatures/models/settlement/country.model";
import { PersonLotDto } from "../dtos/lot/person-lot.dto";
import { PersonStudentStickerSearchDto } from "../dtos/search/person-student-sticker/person-student-sticker-search.dto";
import { LotState } from "../enums/lot-state.enum";
import { PersonBasic } from "../models/parts/person-basic/person-basic.model";
import { PersonStudent } from "../models/parts/person-student/person-student.model";

@Injectable()
export class CurrentPersonContextService {
    uan: string;
    personLotState: LotState;
    personLotId: number;
    personBasicNames: string;
    personBasicNamesAlt: string;
    citizenship: Country;
    secondCitizenship: Country;
    hasActualStudentOrDoctoral: boolean = true;

    graduatedPersonStudentsCount: number;
    singleGraduatedPersonStudent: PersonStudent;
    graduatedMasterPersonStudentsCount: number;
    singleGraduatedMasterPersonStudent: PersonStudent;

    // This is not null only if opened from stickers search
    openedFromStickers = false;
    personStudentStickerDto: PersonStudentStickerSearchDto = null;

    isInEdit = false;

    setIsInEdit(isInEdit: boolean) {
        this.isInEdit = isInEdit;
    }

    setFromLot(personLotDto: PersonLotDto, personStudentStickerDto: PersonStudentStickerSearchDto) {
        this.uan = personLotDto.personLot.uan;
        this.personLotId = personLotDto.personLot.id;
        this.personLotState = personLotDto.personLot.state;
        this.personBasicNames = personLotDto.personBasicNames;
        this.personBasicNamesAlt = personLotDto.personBasicNamesAlt;
        this.citizenship = personLotDto.citizenship;
        this.secondCitizenship = personLotDto.secondCitizenship;
        this.hasActualStudentOrDoctoral = personLotDto.hasActualStudentOrDoctoral;
        this.personStudentStickerDto = personStudentStickerDto;
        this.openedFromStickers = personStudentStickerDto !== null;
    }

    setFromPersonBasic(personBasic: PersonBasic, lotState: LotState) {
        this.personBasicNames = personBasic?.fullName;
        this.personBasicNamesAlt = personBasic?.fullNameAlt;
        this.citizenship = personBasic?.citizenship;
        this.secondCitizenship = personBasic.secondCitizenship;
        this.personLotState = lotState;
    }

    setGraduatedPersonStudents(graduatedPersonStudents: PersonStudent[]) {
        this.graduatedPersonStudentsCount = graduatedPersonStudents?.length;
        this.graduatedMasterPersonStudentsCount = graduatedPersonStudents
            ?.filter(e => e.institutionSpeciality.speciality.educationalQualification.alias === MasterHigh || e.institutionSpeciality.speciality.educationalQualification.alias === MasterSecondary)?.length;
        this.singleGraduatedPersonStudent = graduatedPersonStudents?.length === 1 ? graduatedPersonStudents[0] : null;
        this.singleGraduatedMasterPersonStudent = graduatedPersonStudents
            ?.filter(e => e.institutionSpeciality.speciality.educationalQualification.alias === MasterHigh || e.institutionSpeciality.speciality.educationalQualification.alias === MasterSecondary)?.length === 1
            ? graduatedPersonStudents.filter(e => e.institutionSpeciality.speciality.educationalQualification.alias === MasterHigh || e.institutionSpeciality.speciality.educationalQualification.alias === MasterSecondary)[0] : null;
    }
}