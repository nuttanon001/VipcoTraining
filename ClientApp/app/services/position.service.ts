import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
// classes
import { IPosition } from "../classes/position.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class PositionService extends AbstractRestService<IPosition>{
    constructor(http: Http) {
        super(http, "api/Position/");
    }
}