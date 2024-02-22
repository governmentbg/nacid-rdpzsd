import { Injectable } from '@angular/core';
import { StudentEvent } from 'src/nomenclatures/models/student-status/student-event.model';
import { StudentStatus } from 'src/nomenclatures/models/student-status/student-status.model';

@Injectable()
export class StudentStatusChangeService {

    studentStatusChange(entity: any, studentStatus: StudentStatus, studentStatusName: string, studentEventName: string = null) {
        if (studentStatus) {
            entity[studentStatusName] = studentStatus;
            entity[`${studentStatusName}Id`] = studentStatus.id;
        } else {
            entity[studentStatusName] = null;
            entity[`${studentStatusName}Id`] = null;
        }

        if (studentEventName) {
            entity[studentEventName] = null;
            entity[`${studentEventName}Id`] = null;
        }
    }

    studentEventChange(entity: any, studentEvent: StudentEvent, studentEventName: string, studentStatusName: string = null) {
        if (studentEvent) {
            if (studentStatusName) {
                entity[studentStatusName] = studentEvent.studentStatus;
                entity[`${studentStatusName}Id`] = studentEvent.studentStatusId;
            }

            entity[studentEventName] = studentEvent;
            entity[`${studentEventName}Id`] = studentEvent.id;
        } else {
            entity[studentEventName] = null;
            entity[`${studentEventName}Id`] = null;
        }
    }
}
