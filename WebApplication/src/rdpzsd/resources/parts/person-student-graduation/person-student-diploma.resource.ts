import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { PersonStudentDiploma } from "src/rdpzsd/models/parts/person-student/person-student-diploma.model";

@Injectable()
export class PersonStudentDiplomaResource {

    url = `${this.configuration.restUrl}PersonLot/PersonStudentDiploma`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    create(newDiploma: PersonStudentDiploma) {
        return this.http.post<PersonStudentDiploma>(`${this.url}`, newDiploma);
    }

    update(updateDiploma: PersonStudentDiploma) {
        return this.http.put<PersonStudentDiploma>(`${this.url}`, updateDiploma);
    }

    invalid(partId: number) {
        return this.http.put<PersonStudentDiploma>(`${this.url}/${partId}/Invalid`, null);
    }
}