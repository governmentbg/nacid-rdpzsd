import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { UserAuthorizationState } from 'src/users/dtos/login.dtos';
import { UserDataService } from 'src/users/services/user-data.service';
import { Configuration } from './configuration/configuration';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  userAuthorizationState = UserAuthorizationState;

  constructor(
    public translateService: TranslateService,
    public configuration: Configuration,
    private titleService: Title,
    public userDataService: UserDataService
  ) {
  }

  private setDefaultLanguage() {
    this.translateService.setDefaultLang(this.configuration.defaultLanguage);
    this.translateService.use(this.configuration.defaultLanguage);
    this.translateService.get('root.title')
      .subscribe(translatedTitle => this.titleService.setTitle(translatedTitle));
  }

  ngOnInit() {
    this.setDefaultLanguage();
  }
}
