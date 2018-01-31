import { IBasicCourse } from "./basic-course.class";
import { IStandardCourse } from "./standard-course.class";
import { ISupplementCourse } from "./supplement-course.class";
import { IProgramHasPosition } from "./program-has-position.class";
import { IProgramHasGroup } from "./program-has-group.class";

export interface ITrainingProgram {
    TrainingProgramId: number;
    TrainingProgramCode?: string;
    TrainingProgramName?: string;
    TrainingProgramLeave?: number;
    TrainingProgramLevelString?: string;
    Detail?: string;
    Remark?: string;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    TblBasicCourse?: Array<IBasicCourse>;
    TblStandardCourse?: Array<IStandardCourse>;
    TblSupplementCourse?: Array<ISupplementCourse>;
    TblTrainingProgramHasPosition?: Array<IProgramHasPosition>;
    TblTrainingProgramHasGroup?: Array<IProgramHasGroup>;
}