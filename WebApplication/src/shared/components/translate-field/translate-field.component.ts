import { Component, Input } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: 'translate-field',
  templateUrl: './translate-field.component.html'
})
export class TranslateFieldComponent {

  @Input() entity: any = null;
  @Input() propertyName = 'name';
  @Input() propertyNameAlt = 'nameAlt';

  constructor(public translate: TranslateService) { }
}