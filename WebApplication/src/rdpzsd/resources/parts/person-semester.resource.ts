import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { BasePersonSemester } from "src/rdpzsd/models/parts/base/base-person-semester.model";

@Injectable()
export class PersonSemesterResource<TSemester extends BasePersonSemester> {

    url = `${this.configuration.restUrl}PersonLot/`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    create(newSemester: TSemester) {
        return this.http.post<TSemester>(`${this.url}`, newSemester);
    }

    update(updateSemester: TSemester) {
        return this.http.put<TSemester>(`${this.url}`, updateSemester);
    }

    delete(semesterId: number, partId: number) {
        return this.http.delete<void>(`${this.url}/${semesterId}/${partId}`);
    }
}