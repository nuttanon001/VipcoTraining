using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingPlanningMaster
    {
        public TblTrainingPlanningMaster()
        {
            TblTrainingPlanningDetail = new HashSet<TblTrainingPlanningDetail>();
        }

        public int TrainingPlanningMasterId { get; set; }
        public DateTime? PlanningForYear { get; set; }
        public string PlanningName { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public byte? Status { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblTrainingPlanningDetail> TblTrainingPlanningDetail { get; set; }
    }
}
