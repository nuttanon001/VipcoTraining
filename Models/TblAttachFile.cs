using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblAttachFile
    {
        public TblAttachFile()
        {
            TblCourseHasAttach = new HashSet<TblCourseHasAttach>();
        }

        public int AttactId { get; set; }
        public string AttachFileName { get; set; }
        public string AttachAddress { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblCourseHasAttach> TblCourseHasAttach { get; set; }
    }
}
