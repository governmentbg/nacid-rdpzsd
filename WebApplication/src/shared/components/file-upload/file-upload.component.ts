import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { catchError, throwError } from 'rxjs';
import { Configuration } from 'src/app/configuration/configuration';
import { RdpzsdAttachedFile } from 'src/shared/models/rdpzsd-attached-file.model';
import { FileUploadService } from 'src/shared/services/file-upload.service';

@Component({
  selector: 'file-upload',
  templateUrl: './file-upload.component.html',
  providers: [
    FileUploadService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FileUploadComponent),
      multi: true
    }
  ]
})
export class FileUploadComponent implements ControlValueAccessor {
  constructor(
    private configuration: Configuration,
    private service: FileUploadService,
  ) {
  }

  @Input() fileFormat = '';
  @Input() disabled = false;
  @Input() required = false;
  @Input() showFileUrl = false;
  @Input() fileStorageChildUrld = 'FileStorage';
  @Input() btnClass = 'btn-primary btn-sm';
  @Input() btnTimeClass = 'btn-light btn-sm';
  @Input() formControlClass = 'form-control form-control-sm';
  @Input() inputGroupClass = 'input-group input-group-sm';
  @Input() hideNoFileUpload = false;

  model: RdpzsdAttachedFile | null;
  fileUrl: string;

  // We can use this for TXT import progress bar.
  fileUploadProgress: number = null;

  private fileStorageUrl: any;

  ngOnInit(): void {
    this.fileStorageUrl = `${this.configuration.restUrl}${this.fileStorageChildUrld}`;
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

  getFile() {
    return this.service.getFile(this.fileUrl, this.model.mimeType)
      .subscribe((blob: Blob) => {
        var url = window.URL.createObjectURL(blob);
        window.open(url);
      });
  }

  uploadFile(event: any): void {
    if (this.disabled) {
      return;
    }

    const target = event.target || event.srcElement;
    const files = target.files;
    if (files.length === 1) {

      this.fileUploadProgress = 1;
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

  deleteFile(): void {
    if (this.disabled) {
      return;
    }

    this.model = null;

    this.setFileUrl();
    this.propagateChange(this.model);
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

    this.setFileUrl();
    this.propagateChange(this.model);
  }

  private setFileUrl(): void {
    if (!this.model) {
      return;
    }

    this.fileUrl = `${this.configuration.restUrl}FileStorage?key=${this.model.key}&fileName=${this.model.name}&dbId=${this.model.dbId}`;
  }

}
