import { Directive, forwardRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function PasswordSymbolsValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const passwordValidation = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$/.test(control.value);
    return passwordValidation ? null : { passwordSymbols: 'password is not valid' };
  };
}

@Directive({
  selector: '[passwordSymbolsValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => PasswordSymbolsDirective),
      multi: true
    }
  ]
})

export class PasswordSymbolsDirective implements Validator {
  private valFn = PasswordSymbolsValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}
