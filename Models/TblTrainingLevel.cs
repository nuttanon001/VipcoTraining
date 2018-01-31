using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingLevel
    {
        public TblTrainingLevel()
        {
            TblTrainingCousre = new HashSet<TblTrainingCousre>();
        }

        public int TrainingLevelId { get; set; }
        public string TrainingLevel { get; set; }
        public string Detail { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblTrainingCousre> TblTrainingCousre { get; set; }
    }
}
