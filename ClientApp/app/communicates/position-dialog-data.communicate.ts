import { Injectable } from "@angular/core";
// classes
import { IPosition } from "../classes/position.class";
// services
import { AbstractDialogDataCommunicateService } from "./abstract-dialog-data.communicate";

@Injectable()
export class PositionDialogDataCommunicateService
    extends AbstractDialogDataCommunicateService<IPosition> {
    constructor() {
        super();
    }
}