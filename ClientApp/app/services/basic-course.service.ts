import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IBasicCourse } from "../classes/basic-course.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class BasicCourseService extends AbstractRestService<IBasicCourse>{
    constructor(http: Http) {
        super(http, "api/BasicCourse/");
    }

    // get by master id
    getByMasterId(id: number): Observable<Array<IBasicCourse>> {
        let url: string = this.actionUrl + "GetByProgramID/" + id;
        // debug here
        // console.log(url);
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }
}