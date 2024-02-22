import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Semester } from "src/shared/enums/semester.enum";
import { Period } from "../models/others/period.model";

@Injectable()
export class PeriodNomenclatureResource {

  latestPeriod: Period;

  constructor(
    private http: HttpClient
  ) {
  }

  getLatestPeriod() {
    return this.http.get<Period>(`api/Nomenclature/Period/Latest`)
      .subscribe(e => this.latestPeriod = e);
  }

  getNextPeriod(year: number, semester: Semester) {
    return this.http.get<Period>(`api/Nomenclature/Period/Next?year=${year}&semester=${semester}`);
  }

  getPreviousPeriod(year: number, semester: Semester) {
    return this.http.get<Period>(`api/Nomenclature/Period/Previous?year=${year}&semester=${semester}`);
  }
}