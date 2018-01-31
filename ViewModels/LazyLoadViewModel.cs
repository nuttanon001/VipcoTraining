using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoTraining.ViewModels
{
    public class LazyLoadViewModel
    {
        public int? First { get; set; }
        public int? Rows { get; set; }
        public string SortField { get; set; }
        public int? SortOrder { get; set; }
        public string Filter { get; set; }
    }
}
