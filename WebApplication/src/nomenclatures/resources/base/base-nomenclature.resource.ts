import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { Nomenclature } from "src/nomenclatures/models/base/nomenclature.model";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Injectable()
export class BaseNomenclatureResource<T extends Nomenclature, TFilter extends NomenclatureFilterDto> {

  constructor(
    protected http: HttpClient,
    protected configuration: Configuration
  ) {
  }

  getAll(nomenclatureRoute: string, filter: TFilter) {
    return this.http.post<SearchResultDto<T>>(`${this.configuration.restUrl}Nomenclature/${nomenclatureRoute}`, filter);
  }

  getByAlias(nomenclatureRoute: string, alias: string) {
    return this.http.get<T>(`${this.configuration.restUrl}Nomenclature/${nomenclatureRoute}/Alias?alias=${alias}`);
  }

  exportExcel(nomenclatureRoute: string, filter: TFilter): Observable<Blob> {
    return this.http.post(`${this.configuration.restUrl}Nomenclature/${nomenclatureRoute}/Excel`, filter, { responseType: 'blob' })
      .pipe(
        map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
      );
  }

  create(nomenclatureRoute: string, entity: T) {
    return this.http.post<T>(`${this.configuration.restUrl}Nomenclature/${nomenclatureRoute}/Create`, entity);
  }

  update(nomeclatureRoute: string, entity: T) {
    return this.http.put<T>(`${this.configuration.restUrl}Nomenclature/${nomeclatureRoute}`, entity);
  }

  delete(nomenclatureRoute: string, id: number) {
    return this.http.post(`${this.configuration.restUrl}Nomenclature/${nomenclatureRoute}/Delete`, id);
  }
}