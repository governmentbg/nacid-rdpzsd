import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { NoteDto } from "src/shared/dtos/note.dto";
import { PersonLotActionDto } from "../dtos/lot/person-lot-action.dto";
import { PersonLotDto } from "../dtos/lot/person-lot.dto";
import { LotState } from "../enums/lot-state.enum";
import { PersonLotActionType } from "../enums/person-lot-action-type.enum";
import { PersonBasic } from "../models/parts/person-basic/person-basic.model";
import { PersonLotAction } from "../models/person-lot-action.model";
import { PersonLot } from "../models/person-lot.model";

@Injectable()
export class PersonLotResource {

    url = `${this.configuration.restUrl}PersonLot`;

    constructor(
        private http: HttpClient,
        private configuration: Configuration
    ) {
    }

    getLot(uan: string) {
        return this.http.get<PersonLotDto>(`${this.url}/${uan}`);
    }

    getLotState(id: number) {
        return this.http.get<LotState>(`${this.url}/${id}/State`);
    }

    createLot(personBasic: PersonBasic) {
        return this.http.post<PersonLot>(`${this.url}/CreateLot`, personBasic);
    }

    getLotActions(lotId: number) {
        return this.http.get<PersonLotActionDto[]>(`${this.url}/${lotId}/LotActions`);
    }

    getLatestPersonLotActionByType(lotId: number, actionType: PersonLotActionType) {
        return this.http.get<PersonLotAction>(`${this.url}/${lotId}/LatestPersonLotActionByType?actionType=${actionType}`);
    }

    cancelPendingApproval(lotId: number, noteDto: NoteDto) {
        return this.http.post<LotState>(`${this.url}/${lotId}/CancelPendingApproval`, noteDto);
    }

    sendForApproval(lotId: number) {
        return this.http.post<LotState>(`${this.url}/${lotId}/SendForApproval`, null);
    }

    approve(lotId: number) {
        return this.http.post<LotState>(`${this.url}/${lotId}/Approve`, null);
    }

    erase(lotId: number) {
        return this.http.post<LotState>(`${this.url}/${lotId}/Erase`, null);
    }
}