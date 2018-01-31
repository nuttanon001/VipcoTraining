using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblTrainingCousre
    {
        public TblTrainingCousre()
        {
            TblBasicCourse = new HashSet<TblBasicCourse>();
            TblCourseHasAttach = new HashSet<TblCourseHasAttach>();
            TblStandardCourse = new HashSet<TblStandardCourse>();
            TblSupplementCourse = new HashSet<TblSupplementCourse>();
            TblTrainingMaster = new HashSet<TblTrainingMaster>();
            TblTrainingPlanningDetail = new HashSet<TblTrainingPlanningDetail>();
            TblTrainingRequestMaster = new HashSet<TblTrainingRequestMaster>();
        }

        public int TrainingCousreId { get; set; }
        public string TrainingCousreCode { get; set; }
        public int? TrainingLevelId { get; set; }
        public int? TrainingTypeId { get; set; }
        public string TrainingCousreName { get; set; }
        public double? BaseCost { get; set; }
        public string Detail { get; set; }
        public int? EducationRequirementId { get; set; }
        public double? WorkExperienceRequirement { get; set; }
        public double? MinimunScore { get; set; }
        public string Remark { get; set; }
        public byte? Status { get; set; }
        public double? TrainingDurationHour { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        public TblEducation EducationRequirement { get; set; }
        public TblTrainingLevel TrainingLevel { get; set; }
        public TblTrainingType TrainingType { get; set; }
        public ICollection<TblBasicCourse> TblBasicCourse { get; set; }
        public ICollection<TblCourseHasAttach> TblCourseHasAttach { get; set; }
        public ICollection<TblStandardCourse> TblStandardCourse { get; set; }
        public ICollection<TblSupplementCourse> TblSupplementCourse { get; set; }
        public ICollection<TblTrainingMaster> TblTrainingMaster { get; set; }
        public ICollection<TblTrainingPlanningDetail> TblTrainingPlanningDetail { get; set; }
        public ICollection<TblTrainingRequestMaster> TblTrainingRequestMaster { get; set; }
    }
}
