using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblPosition
    {
        public TblPosition()
        {
            TblEmployee = new HashSet<TblEmployee>();
            TblTrainingProgramHasPosition = new HashSet<TblTrainingProgramHasPosition>();
        }

        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string PositionEname { get; set; }
        public string PositionRemark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
        public ICollection<TblTrainingProgramHasPosition> TblTrainingProgramHasPosition { get; set; }
    }
}
