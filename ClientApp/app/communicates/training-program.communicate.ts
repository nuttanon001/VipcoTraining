import { Injectable } from "@angular/core";
import { Subject } from "rxjs/Subject";
// classes
import { ITrainingProgram} from "../classes/training-program.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";


@Injectable()
export class TrainingProgramCommunicateService
    extends AbstractCommunicateService<ITrainingProgram> {
    constructor() {
        super();
    }
}