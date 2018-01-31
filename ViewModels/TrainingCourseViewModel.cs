using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoTraining.Models;

namespace VipcoTraining.ViewModels
{
    public class TrainingCourseViewModel:TblTrainingCousre
    {
        public string EducationRequirementString { get; set; }
        public string TrainingLevelString { get; set; }
        public string TrainingTypeString { get; set; }
    }
}
