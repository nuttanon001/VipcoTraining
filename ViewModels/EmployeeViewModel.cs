using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoTraining.Models;
namespace VipcoTraining.ViewModels
{
    public class EmployeeViewModel:TblEmployee
    {
        //public string DepartmentString => this.DeptCodeNavigation == null ? "" : this.DeptCodeNavigation?.DeptName ?? "";
        //public string SectionString => this.SectionCodeNavigation == null ? "" : this.SectionCodeNavigation?.SectionName ?? "";
        //public string DivisionString => this.DivisionCodeNavigation == null ? "" : this.DivisionCodeNavigation?.DivisionName ?? "";
        //public string GroupString => this.GroupCodeNavigation == null ? "" : this.GroupCodeNavigation?.GroupDesc ?? "";
        //public string EmployeeTypeString => this.EmptypeCodeNavigation == null ? "" : this.EmptypeCodeNavigation?.EmpTypeDesc ?? "";
        //public string LocationString => this.Locate == null ? "" : this.Locate?.LocateDesc ?? "";

        public string DepartmentString { get; set; }
        public string SectionString { get; set; }
        public string DivisionString { get; set; }
        public string GroupString { get; set; }
        public string EmployeeTypeString { get; set; }
        public string LocationString { get; set; }
        public string EmployeePicture { get; set; }

        //public EmployeeViewModel (TblEmployee data)
        //{
        //    this.DepartmentString = data?.DeptCodeNavigation?.DeptName ?? "";
        //    this.SectionString = data?.SectionCodeNavigation?.SectionName ?? "";
        //    this.DivisionString = data?.DivisionCodeNavigation?.DivisionName ?? "";
        //    this.GroupString = data?.GroupCodeNavigation?.GroupDesc ?? "";
        //    this.EmployeeTypeString = data?.EmptypeCodeNavigation?.EmpTypeDesc ?? "";
        //    this.LocationString = data?.Locate?.LocateDesc ?? "";
        //}
    }
}
