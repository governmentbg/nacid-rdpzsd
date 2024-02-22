import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function CyrillicValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    if (isWhitespace) {
      return { whitespace: 'value is only whitespace' };
    } else {
      const cyrillicPattern = /^[\u0400-\u04FF-\s]+$/;
      return cyrillicPattern.test(control.value) ? null : { cyrillic: 'value is not cyrillic' }
    }
  };
}

@Directive({
  selector: '[cyrillicValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CyrillicDirective),
      multi: true
    }
  ]
})

export class CyrillicDirective implements Validator {

  @Input() enableEmptyValidation = false;

  private valFn = CyrillicValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    if (!this.enableEmptyValidation) {
      return this.valFn(control);
    } else {
      return control.value ? this.valFn(control) : null;
    }
  }
}
