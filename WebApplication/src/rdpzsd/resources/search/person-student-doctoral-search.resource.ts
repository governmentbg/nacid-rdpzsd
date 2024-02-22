import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { BasePersonStudentDoctoralFilterDto } from "src/rdpzsd/dtos/search/person-student-doctoral/base-person-student-doctoral-filter.dto";
import { PersonStudentDoctoralSearchDto } from "src/rdpzsd/dtos/search/person-student-doctoral/person-student-doctoral-search.dto";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Injectable()
export class PersonStudentDoctoralSearchResource<TFilter extends BasePersonStudentDoctoralFilterDto> {

    url = `${this.configuration.restUrl}`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    getAll(filter: TFilter) {
        return this.http.post<SearchResultDto<PersonStudentDoctoralSearchDto>>(`${this.url}`, filter);
    }

    getAllCount(filter: TFilter) {
        return this.http.post<number>(`${this.url}/Count`, filter);
    }
}