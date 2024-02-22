import { FilterDto } from "src/shared/dtos/filter.dto";

export class PersonStudentFilterDto extends FilterDto {
    lotId: number;
    excludeId: number;
    studentStatusAlias: string;
    studentEventAlias: string;
    onlyMasters: boolean = false;
}