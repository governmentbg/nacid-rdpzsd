import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RdpzsdSearchComponent } from './components/search/rdpzsd-search.component';
import { RdpzsdSearchNewComponent } from './components/search/rdpzsd-search-new.component';
import { PersonLotNewComponent } from './components/lot/new/person-lot-new.component';
import { PersonLotComponent } from './components/lot/person-lot.component';
import { RdpzsdSearchApprovalComponent } from './components/search/rdpzsd-search-approval.component';
import { RdpzsdSearchGraduatedComponent } from './components/search/graduated/rdpzsd-search-graduated.component';
import { RdpzsdSearchStickerComponent } from './components/search/graduated/rdpzsd-search-sticker.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'rdpzsd'
    },
    {
        path: 'rdpzsd',
        component: RdpzsdSearchComponent,
        data: {
            title: 'Търсене'
        }
    },
    {
        path: 'rdpzsdGraduated',
        component: RdpzsdSearchGraduatedComponent,
        data: {
            title: 'Търсене - дипломи'
        }
    },
    {
        path: 'rdpzsdStickers',
        component: RdpzsdSearchStickerComponent,
        data: {
            title: 'Търсене - семестриално завършили и стикери'
        }
    },
    {
        path: 'rdpzsdNewSearch',
        component: RdpzsdSearchNewComponent,
        data: {
            title: 'Физически лица'
        }
    },
    {
        path: 'rdpzsdApproval',
        component: RdpzsdSearchApprovalComponent,
        data: {
            title: 'За одобрение'
        }
    },
    {
        path: 'rdpzsd/new',
        component: PersonLotNewComponent,
        data: {
            title: 'Създаване на ново физическо лице'
        }
    },
    {
        path: 'personLot/:uan/:activeTab',
        component: PersonLotComponent,
        data: {
            title: 'Преглед на партида'
        }
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RdpzsdRoutingModule { }
