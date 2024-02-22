import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { PersonStudentStickerFilterDto } from "src/rdpzsd/dtos/search/person-student-sticker/person-student-sticker-filter.dto";
import { PersonStudentStickerSearchDto } from "src/rdpzsd/dtos/search/person-student-sticker/person-student-sticker-search.dto";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Injectable()
export class PersonStudentStickerSearchResource {

    url = `${this.configuration.restUrl}PersonStudentStickerSearch`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    getAll(filter: PersonStudentStickerFilterDto) {
        return this.http.post<SearchResultDto<PersonStudentStickerSearchDto>>(`${this.url}`, filter);
    }

    getAllCount(filter: PersonStudentStickerFilterDto) {
        return this.http.post<number>(`${this.url}/Count`, filter);
    }
}