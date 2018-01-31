using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblGroupName
    {
        public TblGroupName()
        {
            TblEmployee = new HashSet<TblEmployee>();
            TblTrainingProgramHasGroup = new HashSet<TblTrainingProgramHasGroup>();
        }

        public string GroupCode { get; set; }
        public string GroupDesc { get; set; }
        public string GroupEdesc { get; set; }
        public string GroupRemark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
        public ICollection<TblTrainingProgramHasGroup> TblTrainingProgramHasGroup { get; set; }
    }
}
