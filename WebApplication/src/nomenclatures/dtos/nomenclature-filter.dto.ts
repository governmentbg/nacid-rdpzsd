import { FilterDto } from "src/shared/dtos/filter.dto";

export class NomenclatureFilterDto extends FilterDto {
  name: string;
  nameAlt: string;
  getAllData = false;

  // For paginator
  currentPage = 1;
}