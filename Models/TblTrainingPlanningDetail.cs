using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingPlanningDetail
    {
        public int TrainingPlanningDetailId { get; set; }
        public int? TrainingPlanningMasterId { get; set; }
        public int? TrainingCousreId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte? Status { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblTrainingCousre TrainingCousre { get; set; }
        public TblTrainingPlanningMaster TrainingPlanningMaster { get; set; }
    }
}
