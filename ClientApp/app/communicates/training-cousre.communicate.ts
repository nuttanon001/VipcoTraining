import { Injectable } from "@angular/core";
// classes
import { ITrainingCousre } from "../classes/training-cousre.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";

@Injectable()
export class TrainingCousreCommunicateService
    extends AbstractCommunicateService<ITrainingCousre> {
    constructor() {
        super();
    }
}