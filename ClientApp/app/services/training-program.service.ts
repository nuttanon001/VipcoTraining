import { Injectable } from "@angular/core";
import { Http, ResponseContentType } from "@angular/http";
// classes
import { ITrainingProgram } from "../classes/training-program.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
import { Observable } from "rxjs/Observable";

@Injectable()
export class TrainingProgramService extends AbstractRestService<ITrainingProgram>{
    constructor(http: Http) {
        super(http, "api/TrainingPrograms/");
    }

    // get training program report
    getReportByTrainingProgramWithPosition(trainingProgramId: number): Observable<any> {
        let url: string = this.actionUrl + "GetReportByTrainingProgramWithPosition/" + trainingProgramId;
        return this.http.get(url, { responseType: ResponseContentType.Blob })
            .map(res => res.blob())
            .catch(this.handleError)
    }

    // get training program report
    getReportByTrainingProgramWithGroup(trainingProgramId: number): Observable<any> {
        let url: string = this.actionUrl + "GetReportByTrainingProgramWithGroup/" + trainingProgramId;
        return this.http.get(url, { responseType: ResponseContentType.Blob })
            .map(res => res.blob())
            .catch(this.handleError)
    }
}