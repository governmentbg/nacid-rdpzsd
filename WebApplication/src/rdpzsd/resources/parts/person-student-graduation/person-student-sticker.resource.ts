import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { StickerErrorDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker-error.dto";
import { StickerDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker.dto";
import { PersonStudentStickerSearchDto } from "src/rdpzsd/dtos/search/person-student-sticker/person-student-sticker-search.dto";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";

@Injectable()
export class PersonStudentStickerResource {

    url = `${this.configuration.restUrl}PersonLot/PersonStudentSticker`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    sendForSticker(partId: number, stickerDto: StickerDto) {
        return this.http.put<StudentStickerState>(`${this.url}/${partId}/SendForSticker`, stickerDto);
    }

    returnForEdit(partId: number, stickerDto: StickerDto) {
        return this.http.put<StudentStickerState>(`${this.url}/${partId}/ReturnForEdit`, stickerDto);
    }

    markedForPrint(markedForPrintIds: number[]) {
        return this.http.put<void>(`${this.url}/MarkedForPrint`, markedForPrintIds);
    }

    markedRecieved(markedRecievedIds: number[]) {
        return this.http.put<void>(`${this.url}/MarkedRecieved`, markedRecievedIds);
    }

    forPrint(partId: number) {
        return this.http.put<PersonStudentStickerSearchDto>(`${this.url}/${partId}/ForPrint`, null);
    }

    forPrintDuplicate(duplicateDiplomaId: number) {
        return this.http.put<StudentStickerState>(`${this.url}/${duplicateDiplomaId}/ForPrintDuplicate`, null);
    }

    recieved(partId: number) {
        return this.http.put<StudentStickerState>(`${this.url}/${partId}/Recieved`, null);
    }

    recievedDuplicate(duplicateDiplomaId: number) {
        return this.http.put<StudentStickerState>(`${this.url}/${duplicateDiplomaId}/RecievedDuplicate`, null);
    }

    reissueSticker(partId: number, stickerDto: StickerDto) {
        return this.http.put<StudentStickerState>(`${this.url}/${partId}/ReissueSticker`, stickerDto);
    }

    validatePersonStudentStickerInfo(partId: number) {
        return this.http.get<StickerErrorDto>(`${this.url}/${partId}/ValidatePersonStudentStickerInfo`);
    }
}