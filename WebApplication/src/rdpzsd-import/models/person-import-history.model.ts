import { RdpzsdImportHistory } from "./base/rdpzsd-import-history.model";
import { PersonImportHistoryErrorFile } from "./files/person-import-history-error-file.model";
import { PersonImportHistoryFile } from "./files/person-import-history-file.model";

export class PersonImportHistory extends RdpzsdImportHistory<PersonImportHistoryFile, PersonImportHistoryErrorFile> {
}