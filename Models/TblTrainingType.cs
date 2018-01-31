using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingType
    {
        public TblTrainingType()
        {
            InverseTrainingTypeParent = new HashSet<TblTrainingType>();
            TblTrainingCousre = new HashSet<TblTrainingCousre>();
        }

        public int TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
        public string Detail { get; set; }
        public int? TrainingTypeParentId { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblTrainingType TrainingTypeParent { get; set; }
        public ICollection<TblTrainingType> InverseTrainingTypeParent { get; set; }
        public ICollection<TblTrainingCousre> TblTrainingCousre { get; set; }
    }
}
