// core modules
import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions, ResponseContentType } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IEmployee,LazyLoad } from "../classes/class.index";
// timezone
import * as moment from "moment-timezone";

@Injectable()
export class EmployeeService {
    constructor(
        private http: Http,
    ) { }

    // base url
    private baseUrl: string = "api/Employee/";
    // extract data
    private extractData(r: Response) { // for extractdata
        let body = r.json();
        // console.log(body);
        return body || {};
    }
    // extract data for result code
    private extractResultCode(res: Response) {
        if (res) {
            if (res.status === 201) {
                return [{ status: res.status, json: res }]
            }
            else if (res.status === 200) {
                return [{ status: res.status, json: res }]
            }
        }
    }
    // extract with date type not string date
    private extractDataForDate(res: Response) { // for extract change date type
        let body = res.json();

        let data: IEmployee = res.json();
        data.BeginDate = data.BeginDate !== null ? new Date(data.BeginDate) : new Date();
        data.OutDate = data.OutDate !== null ? new Date(data.OutDate) : new Date();
        data.ResidentDate = data.ResidentDate !== null ? new Date(data.ResidentDate) : new Date();

        return data;
    }
    // handle error
    private handleError(error: Response | any) {// for error message
        // In a real world app, we might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = error.status + " - " + (error.statusText || '') + err;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
    // get request option
    private getRequestOption(): RequestOptions {   // for request option
        return new RequestOptions({
            headers: new Headers({
                "Content-Type": "application/json"
            })
        });
    }
    // change timezone date
    private changeTimezone(data: IEmployee): IEmployee {
        var zone = "Asia/Bangkok";
        if (data !== null) {
            if (data.BeginDate !== null) {
                data.BeginDate = moment.tz(data.BeginDate, zone).toDate();
            }
            if (data.OutDate !== null) {
                data.OutDate = moment.tz(data.OutDate, zone).toDate();
            }
            if (data.ResidentDate !== null) {
                data.ResidentDate = moment.tz(data.ResidentDate, zone).toDate();
            }
        }
        return data;
    }

    //===================== Employee =============================\\
    // get employee all
    getEmployeesAll(): Observable<Array<IEmployee>> {
        let url: string = this.baseUrl;
        return this.http.get(url).map(this.extractData).catch(this.handleError);
    }

    // get employee by filter
    getEmployeeByFilter(filter?: string): Observable<Array<IEmployee>> {
        let url: string = this.baseUrl + "FindAll/";
        if (filter) { url += filter + "/"; }
        return this.http.get(url).map(this.extractData).catch(this.handleError);
    }

    // get employee by id
    getEmployeeById(EmployeeCode: string): Observable<IEmployee> {
        let url: string = this.baseUrl + EmployeeCode + "/";
        return this.http.get(url).map(this.extractDataForDate).catch(this.handleError);
    }

    // get employee with lazyload
    getEmployeeWithLazyLoadByPost(lazyLoad: LazyLoad): Observable<any> {
        let url: string = this.baseUrl + "FindAllLayzLoad2" + "/";
        return this.http.post(
            url, JSON.stringify(lazyLoad), this.getRequestOption()
        ).map(this.extractData).catch(this.handleError);
    }

    // post new employee
    postEmployee(nEmployee: IEmployee): Observable<IEmployee> {
        nEmployee = this.changeTimezone(nEmployee);
        let url: string = this.baseUrl;
        return this.http.post(
            url, JSON.stringify(nEmployee), this.getRequestOption()
        ).map(this.extractData).catch(this.handleError);
    }

    // put update employee
    putEmployee(id: string, uEmployee: IEmployee): Observable<IEmployee> {
        uEmployee = this.changeTimezone(uEmployee);
        let url: string = this.baseUrl + id + "/";
        return this.http.put(
            url, JSON.stringify(uEmployee), this.getRequestOption()
        ).map(this.extractData).catch(this.handleError);
    }
    //===================== End Employee =========================\\
}