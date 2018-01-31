import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IProgramHasGroup } from "../classes/program-has-group.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class ProgramHasGroupService extends AbstractRestService<IProgramHasGroup>{
    constructor(http: Http) {
        super(http, "api/TrainingProgramHasGroup/");
    }

    // get by master id
    getByMasterId(id: number): Observable<Array<IProgramHasGroup>> {
        let url: string = this.actionUrl + "GetByProgramID/" + id;
        // debug here
        // console.log(url);
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }
}