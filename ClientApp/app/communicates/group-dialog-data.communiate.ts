import { Injectable } from "@angular/core";
// classes
import { IGroup } from "../classes/group.class";
// services
import { AbstractDialogDataCommunicateService } from "./abstract-dialog-data.communicate";

@Injectable()
export class GroupDialogDataCommunicateService
    extends AbstractDialogDataCommunicateService<IGroup> {
    constructor() {
        super();
    }
}