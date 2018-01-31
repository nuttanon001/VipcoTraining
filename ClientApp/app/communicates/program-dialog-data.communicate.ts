import { Injectable } from "@angular/core";
// classes
import { ITrainingProgram } from "../classes/training-program.class";
// services
import { AbstractDialogDataCommunicateService } from "./abstract-dialog-data.communicate";

@Injectable()
export class ProgramDialogDataCommunicateService
    extends AbstractDialogDataCommunicateService<ITrainingProgram> {
    constructor() {
        super();
    }
}