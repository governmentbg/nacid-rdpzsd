import { EntityVersion } from "./entity-version.model";

export class RdpzsdAttachedFile extends EntityVersion{
    name: string;
    mimeType: string;
    key: string;
    hash: string;
    size: number;
    dbId: number;
}