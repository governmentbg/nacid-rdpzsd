import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PageNotFoundComponent } from '../static-pages/components/page-not-found/page-not-found.component';

const routes: Routes = [
  {
    path: '**',
    redirectTo: '/notFound'
  },
  {
    path: 'notFound',
    component: PageNotFoundComponent,
    data: {
      title: 'Страницата не е намерена'
    }
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaticPageRoutingModule { }

