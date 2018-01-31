using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingRequestMaster
    {
        public TblTrainingRequestMaster()
        {
            TblTrainingRequestDetail = new HashSet<TblTrainingRequestDetail>();
        }

        public int TrainingRequestMasterId { get; set; }
        public int? TrainingCousreId { get; set; }
        public DateTime? DateRequest { get; set; }
        public string EmployeeRequest { get; set; }
        public byte? Status { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblEmployee EmployeeRequestNavigation { get; set; }
        public TblTrainingCousre TrainingCousre { get; set; }
        public ICollection<TblTrainingRequestDetail> TblTrainingRequestDetail { get; set; }
    }
}
