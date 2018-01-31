export interface ITrainingCousre {
    TrainingCousreId: number;
    TrainingCousreCode: string;
    TrainingLevelId?: number;
    TrainingTypeId?: number;
    TrainingCousreName: string;
    Detail?: string;
    EducationRequirementId?: number;
    WorkExperienceRequirement?: number;
    MinimunScore?: number;
    BaseCost?: number;
    Remark?: string;
    Status?: number;
    TrainingDurationHour?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    EducationRequirementString?: string;
    TrainingLevelString?: string;
    TrainingTypeString?: string;
    AttachFile?: FileList;
    RemoveAttach?: Array<number>;
}
