using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingDetail
    {
        public int TrainingDetailId { get; set; }
        public int? TrainingMasterId { get; set; }
        public string EmployeeTraining { get; set; }
        public double? Score { get; set; }
        public double? MinScore { get; set; }
        public byte? StatusForTraining { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblEmployee EmployeeTrainingNavigation { get; set; }
        public TblTrainingMaster TrainingMaster { get; set; }
    }
}
