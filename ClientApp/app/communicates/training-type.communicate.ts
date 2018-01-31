import { Injectable } from "@angular/core";
// classes
import { ITrainingType } from "../classes/training-type.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";

@Injectable()
export class TrainingTypeCommunicateService extends AbstractCommunicateService<ITrainingType> {
    constructor() {
        super();
    }
}