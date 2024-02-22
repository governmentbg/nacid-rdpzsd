import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import * as saveAs from "file-saver";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { Nomenclature } from "src/nomenclatures/models/base/nomenclature.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";

export abstract class BaseEditableNomenclatureComponent<T extends Nomenclature, TFilter extends NomenclatureFilterDto>{

  filter: TFilter = this.initializeFilter(this.structuredFilterType);
  searchResult: SearchResultDto<T> = new SearchResultDto<T>();
  loadingData = false;

  constructor(
    protected baseNomenclatureResource: BaseNomenclatureResource<T, TFilter>,
    protected structuredFilterType: new () => TFilter,
    protected nomenclatureRoute: string,
    protected modalService: NgbModal,
    protected nomenclature: new () => T,
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

  add() {
    let newItem = new this.nomenclature();
    newItem.isActive = true;
    newItem.isEditMode = true;

    this.searchResult.result.unshift(newItem);
  }

  save(item: T) {
    item.originalItem = null;

    if (!item.id) {
      return this.baseNomenclatureResource.create(this.nomenclatureRoute, item)
        .subscribe(result => {
          this.searchResult.result[this.searchResult.result.indexOf(item, 0)] = result;
        });
    }
    else {
      return this.baseNomenclatureResource.update(this.nomenclatureRoute, item)
        .subscribe(result => {
          this.searchResult.result[this.searchResult.result.indexOf(item, 0)] = result;
        });
    }
  }

  edit(item: T) {
    item.originalItem = new this.nomenclature();
    Object.assign(item.originalItem, item);
    item.isEditMode = true;
  }

  delete(id: number) {
    const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
    modal.componentInstance.text = 'rdpzsd.personLot.modals.deleteNomenclature';
    modal.componentInstance.acceptButton = 'root.buttons.delete';
    modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

    modal.result.then((ok) => {
      if (ok) {
        return this.baseNomenclatureResource
          .delete(this.nomenclatureRoute, id)
          .subscribe(() => {
            const item = this.searchResult.result.find(e => e.id === id);
            const index = this.searchResult.result.indexOf(item);
            this.searchResult.result.splice(index, 1);
          })
      }

      return null;
    });
  }

  cancel(item: any) {
    if (!item.id) {
      this.searchResult.result.splice(this.searchResult.result.indexOf(item, 0), 1);
    }
    else {
      Object.keys(item).forEach((key) => {
        item[key] = item.originalItem[key];
      });
      item.isEditMode = false;
      item.originalItem = null;
    }
  }
}