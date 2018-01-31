using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblLoginName
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmpCode { get; set; }
        public bool? Salary { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
