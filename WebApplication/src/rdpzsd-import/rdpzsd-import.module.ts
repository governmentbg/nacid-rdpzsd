import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { CreatePersonImportComponent } from "./components/modals/create-person-import.modal";
import { CreateSpecialityImportComponent } from "./components/modals/create-speciality-import.modal";
import { PersonImportSearchComponent } from "./components/person-import-search.component";
import { PersonImportComponent } from "./components/person-import.component";
import { SpecialityImportSearchComponent } from "./components/speciality-import-search.component";
import { RdpzsdImportRoutingModule } from "./rdpzsd-import.routing";

@NgModule({
    declarations: [
        PersonImportSearchComponent,
        CreatePersonImportComponent,
        PersonImportComponent,
        SpecialityImportSearchComponent,
        CreateSpecialityImportComponent
    ],
    imports: [
        RdpzsdImportRoutingModule,
        SharedModule
    ],
    providers: []
})
export class RdpzsdImportModule { }
