using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainning
    {
        public string TrainCode { get; set; }
        public string DocNo { get; set; }
        public string TrainName { get; set; }
        public string Place { get; set; }
        public string ExName { get; set; }
        public DateTime? Sdate { get; set; }
        public string Stime { get; set; }
        public DateTime? Edate { get; set; }
        public string Etime { get; set; }
        public float? TrainDate { get; set; }
        public float? TrainHour { get; set; }
        public float? TrainCost { get; set; }
        public string TypeCode { get; set; }
        public string Ojcode { get; set; }
        public bool? Certificate { get; set; }
        public bool? Pass { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
