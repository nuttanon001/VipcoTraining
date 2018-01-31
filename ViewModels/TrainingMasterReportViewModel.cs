using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoTraining.ViewModels
{
    public class TrainingMasterReportViewModel
    {
        public string Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string WorkGroup { get; set; }
        public string TrainingType { get; set; }
        public string TrainingDate { get; set; }
        public string TrainingTime { get; set; }
        public string LecturerName { get; set; }
        public string Score { get; set; }
    }
}
