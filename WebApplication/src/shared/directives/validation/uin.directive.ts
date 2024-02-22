import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function UinValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    if (isWhitespace) {
      return { whitespace: 'value is only whitespace' };
    } else {
      if (isNaN(control.value) || control.value.length !== 10) {
        return { uin: 'value is not uin' };
      } else {
        let monthNumbers = +control.value.substring(2, 4);
        let dayNumbers = +control.value.substring(4, 6);
        
        if ((monthNumbers > 12 && monthNumbers <= 20) || (monthNumbers > 32 && monthNumbers <= 40) || dayNumbers > 31) {
          return { uin: 'invalid uin' };
        }

        var weights = [2, 4, 8, 5, 10, 9, 7, 3, 6];
        var checkSum = +control.value.charAt(9);
        var mod = 11;
        var sum = 0;

        for (let i = 0; i <= control.value.length - 2; i++) {
          sum += (control.value[i] * weights[i]);
        }

        var validCheckSum = sum % mod;

        if (validCheckSum >= 10) {
          validCheckSum = 0;
        }

        return checkSum == validCheckSum ? null : { uin: 'value is not uin' };
      }
    }
  };
}

@Directive({
  selector: '[uinValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => UinDirective),
      multi: true
    }
  ]
})

export class UinDirective implements Validator {

  @Input() enableEmptyValidation = false;

  private valFn = UinValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    if (!this.enableEmptyValidation) {
      return this.valFn(control);
    } else {
      return control.value ? this.valFn(control) : null;
    }
  }
}
