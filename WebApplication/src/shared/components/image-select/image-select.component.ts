import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError, throwError } from 'rxjs';
import { Configuration } from 'src/app/configuration/configuration';
import { RdpzsdAttachedFile } from 'src/shared/models/rdpzsd-attached-file.model';
import { FileUploadService } from 'src/shared/services/file-upload.service';
import { AlertMessageDto } from '../alert-message/models/alert-message.dto';
import { AlertMessageService } from '../alert-message/services/alert-message.service';
import { CarouselModalComponent } from '../carousel-modal/carousel-modal.component';

@Component({
  selector: 'image-select',
  templateUrl: './image-select.component.html',
  providers: [
    FileUploadService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ImageSelectComponent),
      multi: true
    }
  ]
})
export class ImageSelectComponent implements OnInit {
  imgSrc: SafeResourceUrl = "assets/img/unknown.png";
  imageFormats: Array<string> = ['image/png', 'image/jpg', 'image/jpeg', 'image/gif', 'image/tiff', 'image/tif', 'image/bpg'];

  // We can use this for TXT import progress bar.
  fileUploadProgress: number = null;

  @Input() dbId = 1;

  imageKey: string;
  @Input('imageKey')
  set imageKeySetter(imageKey: string) {
    if (imageKey) {
      this.imageKey = imageKey;
      const attachedFile = new RdpzsdAttachedFile();
      attachedFile.key = imageKey;
      attachedFile.dbId = this.dbId;
      this.fileUploadService.getNewImage(attachedFile)
        .subscribe(e => this.loadImage(e))
    }
  }

  @Input() carouselRestUrl: string;
  @Input() carouselEnabled = false;
  carouselImages: SafeResourceUrl[] = [];

  @Input() disabled = false;

  model: RdpzsdAttachedFile | null;
  fileUrl: string;

  private fileStorageUrl: any;

  constructor(
    private configuration: Configuration,
    private service: FileUploadService,
    private alertMessageService: AlertMessageService,
    private sanitizer: DomSanitizer,
    private fileUploadService: FileUploadService,
    private modalService: NgbModal,
    private http: HttpClient
  ) {
    this.fileStorageUrl = `${this.configuration.restUrl}FileStorage`;
  }

  ngOnInit(): void {
  }

  propagateChange = (_: any) => { };
  propagateTouched = () => { };

  writeValue(obj: any): void {
    this.model = obj;
    this.setFileUrl();
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.propagateTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  changeImage(event: any): void {
    if (this.disabled) {
      return;
    }
    const target = event.target || event.srcElement;
    const files = target.files;
    if (files.length > 0) {
      if (!this.imageFormats.includes(files[0].type)) {
        const alertMessage = new AlertMessageDto('errorTexts.personBasic_InvalidPictureFormat', 'danger', null);
        this.alertMessageService.next(alertMessage);
        return;
      }

      this.service.uploadFile(files[0], this.fileStorageUrl)
        .pipe(
          catchError(() => {
            this.fileUploadProgress = null;
            return throwError(() => new Error('Error'));
          })
        )
        .subscribe((resultEvent: HttpEvent<RdpzsdAttachedFile>) => {
          if (resultEvent.type === HttpEventType.UploadProgress) {
            this.fileUploadProgress = Math.round(100 * resultEvent.loaded / resultEvent.total);
          } else if (resultEvent.type === HttpEventType.Response) {
            this.fileUploadProgress = null;
            this.markAsUploaded(files[0], resultEvent.body);
          }
        });
    }
  }

  openCarousel() {
    if (this.carouselEnabled && this.carouselRestUrl) {
      this.http.get<string[]>(`${this.configuration.restUrl}${this.carouselRestUrl}`)
        .subscribe(e => {
          if (e && e.length > 0) {
            this.carouselImages = [];
            e.forEach(s => this.carouselImages.push(this.sanitizer.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${s}`)));
            const modal = this.modalService.open(CarouselModalComponent, { centered: true });
            modal.componentInstance.images = this.carouselImages;
          }
        });
    }
  }

  private setFileUrl(): void {
    if (!this.model) {
      return;
    }

    this.fileUrl = `${this.fileStorageUrl}?key=${this.model.key}&fileName=${this.model.name}&dbId=${this.model.dbId}`;
  }

  private markAsUploaded(file: File, additionalInfo: RdpzsdAttachedFile): void {
    if (!this.model) {
      this.model = new RdpzsdAttachedFile();
    }

    this.model.name = file.name;
    this.model.mimeType = file.type;
    this.model.size = file.size;
    this.model.key = additionalInfo.key || (additionalInfo as any).fileKey;
    this.model.hash = additionalInfo.hash;
    this.model.dbId = additionalInfo.dbId;

    this.propagateChange(this.model);
  }

  private loadImage(base64ImageUrl: string): void {
    this.imgSrc = this.sanitizer.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${base64ImageUrl}`);
  }
}
