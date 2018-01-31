using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoTraining.Models;
namespace VipcoTraining.ViewModels
{
    public class TrainingMasterViewModel:TblTrainingMaster
    {
        public string TrainingDateString { get; set; }
        public string TrainingDateTime { get; set; }
        public string TrainingDateEndString { get; set; }
        public string TrainingDateEndTime { get; set; }
        public int? PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
}
