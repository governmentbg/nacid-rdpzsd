import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SettlementTabsComponent } from './components/tabs/settlement-tabs.component';
import { InstitutionTabsComponent } from './components/tabs/institution-tabs.component';
import { OthersTabsComponent } from './components/tabs/others-tabs.component';
import { SpecialityTabsComponent } from './components/tabs/speciality-tabs.component';
import { StudentStatusTabsComponent } from './components/tabs/student-status-tabs.component';

const routes: Routes = [
  {
    path: 'nomenclature/settlement',
    component: SettlementTabsComponent,
    data: {
      title: 'Населени места'
    }
  },
  {
    path: 'nomenclature/institution',
    component: InstitutionTabsComponent,
    data: {
      title: 'Институции'
    }
  },
  {
    path: 'nomenclature/speciality',
    component: SpecialityTabsComponent,
    data: {
      title: 'Специалности'
    }
  },
  {
    path: 'nomenclature/studentStatus',
    component: StudentStatusTabsComponent,
    data: {
      title: 'Статуси и събития'
    }
  },
  {
    path: 'nomenclature/others',
    component: OthersTabsComponent,
    data: {
      title: 'Други номенклатури'
    }
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NomenclatureRoutingModule { }
