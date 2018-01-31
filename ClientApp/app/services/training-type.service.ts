import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
// classes
import { ITrainingType } from "../classes/training-type.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class TrainingTypeService extends AbstractRestService<ITrainingType>{
  constructor(http:Http){
      super(http, "api/TrainingType/");
  }
}