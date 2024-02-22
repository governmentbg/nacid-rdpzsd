import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as saveAs from 'file-saver';
import { catchError, Observable, Observer, throwError } from 'rxjs';
import { Configuration } from 'src/app/configuration/configuration';
import { PersonSecondary } from 'src/rdpzsd/models/parts/person-secondary/person-secondary.model';
import { PersonSecondaryPartResource } from 'src/rdpzsd/resources/parts/person-secondary-part.resource';
import { AlertMessageDto } from 'src/shared/components/alert-message/models/alert-message.dto';
import { AlertMessageService } from 'src/shared/components/alert-message/services/alert-message.service';
import { MessageModalComponent } from 'src/shared/components/message-modal/message-modal.component';
import { ValidityDirective } from 'src/shared/directives/validity-directive/validity.directive';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'person-secondary',
  templateUrl: './person-secondary.component.html',
})
export class PersonSecondaryComponent extends ValidityDirective {

  @Input() personSecondary: PersonSecondary = new PersonSecondary();
  currentYear = (new Date()).getFullYear();
  loadingImages: boolean = false;

  constructor(
    public userDataService: UserDataService,
    private singlePartResource: PersonSecondaryPartResource,
    private modalService: NgbModal,
    public configuration: Configuration,
    private alertMessageService: AlertMessageService
  ) {
    super();
  }

  countryChange(countryId: number) {

    if (this.personSecondary.country?.code === 'BG') {
      this.personSecondary.recognitionDate = null;
      this.personSecondary.recognitionNumber = null;
      this.personSecondary.personSecondaryRecognitionDocument = null;
    }
    this.personSecondary.foreignSchoolName = null;
    this.personSecondary.school = null;
    this.personSecondary.schoolId = null;
    this.personSecondary.countryId = countryId;
    this.personSecondary.missingSchoolName = null;
    this.personSecondary.missingSchoolSettlementId = null;
    this.personSecondary.missingSchoolSettlement = null;
    this.personSecondary.missingSchoolFromRegister = false;
  }

  private loadDiplomaImageModal(rsoIntId: number){
    return new Observable((observer: Observer<any>) => {
      const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
      modal.componentInstance.text = 'rdpzsd.personLot.modals.downloadingData';
      modal.componentInstance.warning = 'rdpzsd.personLot.modals.warning';
      modal.componentInstance.acceptButton = 'root.buttons.yesGenerate';
      modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';
  
      modal.result.then((ok) => {
          if (ok) {
            this.loadingImages = true;
              this.singlePartResource
                  .getImagesFromRso(rsoIntId)
                  .pipe(
                      catchError(() => {
                          observer.next(null);
                          observer.complete();

                          const alertMessage = new AlertMessageDto('errorTexts.system_UnableToConnectToRdDocuments', 'danger', null);
                          this.alertMessageService.next(alertMessage);
                          return throwError(() => new Error('Communication error'));
                      })
                  )
                  .subscribe((blob: Blob) => {
                      observer.next(blob);
                      observer.complete();
                  });
          } else {
              observer.next(null);
              observer.complete();
          }
      });
    });
  }

  loadDiplomaImage(rsoIntId: number) {
    return this.loadDiplomaImageModal(rsoIntId)
      .subscribe((blob: Blob) => {
        if(blob) {
          saveAs(blob, 'ДипломаКопие.pdf');
        };

        this.loadingImages = false;
      });
  }

  schoolFromRegisterChanged() {
    this.personSecondary.school = null;
    this.personSecondary.schoolId = null;
    this.personSecondary.missingSchoolName = null;
    this.personSecondary.missingSchoolSettlementId = null;
    this.personSecondary.missingSchoolSettlement = null;
  }
}
