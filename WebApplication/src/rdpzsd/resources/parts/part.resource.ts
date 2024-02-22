import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { PartHistoryDto } from "src/rdpzsd/dtos/parts/part-history.dto";
import { PartInfo } from "src/rdpzsd/models/parts/base/part-info.model";
import { Part } from "src/rdpzsd/models/parts/base/part.model";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Injectable()
export class PartResource<TEntity extends Part<TPartInfo>, TPartInfo extends PartInfo, THistory, TFilter extends FilterDto> {

    url = `${this.configuration.restUrl}PersonLot/`;

    constructor(
        protected http: HttpClient,
        protected configuration: Configuration
    ) {
    }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}/`;
    }

    getSinglePart(lotId: number) {
        return this.http.get<TEntity>(this.url + lotId);
    }

    getMultiParts(lotId: number) {
        return this.http.get<TEntity[]>(this.url + lotId);
    }

    create(entity: TEntity) {
        return this.http.post<TEntity>(`${this.url}Create`, entity);
    }

    update(entity: TEntity) {
        return this.http.put<TEntity>(`${this.url}Update`, entity);
    }

    erase(partId: number) {
        return this.http.delete<TEntity>(`${this.url}Erase/${partId}`);
    }

    getHistory(id: number) {
        return this.http.get<PartHistoryDto<TEntity, THistory>>(`${this.url}${id}/History`);
    }

    getSearchDto(filter: TFilter) {
        return this.http.post<SearchResultDto<TEntity>>(`${this.url}SearchDto`, filter);
    }
}