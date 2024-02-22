import { Directive, forwardRef, Input } from "@angular/core";
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from "@angular/forms";

export function CustomRegexValidator(patternName?: string): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    if (isWhitespace) {
      return { whitespace: 'value is only whitespace' };
    }else {
      const pattern = patterns[patternName];
      return pattern && pattern.test(control.value) ? null : { error: errors[patternName] }
      }
    };
  }

@Directive({
    selector: '[customRegexValidation]',
    providers: [
      {
        provide: NG_VALIDATORS,
        useExisting: forwardRef(() => CustomRegexDirective),
        multi: true
      }
    ]
})
  
export class CustomRegexDirective implements Validator {
  
    @Input() enableEmptyValidation = false;
    @Input() patternName = '';
    
    private valFn = CustomRegexValidator();
    validate(control: AbstractControl): { [key: string]: any } | null {
      this.valFn = CustomRegexValidator(this.patternName);
      if (!this.enableEmptyValidation) {
        return this.valFn(control);
      } else {
        return control.value ? this.valFn(control) : null;
      }
    }
}

const patterns : { [key: string]: any } = {
    foreignerBirthSettlement: /^[\u0400-\u04FF\s]+$/,
    residenceAddressCyrillic: /^[\u0400-\u04FF0-9-№.",\s]+$/,
    residenceAddressLatin: /^[A-Za-z0-9-№#.",\s]+$/,
    idnNumberRegex: /^[A-Z0-9-\s]+$/,
    schoolNameRegex: /^[\u0400-\u04FF0-9-VIX.",\s]+$/
}

const errors: { [key: string]: any } = {
  foreignerBirthSettlement: 'value is not cyrillic',
  residenceAddressCyrillic: 'value is not cyrillic',
  residenceAddressLatin: 'value is not latin',
  idnNumberRegex: 'invalid IDN number',
  schoolNameRegex: 'value is not cyrillic'
}
  