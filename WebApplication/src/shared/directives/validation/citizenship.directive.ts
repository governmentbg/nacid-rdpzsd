import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[citizenshipValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CitizenshipDirective),
      multi: true
    }
  ]
})
export class CitizenshipDirective {

  @Input() otherCitizenshipId: number;
  
  validate(control: AbstractControl): { [key: string]: any } | null {
    if (this.otherCitizenshipId && control.value) {
      return control.value.id !== this.otherCitizenshipId ? null : { error: 'Invalid citizenship' }
    } else {
      return null;
    }
  }
}
