import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { LogoutAuthGuard } from 'src/app/auth-guard/logout.auth-guard';
import { UserActivationComponent } from './components/activation/user-activation.component';
import { UserRecoverPasswordComponent } from './components/recover/user-recover-password.component';
import { UserChangePasswordComponent } from './components/change-password/user-change-password.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [LogoutAuthGuard],
    data: {
      title: 'Вход в системата'
    }
  },
  {
    path: 'account/activation',
    component: UserActivationComponent,
    data: {
      title: 'Активиране на акаунт'
    }
  },
  {
    path: 'account/recover',
    component: UserRecoverPasswordComponent,
    data: {
      title: 'Възстановяване на парола'
    }
  },
  {
    path: 'account/changePassword',
    component: UserChangePasswordComponent,
    data: {
      title: 'Смяна на парола'
    }
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }

