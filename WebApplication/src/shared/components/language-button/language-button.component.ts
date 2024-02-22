import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'language-button',
  templateUrl: 'language-button.component.html'
})
export class LanguageButtonComponent {

  constructor(
    public translate: TranslateService,
    private titleService: Title
  ) {
  }

  changeLanguage(language: string) {
    this.translate.use(language);
    this.translate.get('root.title')
      .subscribe(translatedTitle => this.titleService.setTitle(translatedTitle));
  }
}
