import { Directive, forwardRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function NoWhitespaceValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    return !isWhitespace ? null : { whitespace: 'value is only whitespace' };
  };
}

@Directive({
  selector: '[noWhiteSpacesValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => NoWhitespaceDirective),
      multi: true
    }
  ]
})

export class NoWhitespaceDirective implements Validator {
  private valFn = NoWhitespaceValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}
