import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PersonImportSearchComponent } from "./components/person-import-search.component";
import { PersonImportComponent } from "./components/person-import.component";
import { SpecialityImportSearchComponent } from "./components/speciality-import-search.component";

const routes: Routes = [
    {
        path: 'personImport',
        component: PersonImportSearchComponent,
        data: {
            title: 'Създаване на физически лица'
        }
    },
    {
        path: 'personImport/:id',
        component: PersonImportComponent,
        data: {
            title: 'Преглед на физически лица'
        }
    },
    {
        path: 'specialityImport',
        component: SpecialityImportSearchComponent,
        data: {
            title: 'Създаване на семестриална информация'
        }
    }
]

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RdpzsdImportRoutingModule { }