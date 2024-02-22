import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, forwardRef, HostListener, Input } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { EnumSelect } from "./enum-select.model";

@Component({
  selector: 'enum-select',
  templateUrl: './enum-select.component.html',
  styleUrls: ['./enum-select.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => EnumSelectComponent),
    multi: true
  }]
})
export class EnumSelectComponent implements ControlValueAccessor {

  @Input() model: any = null;
  @Input() formControlClass = 'form-control form-control-sm';
  @Input() disabled = false;
  @Input() allowClear = true;
  @Input() showSearchBox = false;
  @Input() enumName: string;
  @Input() enumType: any;
  @Input() required = false;
  @Input() touched = true;
  @Input() excludeEnumArray: number[] = [];
  selectOpened = false;
  options: EnumSelect[] = [];
  allOptions: EnumSelect[] = [];
  textFilter: string = null;

  @HostListener('document:click', ['$event']) onClickOutside(event: MouseEvent): void {
    if (this.selectOpened
      && !this.elementRef.nativeElement.contains(event.target)) {
      this.selectOpened = false;
    }
  }

  @HostListener('click', ['$event']) onClick(e?: Event) {
    if (!this.disabled) {
      this.selectOpened = !this.selectOpened;

      if (this.selectOpened) {
        this.loadOptions(this.enumType);
      }
    }
  }

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private elementRef: ElementRef,
    private translate: TranslateService
  ) {
  }

  clearSelection(event: Event) {
    this.setValueFromInside(null);
    event.stopPropagation();
  }

  textFilterChange(textFilter: string) {
    if (textFilter.length >= 1) {
      if (textFilter.length > this.textFilter?.length) {
        this.options = this.options.filter(e => e.name.toLowerCase().indexOf(textFilter.toLowerCase()) !== - 1);
      } else {
        this.options = this.allOptions;
        this.options = this.options.filter(e => e.name.toLowerCase().indexOf(textFilter.toLowerCase()) !== - 1);
      }
    } else {
      this.loadOptions(this.enumType);
    }

    this.textFilter = textFilter;
  }

  selectOption(item: any, event: Event) {
    this.setValueFromInside(item)
    event.stopPropagation();
  }

  private setValueFromInside(newValue: any) {
    this.model = newValue;
    this.propagateChange(newValue);
    this.propagateTouched();
    this.touched = true;
    this.selectOpened = false;
  }

  private loadOptions(enumType: any) {
    this.options = [];
    this.allOptions = [];
    Object.keys(enumType)
      .filter(Number)
      .forEach(e => {
        this.translate.get(`enums.${this.enumName}.${enumType[e]}`)
          .subscribe(enumLabel => {
            this.options.push(new EnumSelect(enumLabel, e));
            this.allOptions.push(new EnumSelect(enumLabel, e));
          });
      });

      if(this.excludeEnumArray) {
        this.excludeEnumArray.forEach((element) => {
          this.options = this.options.filter(e => e.value != element);
        })
        
        this.excludeEnumArray.forEach((element) => {
          this.allOptions = this.allOptions.filter(e => e.value != element);
        })
      }
  }

  // ControlValueAccessor implementation start
  propagateChange = (_: any) => { };
  propagateTouched = () => { };
  registerOnChange(fn: (_: any) => void) {
    this.propagateChange = fn;
  }
  registerOnTouched(fn: () => void) {
    this.propagateTouched = fn;
  }
  writeValue(value: any) {
    this.model = value;
    this.changeDetectorRef.detectChanges();
  }
}