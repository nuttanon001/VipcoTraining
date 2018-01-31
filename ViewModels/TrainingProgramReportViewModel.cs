using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoTraining.Models;

namespace VipcoTraining.ViewModels
{
    public class TrainingProgramReportViewModel
    {
        public string EmpCode { get; set; }
        public string NameThai { get; set; }
        public string GroupName { get; set; }
        public string SectionName { get; set; }
        public string PositionName { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDate { get; set; }
        public string TypeProgram { get; set; }
        public bool Pass { get; set; }
        public string PassString => this.Pass ? "ผ่าน" : "ไม่ผ่าน(ไม่ได้อบรม)";

    }
}
