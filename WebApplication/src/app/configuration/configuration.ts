import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDataService } from 'src/users/services/user-data.service';

@Injectable()
export class Configuration {
  restUrl: string;
  clientUrl: string;
  environmentName: string;
  defaultLanguage: string;
  enableFullFunctionality: boolean;
  enableRsoIntegration: boolean;

  constructor(
    private httpClient: HttpClient,
    private userDataService: UserDataService
  ) { }

  load(): Promise<{}> {
    return new Promise(resolve => {
      this.httpClient.get('../../configuration.json')
        .subscribe(config => {
          this.importSettings(config);
          this.userDataService.getUserData()
            .subscribe(() => resolve(true));
        });
    });
  }

  private importSettings(config: any) {
    this.restUrl = window.location.origin + '/api/';
    this.clientUrl = config.clientUrl || window.location.origin + '/';
    this.environmentName = config.environmentName;
    this.defaultLanguage = config.defaultLanguage;
    this.enableFullFunctionality = config.enableFullFunctionality;
    this.enableRsoIntegration = config.enableRsoIntegration;
  }
}
