import { Directive, Input } from "@angular/core";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { StudentEvent, StudentEventInitialRegistration } from "src/nomenclatures/models/student-status/student-event.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { BasePersonSemester } from "src/rdpzsd/models/parts/base/base-person-semester.model";
import { BasePersonStudentDoctoral } from "src/rdpzsd/models/parts/base/base-person-student-doctoral.model";
import { PartInfo } from "src/rdpzsd/models/parts/base/part-info.model";
import { Part } from "src/rdpzsd/models/parts/base/part.model";
import { PartResource } from "src/rdpzsd/resources/parts/part.resource";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { UserDataService } from "src/users/services/user-data.service";

@Directive()
export abstract class BaseStudentDoctoralPartsComponent<TPart extends BasePersonStudentDoctoral<TPartInfo, TSemester>, TPartInfo extends PartInfo, TSemester extends BasePersonSemester, THistory extends Part<THistoryInfo>, THistoryInfo extends PartInfo, TFilter extends FilterDto> {
    loadingData = false;
    parts: TPart[] = [];
    partState = PartState;
    lotState = LotState;

    studentEventInitialRegistration = StudentEventInitialRegistration;

    @Input() personLotState: LotState;

    personLotId: number;
    @Input('personLotId')
    set personLotIdSetter(personLotId: number) {
        if (personLotId) {
            this.personLotId = personLotId;
            this.loadingData = true;
            this.multiPartResource
                .getMultiParts(personLotId)
                .subscribe(e => {
                    this.parts = e;
                    this.loadingData = false;
                });
        }
    }

    constructor(
        public userDataService: UserDataService,
        protected multiPartResource: PartResource<TPart, TPartInfo, THistory, TFilter>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        protected periodNomenclatureResource: PeriodNomenclatureResource,
        protected partRouteRoute: string
    ) {
        this.multiPartResource.init(partRouteRoute);
    }

    removePart(index: number) {
        this.parts.splice(index, 1);
    }

    protected getStudentEventAlias(alias: string) {
        return new Observable((observer: Observer<any>) => {
            return this.studentEventResource.getByAlias('StudentEvent', alias)
                .pipe(
                    catchError(() => {
                        observer.complete();
                        return throwError(() => new Error('No student event found'));
                    })
                )
                .subscribe(studentEvent => {
                    observer.next(studentEvent);
                    observer.complete();
                });
        });
    }
}