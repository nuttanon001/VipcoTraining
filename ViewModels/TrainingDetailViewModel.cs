using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoTraining.Models;
namespace VipcoTraining.ViewModels
{
    public class TrainingDetailViewModel:TblTrainingDetail
    {
        public string EmployeeNameString { get; set; }
        public string StatusForTrainingString { get; set; }
    }
}
