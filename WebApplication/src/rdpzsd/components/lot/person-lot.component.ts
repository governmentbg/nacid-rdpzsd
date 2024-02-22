import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { StudentEventGraduatedWithDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatusGraduated } from "src/nomenclatures/models/student-status/student-status.model";
import { PersonStudentFilterDto } from "src/rdpzsd/dtos/parts/person-student-filter.dto";
import { PersonStudentStickerSearchDto } from "src/rdpzsd/dtos/search/person-student-sticker/person-student-sticker-search.dto";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { PersonStudentHistory } from "src/rdpzsd/models/parts/person-student/history/person-student-history.model";
import { PersonStudentInfo } from "src/rdpzsd/models/parts/person-student/person-student-info.model";
import { PersonStudent } from "src/rdpzsd/models/parts/person-student/person-student.model";
import { PersonLot } from "src/rdpzsd/models/person-lot.model";
import { PartResource } from "src/rdpzsd/resources/parts/part.resource";
import { PersonLotResource } from "src/rdpzsd/resources/person-lot.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { UserDataService } from "src/users/services/user-data.service";

@Component({
    selector: 'person-lot',
    templateUrl: './person-lot.component.html',
    providers: [CurrentPersonContextService, PartResource]
})
export class PersonLotComponent implements OnInit {
    personLot = new PersonLot();
    lotState = LotState;
    loadingData = false;
    activeTab = 'Basic';

    personSecondaryFromRso: boolean;

    // This is not null only if opened from stickers search
    personStudentStickerDto: PersonStudentStickerSearchDto = null;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private resource: PersonLotResource,
        public currentPersonContextService: CurrentPersonContextService,
        public userDataService: UserDataService,
        private personStudentPartResource: PartResource<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentFilterDto>
    ) {
        this.personStudentPartResource.init('PersonStudent');

        if (this.router.getCurrentNavigation().extras.state) {
            this.personStudentStickerDto = this.router.getCurrentNavigation().extras.state.personStudentStickerDto;
        }

        if (this.route.snapshot.queryParamMap.get('fromStickers') === 'true' && this.personStudentStickerDto === null) {
            this.router.navigate(['/rdpzsdStickers']);
        }
    }

    setFromRso(fromRso: boolean) {
        this.personSecondaryFromRso = fromRso;
    }

    ngOnInit() {
        this.loadingData = true;
        this.route.params.subscribe(p => {
            this.resource.getLot(p.uan)
                .subscribe(e => {
                    this.personLot = e.personLot;
                    this.currentPersonContextService.setFromLot(e, this.personStudentStickerDto);

                    this.loadingData = false;

                    this.personSecondaryFromRso = e.personSecondaryFromRso;

                    const personStudentFilterDto = new PersonStudentFilterDto();
                    personStudentFilterDto.lotId = e.personLot.id;
                    personStudentFilterDto.studentStatusAlias = StudentStatusGraduated;

                    this.activeTab = (p?.activeTab || 'Basic');

                    this.personStudentPartResource.getSearchDto(personStudentFilterDto)
                        .subscribe(e => this.currentPersonContextService.setGraduatedPersonStudents(e.result))
                });
        });
    }
}