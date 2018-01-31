using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoTraining.ViewModels
{
    public class EmpHistoryTrainingViewModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string Section { get; set; }
        public List<CourseHistory> HistoryCourses { get; set; }
    }

    public class CourseHistory
    {
        public string Row { get; set; }
        public string Course { get; set; }
        public string CourseType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalTime { get; set; }
        public string LecturerName { get; set; }
        public string Result { get; set; }
    }
}
