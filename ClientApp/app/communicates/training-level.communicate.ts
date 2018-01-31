import { Injectable } from "@angular/core";
// classes
import { ITrainingLevel } from "../classes/training-level.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";

@Injectable()
export class TrainingLevelCommunicateService extends AbstractCommunicateService<ITrainingLevel> {
    constructor() {
        super();
    }
}