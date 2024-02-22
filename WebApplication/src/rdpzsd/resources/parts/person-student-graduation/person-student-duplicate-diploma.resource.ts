import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { PersonStudentDuplicateDiplomaCreateDto } from "src/rdpzsd/dtos/parts/person-student-duplicate-diploma/person-student-duplicate-diploma-create.dto";
import { PersonStudentDuplicateDiploma } from "src/rdpzsd/models/parts/person-student/person-student-duplicate-diploma.model";

@Injectable()
export class PersonStudentDuplicateDiplomaResource {

    url = `${this.configuration.restUrl}PersonLot/PersonStudentDuplicateDiploma`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    create(newDuplicateDiplomaDto: PersonStudentDuplicateDiplomaCreateDto) {
        return this.http.post<PersonStudentDuplicateDiploma>(`${this.url}`, newDuplicateDiplomaDto);
    }

    update(updateDuplicateDiploma: PersonStudentDuplicateDiploma) {
        return this.http.put<PersonStudentDuplicateDiploma>(`${this.url}`, updateDuplicateDiploma);
    }

    invalid(duplicateDiplomaId: number) {
        return this.http.put<PersonStudentDuplicateDiploma>(`${this.url}/${duplicateDiplomaId}/Invalid`, null);
    }
}