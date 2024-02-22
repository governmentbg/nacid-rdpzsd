import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { BreadcrumbItem } from './breadcrumb-item.model';

@Component({
  selector: 'breadcrumb',
  templateUrl: './breadcrumb.component.html'
})
export class BreadcrumbComponent implements OnInit {

  breadcrumbItems: BreadcrumbItem[] = [];

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
  }

  private createBreadcrumbs(route: ActivatedRoute, url: string = '') {
    const children: ActivatedRoute[] = route.children;

    for (const child of children) {
      const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
      if (routeURL !== '') {
        url += `/${routeURL}`;
      }

      const label = child.snapshot.data['title'];
      if (label) {
        this.breadcrumbItems.push(new BreadcrumbItem(url, label));
      }

      this.createBreadcrumbs(child, url);
    }
  }

  ngOnInit() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.breadcrumbItems = [];
        this.createBreadcrumbs(this.activatedRoute.root);
      });
  }
}
