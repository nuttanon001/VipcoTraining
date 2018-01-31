using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblSection
    {
        public TblSection()
        {
            TblEmployee = new HashSet<TblEmployee>();
        }

        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string SectionEname { get; set; }
        public string SectionRemark { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
    }
}
