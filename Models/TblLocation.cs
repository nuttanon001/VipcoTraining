using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblLocation
    {
        public TblLocation()
        {
            TblEmployee = new HashSet<TblEmployee>();
        }

        public string LocateId { get; set; }
        public string LocateDesc { get; set; }
        public string Remark { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Soil { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string Amphur { get; set; }
        public string Province { get; set; }
        public string Post { get; set; }
        public string Telephone { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblEmployee> TblEmployee { get; set; }
    }
}
