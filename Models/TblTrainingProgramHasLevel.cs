using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingProgramHasLevel
    {
        public int ProgramHasLevelId { get; set; }
        public int? TrainingProgramId { get; set; }
        public int? TrainingLevelId { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
