import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
// classes
import { IEducation } from "../classes/education.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class EducationService extends AbstractRestService<IEducation>{
  constructor(http:Http){
      super(http, "api/Education/");
  }
}