import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function PhoneValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    if (isWhitespace) {
      return { whitespace: 'value is only whitespace' };
    } else {
      const phonePattern = /^[\+]?[(]?[0-9\s]{3}[)]?[-\.]?[0-9\s]{3}[-\.]?[0-9\s]{3,12}$/im;
      return phonePattern.test(control.value) ? null : { phone: 'value is not a valid phone number' }
    }
  };
}

@Directive({
  selector: '[phoneValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => PhoneDirective),
      multi: true
    }
  ]
})

export class PhoneDirective implements Validator {

  @Input() enableEmptyValidation = false;

  private valFn = PhoneValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    if (!this.enableEmptyValidation) {
      return this.valFn(control);
    } else {
      return control.value ? this.valFn(control) : null;
    }
  }
}
