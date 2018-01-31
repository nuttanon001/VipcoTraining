using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainEmployee
    {
        public string TrainCode { get; set; }
        public string EmpCode { get; set; }
        public string EmptypeCode { get; set; }
        public string DeptCode { get; set; }
        public string DivisionCode { get; set; }
        public string SectionCode { get; set; }
        public string GroupCode { get; set; }
        public string PositionCode { get; set; }
        public string LocateId { get; set; }
        public float? Cost { get; set; }
        public string Remark { get; set; }
        public bool? Status { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Cert { get; set; }
    }
}
