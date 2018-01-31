using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblPlace
    {
        public TblPlace()
        {
            TblTrainingMasterHasPlace = new HashSet<TblTrainingMasterHasPlace>();
        }

        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ICollection<TblTrainingMasterHasPlace> TblTrainingMasterHasPlace { get; set; }
    }
}
