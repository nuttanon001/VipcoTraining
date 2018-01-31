using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblDepartment
    {
        public TblDepartment()
        {
            TblEmployee = new HashSet<TblEmployee>();
        }

        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string DeptEname { get; set; }
        public string DeptRemark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
    }
}
