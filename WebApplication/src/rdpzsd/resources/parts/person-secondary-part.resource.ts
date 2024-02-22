import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { PersonSecondaryHistory } from "src/rdpzsd/models/parts/person-secondary/history/person-secondary-history.model";
import { PersonSecondaryInfo } from "src/rdpzsd/models/parts/person-secondary/person-secondary-info.model";
import { PersonSecondary } from "src/rdpzsd/models/parts/person-secondary/person-secondary.model";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { PartResource } from "./part.resource";

@Injectable()
export class PersonSecondaryPartResource extends PartResource<PersonSecondary, PersonSecondaryInfo, PersonSecondaryHistory, FilterDto>{

    constructor(
        protected http: HttpClient,
        protected configuration: Configuration
    ) {
        super(http, configuration);
        this.init('PersonSecondary');
    }

    getFromRso(personLotId: number) {
        return this.http.get<PersonSecondary>(`${this.url}${personLotId}/GetFromRso`);
    }

    getImagesFromRso(rsoIntId: number): Observable<Blob>{
        return this.http.get(`${this.url}${rsoIntId}/GetImagesFromRso`, { responseType: 'blob'})
            .pipe(
                map(response => new Blob([response], {type: 'application/pdf '}))
            );
    }
}