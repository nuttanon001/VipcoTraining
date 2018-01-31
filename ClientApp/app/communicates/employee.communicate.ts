import { Injectable } from "@angular/core";
import { Subject } from "rxjs/Subject";
import { IEmployee } from "../classes/employee.class";

@Injectable()
export class EmployeeCommunicateService {
    // Observable string sources
    private EmployeeSenderSource = new Subject<Array<IEmployee>>();
    // Observable string streams
    ToSender$ = this.EmployeeSenderSource.asObservable();
    // Service message commands
    toSender(employees: Array<IEmployee>): void {
        this.EmployeeSenderSource.next(employees);
    }
}