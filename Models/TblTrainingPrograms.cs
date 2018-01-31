using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingPrograms
    {
        public TblTrainingPrograms()
        {
            TblBasicCourse = new HashSet<TblBasicCourse>();
            TblStandardCourse = new HashSet<TblStandardCourse>();
            TblSupplementCourse = new HashSet<TblSupplementCourse>();
            TblTrainingProgramHasGroup = new HashSet<TblTrainingProgramHasGroup>();
            TblTrainingProgramHasPosition = new HashSet<TblTrainingProgramHasPosition>();
        }

        public int TrainingProgramId { get; set; }
        public string TrainingProgramCode { get; set; }
        public string TrainingProgramName { get; set; }
        public double? TrainingProgramLeave { get; set; }
        public string TrainingProgramLevelString { get; set; }
        public string Detail { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblBasicCourse> TblBasicCourse { get; set; }
        public ICollection<TblStandardCourse> TblStandardCourse { get; set; }
        public ICollection<TblSupplementCourse> TblSupplementCourse { get; set; }
        public ICollection<TblTrainingProgramHasGroup> TblTrainingProgramHasGroup { get; set; }
        public ICollection<TblTrainingProgramHasPosition> TblTrainingProgramHasPosition { get; set; }
    }
}
