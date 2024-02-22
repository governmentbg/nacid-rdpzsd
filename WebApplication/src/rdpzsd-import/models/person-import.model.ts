import { RdpzsdImport } from "./base/rdpzsd-import.model";
import { PersonImportErrorFile } from "./files/person-import-error-file.model";
import { PersonImportFile } from "./files/person-import-file.model";
import { PersonImportHistoryErrorFile } from "./files/person-import-history-error-file.model";
import { PersonImportHistoryFile } from "./files/person-import-history-file.model";
import { PersonImportHistory } from "./person-import-history.model";

export class PersonImport extends RdpzsdImport<PersonImportFile, PersonImportErrorFile, PersonImportHistory, PersonImportHistoryFile, PersonImportHistoryErrorFile> {
}