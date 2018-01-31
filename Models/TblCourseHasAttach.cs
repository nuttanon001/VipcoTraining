using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblCourseHasAttach
    {
        public int CourseHasAttachId { get; set; }
        public int? TrainingCousreId { get; set; }
        public int? AttactId { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblAttachFile Attact { get; set; }
        public TblTrainingCousre TrainingCousre { get; set; }
    }
}
