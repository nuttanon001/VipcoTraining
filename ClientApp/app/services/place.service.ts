import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IPlace } from "../classes/place.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class PlaceService extends AbstractRestService<IPlace>{
    constructor(http: Http) {
        super(http, "api/Place/");
    }
}