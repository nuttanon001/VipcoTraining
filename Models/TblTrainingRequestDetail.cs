using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingRequestDetail
    {
        public int TrainingRequestDetailId { get; set; }
        public int? TrainingRequestMasterId { get; set; }
        public string EmployeeNeedTraining { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblEmployee EmployeeNeedTrainingNavigation { get; set; }
        public TblTrainingRequestMaster TrainingRequestMaster { get; set; }
    }
}
