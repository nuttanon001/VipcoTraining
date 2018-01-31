import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { ITrainingDetail } from "../classes/training-detail.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class TrainingDetailService extends AbstractRestService<ITrainingDetail>{
    constructor(http: Http) {
        super(http, "api/TrainingDetail/");
    }

    // get by master id
    getByMasterId(id: number): Observable<Array<ITrainingDetail>> {
        let url: string = this.actionUrl + "GetByMasterID/" + id;
        // debug here
        // console.log(url);
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }
}