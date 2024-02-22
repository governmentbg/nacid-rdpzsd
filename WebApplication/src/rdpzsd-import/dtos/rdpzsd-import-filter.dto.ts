import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { ImportState } from "../enums/import-state.enum";

export class RdpzsdImportFilterDto extends FilterDto {
    state: ImportState;
    institutionId: number;
    institution: Institution;
    subordinateId: number;
    subordinate: Institution;
}