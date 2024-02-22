import { Component, Input } from "@angular/core";

@Component({
  selector: 'loading-section',
  templateUrl: 'loading-section.component.html'
})
export class LoadingSectionComponent {

  @Input() loadingText = 'root.loadingSection.text';
  @Input() iconSize = 'fa-3x';
  @Input() marginSize = '4';

}