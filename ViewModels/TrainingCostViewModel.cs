using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoTraining.ViewModels
{
    public class TrainingCostViewModel
    {
        public string Row { get; set; }
        public int? TrianingId { get; set; }
        public string TrainingName { get; set; }
        public string TrainingDate { get; set; }
        public int? People { get; set; }
        public string PeopleString => this.People.HasValue ? $"{this.People} คน" : "";
        public double? Cost { get; set; }
        public string CostString => this.Cost.HasValue ? this.Cost.Value.ToString("0.00") + " บาท" : "-";
        public double? CostPerMan => this.Cost.HasValue && this.People.HasValue ? (this.Cost / this.People) : 0;
        public string CostPerManString => this.CostPerMan.HasValue ? this.CostPerMan.Value.ToString("0.00")+" บาท" : "-";
        public string Remark { get; set; }
    }
}
