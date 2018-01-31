import { Injectable } from "@angular/core";
// classes
import { IEmployee } from "../classes/employee.class";
// services
import { AbstractDialogDataCommunicateService } from "./abstract-dialog-data.communicate";

@Injectable()
export class EmployeeDialogDataCommunicateService
    extends AbstractDialogDataCommunicateService<IEmployee> {
    constructor() {
        super();
    }
}