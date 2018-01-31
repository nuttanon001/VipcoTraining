using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblStandardCourse
    {
        public int StandardCourseId { get; set; }
        public int? TrainingProgramId { get; set; }
        public int? TrainingCousreId { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblTrainingCousre TrainingCousre { get; set; }
        public TblTrainingPrograms TrainingProgram { get; set; }
    }
}
