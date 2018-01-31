export interface ITrainingDetail {
    TrainingDetailId: number;
    TrainingMasterId?: number;
    EmployeeTraining?: string;
    Score?: number;
    MinScore?: number;
    StatusForTraining?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // from viewmodel
    EmployeeNameString?: string;
    StatusForTrainingString?: string;
}