import { Injectable } from "@angular/core";
// classes
import { ITrainingCousre } from "../classes/training-cousre.class";
// services
import { AbstractDialogDataCommunicateService } from "./abstract-dialog-data.communicate";

@Injectable()
export class CousreDialogDataCommunicateService
    extends AbstractDialogDataCommunicateService<ITrainingCousre> {
    constructor() {
        super();
    }
}