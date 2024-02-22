import { Nomenclature } from "../base/nomenclature.model";

export const StudentStatusActive = 'active';
export const StudentStatusInterrupted = 'interrupted';
export const StudentStatusCompleted = 'completed';
export const StudentStatusGraduated = 'graduated';
export const StudentStatusProcessGraduation = 'processGraduation';

export class StudentStatus extends Nomenclature {
    alias: string;
}