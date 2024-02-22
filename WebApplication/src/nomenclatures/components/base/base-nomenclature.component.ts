import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { Nomenclature } from "src/nomenclatures/models/base/nomenclature.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";
import { saveAs } from 'file-saver';

export abstract class BaseNomenclatureComponent<T extends Nomenclature, TFilter extends NomenclatureFilterDto> {

  filter: TFilter = this.initializeFilter(this.structuredFilterType);
  searchResult: SearchResultDto<T> = new SearchResultDto<T>();
  loadingData = false;

  constructor(
    protected baseNomenclatureResource: BaseNomenclatureResource<T, TFilter>,
    protected structuredFilterType: new () => TFilter,
    protected nomenclatureRoute: string
  ) {
  }

  initializeFilter(C: { new(): TFilter }) {
    return new C();
  }

  clear() {
    this.filter = this.initializeFilter(this.structuredFilterType);
    return this.getData(false);
  }

  getData(triggerLoadingDataIndicator = true) {
    if (triggerLoadingDataIndicator) {
      this.loadingData = true;
    }

    this.filter.offset = (this.filter.currentPage - 1) * this.filter.limit;
    return this.baseNomenclatureResource
      .getAll(this.nomenclatureRoute, this.filter)
      .subscribe(e => {
        this.searchResult = e;
        this.loadingData = false;
      });
  }

  exportExcel() {
    return this.baseNomenclatureResource.exportExcel(this.nomenclatureRoute, this.filter)
      .subscribe((blob: Blob) => {
        saveAs(blob, `${this.nomenclatureRoute}.xlsx`);
      });
  }
}
