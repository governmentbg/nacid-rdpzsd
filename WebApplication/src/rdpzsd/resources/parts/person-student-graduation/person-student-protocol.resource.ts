import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { PersonStudentProtocol } from "src/rdpzsd/models/parts/person-student/person-student-protocol.model";

@Injectable()
export class PersonStudentProtocolResource {

    url = `${this.configuration.restUrl}PersonLot/PersonStudentProtocol`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    create(newProtocol: PersonStudentProtocol) {
        return this.http.post<PersonStudentProtocol>(`${this.url}`, newProtocol);
    }

    update(updateProtocol: PersonStudentProtocol) {
        return this.http.put<PersonStudentProtocol>(`${this.url}`, updateProtocol);
    }

    delete(protocolId: number, partId: number) {
        return this.http.delete<void>(`${this.url}/${protocolId}/${partId}`);
    }
}