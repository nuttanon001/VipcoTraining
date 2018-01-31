using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoTraining.Models;

namespace VipcoTraining.ViewModels
{
    public class EmployeeLazyViewModel
    {
        public string EmpCode { get; private set; }
        public string NameThai { get; private set; }
        public string SectionString { get; private set; }
        public string GroupString { get; private set; }
        public EmployeeLazyViewModel(TblEmployee item = null)
        {
            if (item != null)
            {
                this.EmpCode = item.EmpCode;
                this.NameThai = item.NameThai;
                this.SectionString = item?.SectionCodeNavigation?.SectionName ?? "-";
                this.GroupString = item?.GroupCodeNavigation?.GroupDesc ?? "-";
            }
        }
    }

    public class EmployeeLazyViewModel2
    {
        public string EmpCode { get; set; }
        public string NameThai { get; set; }
        public string SectionString { get; set; }
        public string GroupString { get; set; }
    }
}
