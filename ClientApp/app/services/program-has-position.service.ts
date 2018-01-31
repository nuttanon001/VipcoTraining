import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IProgramHasPosition } from "../classes/program-has-position.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class ProgramHasPositionService extends AbstractRestService<IProgramHasPosition>{
    constructor(http: Http) {
        super(http, "api/TrainingProgramHasPosition/");
    }

    // get by master id
    getByMasterId(id: number): Observable<Array<IProgramHasPosition>> {
        let url: string = this.actionUrl + "GetByProgramID/" + id;
        // debug here
        // console.log(url);
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }
}