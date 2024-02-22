import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Configuration } from "src/app/configuration/configuration";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";
import { InstitutionSpecialityFilterDto } from "../dtos/institution/institution-speciality-filter.dto";
import { InstitutionSpeciality } from "../models/institution/institution-speciality.model";

@Injectable()
export class InstitutionSpecialityNomenclatureResource {

  constructor(
    private http: HttpClient,
    private configuration: Configuration
  ) {
  }

  getAll(filter: InstitutionSpecialityFilterDto) {
    return this.http.post<SearchResultDto<InstitutionSpeciality>>(`${this.configuration.restUrl}Nomenclature/InstitutionSpeciality`, filter);
  }

  exportExcel(filter: InstitutionSpecialityFilterDto): Observable<Blob> {
    return this.http.post(`${this.configuration.restUrl}Nomenclature/InstitutionSpeciality/Excel`, filter, { responseType: 'blob' })
      .pipe(
        map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
      );
  }
}