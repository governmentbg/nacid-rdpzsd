import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureCodeFilterDto } from "src/nomenclatures/dtos/nomenclature-code-filter.dto";
import { NomenclatureCode } from "src/nomenclatures/models/base/nomenclature-code.model";
import { BaseNomenclatureResource } from "./base-nomenclature.resource";

@Injectable()
export class BaseNomenclatureCodeResource<T extends NomenclatureCode, TFilter extends NomenclatureCodeFilterDto> extends BaseNomenclatureResource<T, TFilter>
{
    constructor(
        protected http: HttpClient,
        protected configuration: Configuration
    ) {
        super(http, configuration);
    }

    getByCode(nomenclatureRoute: string, code: string) {
        return this.http.get<T>(`${this.configuration.restUrl}Nomenclature/${nomenclatureRoute}/Code?code=${code}`);
    }
}