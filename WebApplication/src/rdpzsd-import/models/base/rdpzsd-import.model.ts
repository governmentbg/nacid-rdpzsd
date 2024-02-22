import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { ImportState } from "src/rdpzsd-import/enums/import-state.enum";
import { EntityVersion } from "src/shared/models/entity-version.model";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";
import { RdpzsdImportHistory } from "./rdpzsd-import-history.model";

export class RdpzsdImport<TFile extends RdpzsdAttachedFile, TErrorFile extends RdpzsdAttachedFile, TImportHistory extends RdpzsdImportHistory<TImportHistoryFile, TImportHistoryErrorFile>, TImportHistoryFile extends RdpzsdAttachedFile, TImportHistoryErrorFile extends RdpzsdAttachedFile> extends EntityVersion {
    state: ImportState;
    userId: number;
    userFullname: string;
    userEmail: string;
    institutionId: number;
    institution: Institution;
    subordinateId: number;
    subordinate: Institution;
    createDate: Date;
    finishDate: Date;
    entitiesAcceptedCount: number;
    entitiesCount: number;
    firstCriteriaAcceptedCount: number;
    firstCriteriaCount: number;
    secondCriteriaAcceptedCount: number;
    secondCriteriaCount: number;
    importFile: TFile;
    errorFile: TErrorFile;
    importHistories: TImportHistory[] = [];
}