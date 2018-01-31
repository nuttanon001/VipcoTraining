using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblEmpType
    {
        public TblEmpType()
        {
            TblEmployee = new HashSet<TblEmployee>();
        }

        public string EmpTypeCode { get; set; }
        public string EmpTypeDesc { get; set; }
        public string EmpTypeRemark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
    }
}
