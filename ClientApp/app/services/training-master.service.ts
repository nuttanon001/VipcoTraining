import { Injectable } from "@angular/core";
import { Http, RequestOptions, Headers, ResponseContentType } from "@angular/http";
// classes
import { ITrainingMaster } from "../classes/training-master.class";
import { ITrainingFilter } from "../classes/training-filter.class";
import { ITrainingReport } from "../classes/training-report.class";
import { TrainingCost } from "../classes/training-cost.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
import { Observable } from "rxjs/Observable";
@Injectable()
export class TrainingMasterService extends AbstractRestService<ITrainingMaster>{
    constructor(http: Http) {
        super(http, "api/TrainingMaster/");
    }
    // get transport-data report
    getReportByTrainingMaster(trainingId: number): Observable<any> {
        let url: string = this.actionUrl + "GetReportByTrainingMaster/" + trainingId;
        return this.http.get(url, { responseType: ResponseContentType.Blob })
            .map(res => res.blob())
            .catch(this.handleError)
    }

    // get transport-data report
    getReportEmployeeHistory(empCode: string): Observable<any> {
        let url: string = this.actionUrl + "GetReportByEmployeeCodeExcel/" + empCode;
        return this.http.get(url, { responseType: ResponseContentType.Blob })
            .map(res => res.blob())
            .catch(this.handleError)
    }

    // get report excel by training course id
    getReportExcelByTrainingCourse(trainingFilter: ITrainingFilter): Observable<any> {
        let url: string = this.actionUrl + "GetReportExcelByTrainingCourseAll/";

        return this.http.post(url, JSON.stringify(trainingFilter), this.getRequestOptionWithBlob())
            .map(res => res.blob())
            .catch(this.handleError);
    }

    // get report excel by training program id
    getReportExcelByTrainingProgram(trainingFilter: ITrainingFilter): Observable<any> {
        let url: string = this.actionUrl + "GetReportByTrainingProgramWithTrainingFilter/";

        return this.http.post(url, JSON.stringify(trainingFilter), this.getRequestOptionWithBlob())
            .map(res => res.blob())
            .catch(this.handleError);
    }

    // get report training cost by training course
    getTrainingCostFromHistory(trainingFilter: ITrainingFilter): Observable<Array<TrainingCost>> {
        return this.http.post(this.actionUrl + "GetTrainingCostFromHistory/", JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData)
            .catch(this.handleError);
    }

    // get transport-data report
    getTrainingCostFromHistoryExcel(trainingFilter: ITrainingFilter): Observable<any> {
        let url: string = this.actionUrl + "GetTrainingCostFromHistoryExcel/";
        return this.http.post(url, JSON.stringify(trainingFilter), this.getRequestOptionWithBlob())
            .map(res => res.blob())
            .catch(this.handleError);
    }

    // get report by training program
    getReportByTrainingProgram(trainingFilter: ITrainingFilter,
        subAction: string = "GetReportByTrainingProgramWithPosition/"): Observable<Array<ITrainingReport>> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // get report by training program for basic
    getReportByTrainingProgramForBasic(trainingFilter: ITrainingFilter,
        subAction: string = "GetReportByTrainingProgramForBasic/"): Observable<Array<ITrainingReport>> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // get report by training program for standard
    getReportByTrainingProgramForStandard(trainingFilter: ITrainingFilter,
        subAction: string = "GetReportByTrainingProgramForStandard/"): Observable<Array<ITrainingReport>> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // get report by training program for supplement
    getReportByTrainingProgramForSupplement(trainingFilter: ITrainingFilter,
        subAction: string = "GetReportByTrainingProgramForSupplement/"): Observable<Array<ITrainingReport>> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // get report by training course
    getReportByTrainingCourse(trainingFilter: ITrainingFilter,
        subAction: string = "GetReportByTrainingCourse/"): Observable<Array<ITrainingReport>> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // get report by training course
    getReportByEmployeeCode(trainingFilter: ITrainingFilter,
        subAction: string = "GetReportByEmployeeCode/"): Observable<Array<ITrainingReport>> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(trainingFilter), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }
}