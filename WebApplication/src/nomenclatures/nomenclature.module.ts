import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { InstitutionDoctoralSearchComponent } from "./components/institution/institution-doctoral-search.component";
import { InstitutionSearchComponent } from "./components/institution/institution-search.component";
import { InstitutionSpecialitySearchComponent } from "./components/institution/institution-speciality-search.component";
import { SubordinateSearchComponent } from "./components/institution/subordinate-search.component";
import { AdmissionReasonSearchComponent } from "./components/others/admission-reason-search.component";
import { CountrySearchComponent } from "./components/settlement/country-search.component";
import { DistrictSearchComponent } from "./components/settlement/district-search.component";
import { MunicipalitySearchComponent } from "./components/settlement/municipality-search.component";
import { SettlementSearchComponent } from "./components/settlement/settlement-search.component";
import { InstitutionTabsComponent } from "./components/tabs/institution-tabs.component";
import { OthersTabsComponent } from "./components/tabs/others-tabs.component";
import { SettlementTabsComponent } from "./components/tabs/settlement-tabs.component";
import { NomenclatureRoutingModule } from "./nomenclature.routing";
import { InstitutionSpecialityNomenclatureResource } from "./resources/institution-speciality-nomenclature.resource";
import { EducationFeeTypeSearchComponent } from './components/others/education-fee-type-search.component';
import { SpecialityTabsComponent } from './components/tabs/speciality-tabs.component';
import { AdmissionReasonHistoryComponent } from "./components/others/admission-reason-history.component";
import { StudentStatusTabsComponent } from "./components/tabs/student-status-tabs.component";
import { StudentStatusSearchComponent } from "./components/student-status/student-status-search.component";
import { StudentEventSearchComponent } from "./components/student-status/student-event-search.component";
import { PeriodSearchComponent } from "./components/others/period-search.component";
import { SchoolSearchComponent } from "./components/others/school-search.component";
import { PersonUanExportComponent } from "./components/others/person-uan-export.component";
import { SchoolCreateModalComponent } from "./components/others/school-create-modal.component";
import { PersonUanSearchComponent } from "./components/others/person-uan-search.component";
import { ResearchAreaSearchComponent } from './components/others/research-area-search.component';

@NgModule({
  declarations: [
    SettlementTabsComponent,
    InstitutionTabsComponent,
    OthersTabsComponent,
    CountrySearchComponent,
    DistrictSearchComponent,
    MunicipalitySearchComponent,
    SettlementSearchComponent,
    InstitutionSearchComponent,
    SubordinateSearchComponent,
    InstitutionSpecialitySearchComponent,
    InstitutionDoctoralSearchComponent,
    AdmissionReasonSearchComponent,
    EducationFeeTypeSearchComponent,
    SpecialityTabsComponent,
    AdmissionReasonHistoryComponent,
    StudentStatusTabsComponent,
    StudentStatusSearchComponent,
    StudentEventSearchComponent,
    PeriodSearchComponent,
    SchoolSearchComponent,
    PersonUanExportComponent,
    SchoolCreateModalComponent,
    PersonUanSearchComponent,
	ResearchAreaSearchComponent
  ],
  imports: [
  NomenclatureRoutingModule,
    SharedModule
  ],
  providers: [
    InstitutionSpecialityNomenclatureResource
  ]
})
export class NomenclatureModule { }
