import { Level } from "src/shared/enums/level.enum";
import { NomenclatureCode } from "./nomenclature-code.model";

export class NomenclatureHierarchy extends NomenclatureCode {
  rootId: number;
  parentId: number;
  level: Level;
}