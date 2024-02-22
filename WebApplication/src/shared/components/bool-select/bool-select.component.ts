import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, forwardRef, HostListener, Input, OnInit } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { BoolSelectModel } from "./bool-select.model";

@Component({
  selector: 'bool-select',
  templateUrl: './bool-select.component.html',
  styleUrls: ['./bool-select.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => BoolSelectComponent),
    multi: true
  }]
})
export class BoolSelectComponent implements ControlValueAccessor, OnInit {

  @Input() model: boolean = null;
  @Input() nullable = true;
  @Input() formControlClass = 'form-control form-control-sm';
  @Input() disabled = false;
  @Input() labels: string[] = ['booleans.active', 'booleans.inactive', 'booleans.all'];
  selectOpened = false;
  items: BoolSelectModel[] = [];

  @HostListener('document:click', ['$event']) onClickOutside(event: MouseEvent): void {
    if (this.selectOpened
      && !this.elementRef.nativeElement.contains(event.target)) {
      this.selectOpened = false;
    }
  }

  @HostListener('click', ['$event']) onClick(e?: Event) {
    if (!this.disabled) {
      this.selectOpened = !this.selectOpened;
    }
  }

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private elementRef: ElementRef
  ) {
  }

  selectOption(item: boolean, event: Event) {
    event.stopPropagation();
    this.model = item;
    this.propagateChange(item);
    this.propagateTouched();
    this.selectOpened = false;
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

  ngOnInit() {
    this.items.push(new BoolSelectModel(1, this.labels[0], true));
    this.items.push(new BoolSelectModel(2, this.labels[1], false));

    if (this.nullable) {
      this.items.push(new BoolSelectModel(3, this.labels[2], null));
    } else {
      this.labels = this.labels.slice(0, 2);
    }
  }
}