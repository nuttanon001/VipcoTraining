using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingMaster
    {
        public TblTrainingMaster()
        {
            TblTrainingDetail = new HashSet<TblTrainingDetail>();
            TblTrainingMasterHasPlace = new HashSet<TblTrainingMasterHasPlace>();
        }

        public int TrainingMasterId { get; set; }
        public int? TrainingCousreId { get; set; }
        public string TrainingCode { get; set; }
        public double? TrainingCost { get; set; }
        public string TrainingName { get; set; }
        public string Detail { get; set; }
        public DateTime? TrainingDate { get; set; }
        public DateTime? TrainingDateEnd { get; set; }
        public string LecturerName { get; set; }
        public double? TrainingDurationHour { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblTrainingCousre TrainingCousre { get; set; }
        public ICollection<TblTrainingDetail> TblTrainingDetail { get; set; }
        public ICollection<TblTrainingMasterHasPlace> TblTrainingMasterHasPlace { get; set; }
    }
}
