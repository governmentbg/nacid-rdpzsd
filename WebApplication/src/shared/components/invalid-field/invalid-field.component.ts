import { Component, Input } from '@angular/core';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'invalid-field',
  templateUrl: 'invalid-field.component.html'
})
export class InvalidFieldComponent {

  @Input() text = 'invalidFields.required';
  @Input() element: NgModel = null;
  @Input () requiredTouched: boolean = true;
}
