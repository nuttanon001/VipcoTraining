using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingProgramHasGroup
    {
        public int ProgramHasGroupId { get; set; }
        public int? TrainingProgramId { get; set; }
        public string GroupCode { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblGroupName GroupCodeNavigation { get; set; }
        public TblTrainingPrograms TrainingProgram { get; set; }
    }
}
