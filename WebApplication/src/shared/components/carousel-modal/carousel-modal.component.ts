import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { SafeResourceUrl } from '@angular/platform-browser';
import { NgbCarousel } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'carousel-modal',
    templateUrl: './carousel-modal.component.html'
})
export class CarouselModalComponent implements AfterViewInit {

    @Input() images: SafeResourceUrl[] = [];

    @ViewChild('carousel', { static: true }) carousel: NgbCarousel;

    ngAfterViewInit() {
        this.carousel.wrap = true;
        this.carousel.keyboard = false;
        this.carousel.pause();
    }
}
