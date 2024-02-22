import { Component, OnInit } from '@angular/core';
import { ResearchArea } from 'src/nomenclatures/models/research-area.model';
import { BaseNomenclatureComponent } from '../base/base-nomenclature.component';
import { BaseNomenclatureCodeResource } from 'src/nomenclatures/resources/base/base-nomenclature-code.resource';
import { ResearchAreaSearchFilterDto } from 'src/nomenclatures/dtos/others/research-area-search-filter.dto';

@Component({
	selector: 'research-area-search',
	templateUrl: './research-area-search.component.html',
	providers: [BaseNomenclatureCodeResource]
})
export class ResearchAreaSearchComponent extends BaseNomenclatureComponent<ResearchArea, ResearchAreaSearchFilterDto> implements OnInit {

	constructor(
		baseNomenclatureResource: BaseNomenclatureCodeResource<ResearchArea, ResearchAreaSearchFilterDto>) {
		super(baseNomenclatureResource, ResearchAreaSearchFilterDto, 'ResearchArea');
	}

	ngOnInit() {
		return this.getData();
	}

}
