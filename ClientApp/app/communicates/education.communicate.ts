import { Injectable } from "@angular/core";
// classes
import { IEducation } from "../classes/education.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";

@Injectable()
export class EducationCommunicateService extends AbstractCommunicateService<IEducation> {
    constructor() {
        super();
    }
}