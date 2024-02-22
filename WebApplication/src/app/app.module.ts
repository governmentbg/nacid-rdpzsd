import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { NomenclatureModule } from 'src/nomenclatures/nomenclature.module';
import { PeriodNomenclatureResource } from 'src/nomenclatures/resources/period-nomenclature.resource';
import { RdpzsdImportModule } from 'src/rdpzsd-import/rdpzsd-import.module';
import { RdpzsdReportModule } from 'src/rdpzsd-report/rdpzsd-report.module';
import { RdpzsdModule } from 'src/rdpzsd/rdpzsd.module';
import { CourseSemesterCollectionService } from 'src/shared/services/course-semester/course-semester.service';
import { SharedModule } from 'src/shared/shared.module';
import { StaticPageModule } from 'src/static-pages/static-page.module';
import { UserModule } from 'src/users/user.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LogoutAuthGuard } from './auth-guard/logout.auth-guard';
import { Configuration } from './configuration/configuration';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { BreadcrumbComponent } from './root/breadcrumb/breadcrumb.component';
import { FooterComponent } from './root/footer/footer.component';
import { HeaderComponent } from './root/header/header.component';
import { LoginHeaderComponent } from './root/header/login-header.component';
import { LogoutHeaderComponent } from './root/header/logout-header.component';
import { UserHeaderComponent } from './root/header/user-header.component';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LogoutHeaderComponent,
    LoginHeaderComponent,
    UserHeaderComponent,
    FooterComponent,
    BreadcrumbComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    UserModule,
    RdpzsdModule,
    RdpzsdImportModule,
    RdpzsdReportModule,
    NomenclatureModule,
    SharedModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    StaticPageModule
  ],
  providers: [
    Configuration,
    PeriodNomenclatureResource,
    LogoutAuthGuard,
    CourseSemesterCollectionService,
    {
      provide: APP_INITIALIZER,
      useFactory: configFactory,
      deps: [Configuration],
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: latestPeriodFactory,
      deps: [PeriodNomenclatureResource],
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: courseSemesterFactory,
      deps: [CourseSemesterCollectionService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function configFactory(config: Configuration) {
  return () => config.load();
}

export function latestPeriodFactory(latestPeriodService: PeriodNomenclatureResource) {
  return () => latestPeriodService.getLatestPeriod();
}

export function courseSemesterFactory(courseSemesterService: CourseSemesterCollectionService): Function {
  return () => courseSemesterService.constructCollection();
}
