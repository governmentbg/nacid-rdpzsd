import { SearchResultDto } from "src/shared/dtos/search-result.dto";
import { saveAs } from 'file-saver';
import { InstitutionSpecialityFilterDto } from "src/nomenclatures/dtos/institution/institution-speciality-filter.dto";
import { InstitutionSpecialityNomenclatureResource } from "src/nomenclatures/resources/institution-speciality-nomenclature.resource";
import { InstitutionSpeciality } from "src/nomenclatures/models/institution/institution-speciality.model";
import { Component, OnInit } from "@angular/core";
import { Level } from "src/shared/enums/level.enum";
import { TranslateService } from "@ngx-translate/core";
import { Doctor } from "src/nomenclatures/models/institution/educational-qualification.model";
import { UserDataService } from "src/users/services/user-data.service";
import { InstitutionChangeService } from "src/shared/services/institutions/institution-change.service";
import { InstitutionSpecialityJointSpeciality } from "src/nomenclatures/models/institution/institution-speciality-joint-speciality.model";

@Component({
  selector: 'institution-speciality-search',
  templateUrl: './institution-speciality-search.component.html'
})
export class InstitutionSpecialitySearchComponent implements OnInit {

  filter: InstitutionSpecialityFilterDto = this.initializeFilter();
  searchResult: SearchResultDto<InstitutionSpeciality> = new SearchResultDto<InstitutionSpeciality>();
  level = Level;
  loadingData = false;
  doctor = Doctor;

  constructor(
    private institutionSpecialityResource: InstitutionSpecialityNomenclatureResource,
    public translateService: TranslateService,
    public userDataService: UserDataService,
    public institutionChangeService: InstitutionChangeService
  ) {
  }

  initializeFilter() {
    var institutionSpecialityFilter = new InstitutionSpecialityFilterDto();
    institutionSpecialityFilter.isActive = true;
    return institutionSpecialityFilter;
  }

  clear() {
    this.filter = this.initializeFilter();
    return this.getData(false);
  }

  getData(triggerLoadingDataIndicator = true) {
    if (triggerLoadingDataIndicator) {
      this.loadingData = true;
    }

    this.filter.offset = (this.filter.currentPage - 1) * this.filter.limit;
    return this.institutionSpecialityResource
      .getAll(this.filter)
      .subscribe(e => {
        this.searchResult = e;
        this.loadingData = false;
      });
  }

  exportExcel() {
    return this.institutionSpecialityResource.exportExcel(this.filter)
      .subscribe((blob: Blob) => {
        saveAs(blob, 'Speciality.xlsx');
      });
  }

  ngOnInit() {
    return this.getData();
  }
  institutionSpecialityJointSpecialitiesByLocation(jointSpecialities: any[], location: number): InstitutionSpecialityJointSpeciality[] {
	return jointSpecialities.filter(e => e.location == location);
}
}
