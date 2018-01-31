using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblDivision
    {
        public TblDivision()
        {
            TblEmployee = new HashSet<TblEmployee>();
        }

        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DivisionEname { get; set; }
        public string DivisionRemark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
    }
}
