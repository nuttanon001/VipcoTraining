import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
// classes
import { ILocation } from "../classes/location.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class LocationService extends AbstractRestService<ILocation>{
    constructor(http: Http) {
        super(http, "api/Location/");
    }
}