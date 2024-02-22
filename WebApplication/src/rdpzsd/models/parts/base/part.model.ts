import { PartState } from "src/rdpzsd/enums/part-state.enum";
import { EntityVersion } from "src/shared/models/entity-version.model";
import { PartInfo } from "./part-info.model";

export class Part<TPartInfo extends PartInfo> extends EntityVersion {
    partInfo: TPartInfo;
    state: PartState;
}