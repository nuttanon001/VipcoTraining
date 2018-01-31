using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingMasterHasPlace
    {
        public int TrainingMasterHasPlaceId { get; set; }
        public int? TrainingMasterId { get; set; }
        public int? PlaceId { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblPlace Place { get; set; }
        public TblTrainingMaster TrainingMaster { get; set; }
    }
}
