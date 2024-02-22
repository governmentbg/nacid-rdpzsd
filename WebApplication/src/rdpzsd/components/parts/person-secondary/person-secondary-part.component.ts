import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration } from 'src/app/configuration/configuration';
import { NomenclatureCodeFilterDto } from 'src/nomenclatures/dtos/nomenclature-code-filter.dto';
import { Country } from 'src/nomenclatures/models/settlement/country.model';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { PartHistoryDto } from 'src/rdpzsd/dtos/parts/part-history.dto';
import { LotState } from 'src/rdpzsd/enums/lot-state.enum';
import { PartState } from 'src/rdpzsd/enums/part-state.enum';
import { StudentStickerState } from 'src/rdpzsd/enums/parts/student-sticker-state.enum';
import { PersonSecondaryHistory } from 'src/rdpzsd/models/parts/person-secondary/history/person-secondary-history.model';
import { PersonSecondary } from 'src/rdpzsd/models/parts/person-secondary/person-secondary.model';
import { PersonSecondaryPartResource } from 'src/rdpzsd/resources/parts/person-secondary-part.resource';
import { CurrentPersonContextService } from 'src/rdpzsd/services/current-person-context.service';
import { UserDataService } from 'src/users/services/user-data.service';
import { PersonSecondaryHistoryComponent } from './person-secondary-history.component';

@Component({
  selector: 'person-secondary-part',
  templateUrl: './person-secondary-part.component.html',
  providers: [
    PersonSecondaryPartResource,
    BaseNomenclatureCodeResource
  ]
})
export class PersonSecondaryPartComponent implements OnInit {

  entity: PersonSecondary = new PersonSecondary();
  loadingData: boolean;
  partState = PartState;
  stickerState = StudentStickerState;

  @Output() fromRsoEventEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();

  personLotId: number;
  @Input('personLotId')
  set personLotIdSetter(personLotId: number) {
    if (personLotId) {
      this.loadingData = true;
      this.personLotId = personLotId;
      this.singlePartResource
        .getSinglePart(personLotId)
        .subscribe(e => {
          this.entity = e;
          this.loadingData = false;
          this.fromRsoChange();
        });
    }
  }

  @Input() fromModal = false;
  @Input() hideEditActions = false;
  @Input() personLotState: LotState;

  currentPersonContextService: CurrentPersonContextService = null;

  isValidPersonSecondaryForm = false;
  isEditMode = false;

  constructor(
    private singlePartResource: PersonSecondaryPartResource,
    private injector: Injector,
    private activeModal: NgbActiveModal,
    public userDataService: UserDataService,
    private modalService: NgbModal,
    private baseNomenclatureCodeResource: BaseNomenclatureCodeResource<Country, NomenclatureCodeFilterDto>,
    public configuration: Configuration
  ) { }

  getFromRso() {
    return this.singlePartResource
      .getFromRso(this.personLotId)
      .subscribe(e => {
        if (!this.entity?.id) {
          this.addPart(e);
        } else {
          this.entity = e;
          this.entity.id = this.personLotId;
          this.entity.state = this.partState.actual;
          this.fromRsoChange();
        }
      });
  }

  addPart(personSecondary: PersonSecondary) {
    this.entity = personSecondary || new PersonSecondary();
    this.entity.state = PartState.actual;

    if (!personSecondary) {
      this.baseNomenclatureCodeResource.getByCode('Country', 'BG')
        .subscribe((country) => {
          this.entity.countryId = country.id
          this.entity.country = country;
        })
    }

    if (!this.fromModal) {
      this.currentPersonContextService.setIsInEdit(true);
    }
    this.isEditMode = true;
  }

  modelChange(entity: PersonSecondary) {
    this.entity = entity;
    this.fromRsoChange();
  }

  showHistory(partHistoryDto: PartHistoryDto<PersonSecondary, PersonSecondaryHistory>) {
    const modal = this.modalService.open(PersonSecondaryHistoryComponent, { backdrop: 'static', size: 'xl' });
    modal.componentInstance.personSecondaryHistoryDto = partHistoryDto;
  }

  closeModal() {
    this.activeModal.close(this.entity);
  }

  fromRsoChange() {
    if(this.entity){
      this.fromRsoEventEmitter.emit(this.entity.fromRso);
    }
  }

  ngOnInit() {
    if (!this.fromModal) {
      this.currentPersonContextService = this.injector.get<CurrentPersonContextService>(CurrentPersonContextService);
      this.hideEditActions = (this.currentPersonContextService.openedFromStickers
        && this.currentPersonContextService.personStudentStickerDto?.stickerState !== this.stickerState.none
        && this.currentPersonContextService.personStudentStickerDto?.stickerState !== this.stickerState.returnedForEdit)
    }
  }
}
