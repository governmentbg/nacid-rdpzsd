import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SyncButtonComponent } from './components/sync-button/sync-button.component';
import { AutofocusDirective } from './directives/autofocus.directive';
import { AlertMessageService } from './components/alert-message/services/alert-message.service';
import { AlertMessageComponent } from './components/alert-message/alert-message.component';
import { NoWhitespaceDirective } from './directives/validation/no-whitespace.directive';
import { PasswordSymbolsDirective } from './directives/validation/password-symbols.directive';
import { EmailDirective } from './directives/validation/email.directive';
import { DropdownButtonComponent } from './components/dropdown-button/dropdown-button.component';
import { TranslateFieldComponent } from './components/translate-field/translate-field.component';
import { LanguageButtonComponent } from './components/language-button/language-button.component';
import { LoadingPageComponent } from './components/loading-page/loading-page.component';
import { LoadingSectionComponent } from './components/loading-section/loading-section.component';
import { NomenclatureSelectComponent } from './components/nomenclature-select/nomenclature-select.component';
import { NomenclaturePipe } from './pipes/nomenclature.pipe';
import { OptionsSelectComponent } from './components/options-select/options-select.component';
import { SettlementChangeService } from './services/settlements/settlement-change.service';
import { BoolSelectComponent } from './components/bool-select/bool-select.component';
import { EnumSelectComponent } from './components/enum-select/enum-select.component';
import { CollapsableLabelComponent } from './components/collapsable-label/collapsable-label.component';
import { InstitutionChangeService } from './services/institutions/institution-change.service';
import { CyrillicDirective } from './directives/validation/cyrillic.directive';
import { LatinDirective } from './directives/validation/latin.directive';
import { InvalidFieldComponent } from './components/invalid-field/invalid-field.component';
import { UinDirective } from './directives/validation/uin.directive';
import { ForeignerNumberDirective } from './directives/validation/foreigner-number.directive';
import { DatetimeComponent } from './components/date-time/date-time.component';
import { PhoneDirective } from './directives/validation/phone.directive';
import { MessageModalComponent } from './components/message-modal/message-modal.component';
import { CollapsableSectionComponent } from './components/collapsable-section/collapsable-section.component';
import { CustomRegexDirective } from './directives/validation/custom-regex.directive';
import { StudentStatusChangeService } from './services/student-statuses/student-status-change.service';
import { ImageSelectComponent } from './components/image-select/image-select.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { UanDirective } from './directives/validation/uan.directive';
import { NumbersOnlyDirective } from './directives/validation/numbers-only.directive';
import { LabelColorDirective } from './directives/validation/label-color.directive';
import { CarouselModalComponent } from './components/carousel-modal/carousel-modal.component';
import { FormatNumberPipe } from './pipes/format-number.pipe';
import { NoteModalComponent } from './components/message-modal/note-modal.component';
import { DropdownSubmenuComponent } from './components/dropdown-submenu/dropdown-submenu.component';
import { CitizenshipDirective } from './directives/validation/citizenship.directive';

const components = [
  SyncButtonComponent,
  DropdownButtonComponent,
  DropdownSubmenuComponent,
  AutofocusDirective,
  AlertMessageComponent,
  ImageSelectComponent,
  FileUploadComponent,
  NoWhitespaceDirective,
  CustomRegexDirective,
  CyrillicDirective,
  LatinDirective,
  NumbersOnlyDirective,
  UanDirective,
  PasswordSymbolsDirective,
  EmailDirective,
  PhoneDirective,
  UinDirective,
  ForeignerNumberDirective,
  LabelColorDirective,
  CitizenshipDirective,
  TranslateFieldComponent,
  LanguageButtonComponent,
  LoadingPageComponent,
  LoadingSectionComponent,
  NomenclatureSelectComponent,
  NomenclaturePipe,
  FormatNumberPipe,
  OptionsSelectComponent,
  BoolSelectComponent,
  EnumSelectComponent,
  DatetimeComponent,
  CollapsableLabelComponent,
  CollapsableSectionComponent,
  InvalidFieldComponent,
  MessageModalComponent,
  CarouselModalComponent,
  NoteModalComponent
];

const providers = [
  AlertMessageService,
  SettlementChangeService,
  InstitutionChangeService,
  StudentStatusChangeService,
  NgbActiveModal,
  DatePipe
]

const commonModules = [
  CommonModule,
  FormsModule,
  TranslateModule,
  NgbModule
]

@NgModule({
  declarations: components,
  imports: commonModules,
  providers: [providers],
  exports: [...commonModules, ...components]
})
export class SharedModule { }
