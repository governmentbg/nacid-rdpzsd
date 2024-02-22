import { HttpClient } from "@angular/common/http";
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, EventEmitter, forwardRef, HostListener, Input, OnInit, Output } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { Observable, Subject } from "rxjs";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { Configuration } from "src/app/configuration/configuration";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

@Component({
  selector: 'nomenclature-select',
  templateUrl: './nomenclature-select.component.html',
  styleUrls: ['./nomenclature-select.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NomenclatureSelectComponent),
    multi: true
  }]
})
export class NomenclatureSelectComponent implements ControlValueAccessor, OnInit {

  @Input() restUrl: string = null;
  @Input() placeholder: string = null;
  @Input() textTemplate: string = null;
  @Input() disabled = false;
  @Input() allowClear = true;
  @Input() keyProperty = 'id';
  @Input() limit = 10;
  @Input() includeInactive = false;
  @Input() collection: any[];
  @Input() filterCollectionBy: string;
  @Input() showSearchBox = true;
  @Input() required = false;
  @Input() formControlClass = 'form-control form-control-sm'
  @Input() touched = true;
  @Input() translateName = false;
  filter = new FilterDto();
  @Input('filter')
  set filterSetter(filter: any) {
    this.filter = filter;
    this.options = [];
  }

  @Output() readonly keyPropertyChange = new EventEmitter<number>();
  private textFilterChanged = new Subject<string>();

  selectedModel: any = null;
  loading = false;
  selectOpened = false;
  options: any[] = [];
  totalCount = 0;

  constructor(
    private elementRef: ElementRef,
    private httpClient: HttpClient,
    private changeDetectorRef: ChangeDetectorRef,
    private configuration: Configuration
  ) {
  }

  @HostListener('document:click', ['$event']) onClickOutside(event: MouseEvent): void {
    if (this.selectOpened
      && !this.elementRef.nativeElement.contains(event.target)
      && (<HTMLTextAreaElement>event.target).id != 'chevronButton') {
      this.closeSelect();
    }
  }

  @HostListener('click', ['$event']) onClick(e?: Event) {
    if (!this.disabled) {
      if (e && (e.target as Element).className === 'options-item') {
        return;
      }

      this.clickElement(e);
    }
  }

  @HostListener('keydown', ['$event'])
  keyboardInput(event: KeyboardEvent) {
    if (!this.selectOpened) {
      if (event.key === 'ArrowDown') {
        this.onClick();
      }

      return;
    }

    switch (event.key) {
      case 'Enter':
        this.closeSelect();
        break;
      case 'Escape':
        this.closeSelect();
        break;
    }
  }

  @HostListener('scroll', ['$event'])
  onScroll(event: any) {
    if (!this.loading && this.options && this.options.length < this.totalCount && event.target.offsetHeight + event.target.scrollTop >= event.target.scrollHeight) {
      this.loadOptions();
    }
  }

  clickElement(event: Event) {
    if (!this.selectOpened) {
      this.filter.limit = this.limit;
      this.filter.offset = 0;
      this.loadOptions();
    }

    this.selectOpened = !this.selectOpened;
  }

  clearSelection(event: Event) {
    this.setValueFromInside(null);
    event.stopPropagation();
  }

  optionsChanged(items: any[]) {
    if (items.length > 0) {
      this.setValueFromInside(items[0]);
    }
    this.closeSelect();
  }

  textFilterChange(textFilter: string) {
    this.loading = true;
    this.filter.offset = 0;
    this.textFilterChanged.next(textFilter);
  }

  private setValueFromInside(newValue: any) {
    this.selectedModel = newValue;
    this.propagateChange(newValue);
    this.keyPropertyChange.emit(newValue ? newValue[this.keyProperty] : null);
    this.propagateTouched();
    this.touched = true;
  }

  private closeSelect() {
    this.selectOpened = false;
    this.filter.textFilter = null;
    if (this.restUrl) {
      this.options = [];
      this.changeDetectorRef.detectChanges();
    }
  }

  private loadOptions() {
    if (!this.restUrl) {
      this.options = this.collection;
      if (this.filterCollectionBy && this.filter.textFilter) {
        this.options = this.options.filter(option => {
          const innerProperties = this.filterCollectionBy.split('.');
          let filteredProperty = innerProperties.length ? option : option[this.filterCollectionBy];
          innerProperties.forEach(key => filteredProperty = filteredProperty[key]);
          return filteredProperty.toString().toLowerCase().indexOf(this.filter.textFilter?.toLowerCase()) !== -1;
        });
      }
      this.loading = false;
    } else {
      this.loading = true;
      this.getFiltered()
        .subscribe(e => {
          const currentElements = e.result;
          this.totalCount = e.totalCount;

          if (this.filter.offset) {
            const tempArray = this.options.slice(0);
            tempArray.push(...currentElements);
            this.options = tempArray;
          } else {
            this.options = currentElements;
          }

          this.filter.offset = this.options.length;
          this.loading = false;
          this.changeDetectorRef.detectChanges();
        });
    }
  }

  private getFiltered(): Observable<SearchResultDto<any>> {
    this.filter.isActive = this.includeInactive ? null : true;
    return this.httpClient.post<SearchResultDto<any>>(`${this.configuration.restUrl}${this.restUrl}`, this.filter);
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
    this.selectedModel = value;
    this.changeDetectorRef.detectChanges();
  }

  ngOnInit() {
    this.textFilterChanged.pipe(
      debounceTime(500),
      distinctUntilChanged())
      .subscribe(newFilter => {
        this.filter.textFilter = newFilter;
        this.loadOptions();
      });
  }
}