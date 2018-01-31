import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
// classes
import { ITrainingLevel } from "../classes/training-level.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class TrainingLevelService extends AbstractRestService<ITrainingLevel>{
  constructor(http:Http){
      super(http, "api/TrainingLevel/");
  }
}