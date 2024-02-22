import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function LatinValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    if (isWhitespace) {
      return { whitespace: 'value is only whitespace' };
    } else {
      const latinPattern = /^[A-Za-z-\s]+$/;
      return latinPattern.test(control.value) ? null : { latin: 'value is not latin' }
    }
  };
}

@Directive({
  selector: '[latinValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => LatinDirective),
      multi: true
    }
  ]
})

export class LatinDirective implements Validator {

  @Input() enableEmptyValidation = false;

  private valFn = LatinValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    if (!this.enableEmptyValidation) {
      return this.valFn(control);
    } else {
      return control.value ? this.valFn(control) : null;
    }
  }
}
