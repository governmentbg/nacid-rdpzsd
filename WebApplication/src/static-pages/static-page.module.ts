import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { PageNotFoundComponent } from "./components/page-not-found/page-not-found.component";
import { StaticPageRoutingModule } from "./static-page.routing";

@NgModule({
  declarations: [
    PageNotFoundComponent
  ],
  imports: [
    StaticPageRoutingModule,
    SharedModule
  ],
  providers: [
  ]
})
export class StaticPageModule { }
