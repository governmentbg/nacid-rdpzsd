import { ResearchArea } from "src/nomenclatures/models/research-area.model";
import { NomenclatureHierarchyFilterDto } from "../nomenclature-hierarchy-filter.dto";

export class ResearchAreaSearchFilterDto extends NomenclatureHierarchyFilterDto<ResearchArea> {

	constructor() {
		super();
		this.level = 2;
	}
}