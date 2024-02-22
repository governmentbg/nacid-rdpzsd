import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'options-select',
  templateUrl: 'options-select.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['./options-select.styles.css']
})
export class OptionsSelectComponent {

  @Input() selectedOptions: any[] = [];
  @Input() options: any[] = [];
  @Input() keyProperty = 'id';
  @Input() textTemplate: string;
  @Input() translateName = false;

  @Output() readonly modelChange = new EventEmitter<any[]>();

  selectOption(item: any, event: Event) {
    event.stopPropagation();
    const index = this.selectedOptions.findIndex(e => e[this.keyProperty] === item[this.keyProperty]);
    if (index >= 0) {
      this.selectedOptions.splice(index, 1);
    } else {
      this.selectedOptions.push(item);
    }

    this.modelChange.emit(this.selectedOptions);
  }

  is(item: any) {
    const ind = this.selectedOptions.findIndex(e => e[this.keyProperty] === item[this.keyProperty]);
    if (ind >= 0) {
      return true;
    }

    return false;
  }
}
