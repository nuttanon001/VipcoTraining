import { ITrainingDetail } from "./training-detail.class";

export interface ITrainingMaster {
    TrainingMasterId : number;
    TrainingCousreId? : number;
    TrainingCode? : string;
    TrainingName? : string;
    Detail? : string;
    TrainingDate?: Date;
    TrainingDateEnd?: Date;
    TrainingCost?: number;
    LecturerName? : string;
    TrainingDurationHour? : number;
    Remark? : string;
    Creator? : string;
    CreateDate? : Date;
    Modifyer? : string;
    ModifyDate?: Date;
    // from viewmodel
    TrainingDateTime?: string;
    TrainingDateEndTime?: string;
    TblTrainingDetail?: Array<ITrainingDetail>;
    PlaceId?: number;
    PlaceName?: string;
}
