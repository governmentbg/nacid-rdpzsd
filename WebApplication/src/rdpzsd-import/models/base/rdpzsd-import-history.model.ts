import { ImportState } from "src/rdpzsd-import/enums/import-state.enum";
import { EntityVersion } from "src/shared/models/entity-version.model";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";

export class RdpzsdImportHistory<TFile extends RdpzsdAttachedFile, TErrorFile extends RdpzsdAttachedFile> extends EntityVersion {
    rdpzsdImportId: number;
    state: ImportState;
    createDate: Date;
    importFile: TFile;
    errorFile: TErrorFile;
}