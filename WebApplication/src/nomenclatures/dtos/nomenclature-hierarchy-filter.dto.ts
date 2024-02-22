import { Level } from "src/shared/enums/level.enum";
import { NomenclatureHierarchy } from "../models/base/nomenclature-hierarchy.model";
import { NomenclatureCodeFilterDto } from "./nomenclature-code-filter.dto";

export class NomenclatureHierarchyFilterDto<T extends NomenclatureHierarchy> extends NomenclatureCodeFilterDto {
  level: Level;
  excludeLevel: boolean;
  rootId: number;
  root: T;
  parentId: number;
  parent: T;
}