import { RdpzsdImport } from "./base/rdpzsd-import.model";
import { SpecialityImportErrorFile } from "./files/speciality-import-error-file.model";
import { SpecialityImportFile } from "./files/speciality-import-file.model";
import { SpecialityImportHistoryErrorFile } from "./files/speciality-import-history-error-file.model";
import { SpecialityImportHistoryFile } from "./files/speciality-import-history-file.model";
import { SpecialityImportHistory } from "./speciality-import.history.model";

export class SpecialityImport extends RdpzsdImport<SpecialityImportFile, SpecialityImportErrorFile, SpecialityImportHistory, SpecialityImportHistoryFile, SpecialityImportHistoryErrorFile> {
}