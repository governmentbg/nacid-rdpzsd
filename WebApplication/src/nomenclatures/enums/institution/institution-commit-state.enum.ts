export enum InstitutionCommitState {
    initialDraft = 1,
    modification = 2,
    actual = 3,
    actualWithModification = 4,
    history = 5,
    deleted = 6,
    commitReady = 7,
    modificationErase = 8,
    modificationRevertErase = 9
}