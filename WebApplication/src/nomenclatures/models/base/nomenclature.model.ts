import { EntityVersion } from "src/shared/models/entity-version.model";

export class Nomenclature extends EntityVersion {
  name: string;
  nameAlt: string;
  isActive = true;
  viewOrder: number;
  isEditMode: boolean;
  originalItem: any;
}