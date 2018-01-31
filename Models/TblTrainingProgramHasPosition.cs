using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingProgramHasPosition
    {
        public int ProgramHasPositionId { get; set; }
        public int? TrainingProgramId { get; set; }
        public string PositionCode { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblPosition PositionCodeNavigation { get; set; }
        public TblTrainingPrograms TrainingProgram { get; set; }
    }
}
