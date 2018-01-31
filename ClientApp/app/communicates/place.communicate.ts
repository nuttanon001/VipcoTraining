import { Injectable } from "@angular/core";
// classes
import { IPlace } from "../classes/place.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";

@Injectable()
export class PlaceCommunicateService extends AbstractCommunicateService<IPlace> {
    constructor() {
        super();
    }
}