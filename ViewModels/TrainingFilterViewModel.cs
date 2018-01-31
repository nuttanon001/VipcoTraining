using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoTraining.ViewModels
{
    public class TrainingFilterViewModel
    {
        public int TrainingId { get; set; }
        public string EmployeeCode { get; set; }
        public string LocateID { get; set; }
        public string GroupCode { get; set; }
        public string PositionCode { get; set; }
        public int GetTypeProgram { get; set; }
        public DateTime? AfterDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
