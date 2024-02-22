import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { UserActivationComponent } from "./components/activation/user-activation.component";
import { UserChangePasswordComponent } from "./components/change-password/user-change-password.component";
import { LoginComponent } from "./components/login/login.component";
import { UserRecoverPasswordComponent } from "./components/recover/user-recover-password.component";
import { UserSideBarComponent } from "./components/side-bar/user-side-bar.component";
import { UserDataService } from "./services/user-data.service";
import { UserResource } from "./user.resource";
import { UserRoutingModule } from "./user.routing";

@NgModule({
  declarations: [
    LoginComponent,
    UserActivationComponent,
    UserRecoverPasswordComponent,
    UserChangePasswordComponent,
    UserSideBarComponent
  ],
  imports: [
    UserRoutingModule,
    SharedModule
  ],
  providers: [
    UserResource,
    UserDataService
  ],
  exports: [
    UserSideBarComponent
  ]
})
export class UserModule { }
