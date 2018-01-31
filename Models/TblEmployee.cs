using System;
using System.Collections.Generic;

namespace VipcoTraining.Models
{
    public partial class TblEmployee
    {
        public TblEmployee()
        {
            TblTrainingDetail = new HashSet<TblTrainingDetail>();
            TblTrainingRequestDetail = new HashSet<TblTrainingRequestDetail>();
            TblTrainingRequestMaster = new HashSet<TblTrainingRequestMaster>();
        }

        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string EmpCard { get; set; }
        public string Title { get; set; }
        public string NameThai { get; set; }
        public string NameEng { get; set; }
        public string NickName { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? ResidentDate { get; set; }
        public string EmptypeCode { get; set; }
        public string DeptCode { get; set; }
        public string DivisionCode { get; set; }
        public string SectionCode { get; set; }
        public string GroupCode { get; set; }
        public string GroupMis { get; set; }
        public string PositionCode { get; set; }
        public string LevelCode { get; set; }
        public string AreaId { get; set; }
        public string LocateId { get; set; }
        public string EduLevel { get; set; }
        public string EduInstitute { get; set; }
        public string EduBranch { get; set; }
        public string EduCertificate { get; set; }
        public bool? EmpStatusOut { get; set; }
        public DateTime? OutDate { get; set; }
        public byte[] EmpPict { get; set; }
        public string Jdno { get; set; }
        public string GroupId { get; set; }
        public string LevelId { get; set; }
        public string WorkId { get; set; }

        public TblDepartment DeptCodeNavigation { get; set; }
        public TblDivision DivisionCodeNavigation { get; set; }
        public TblEmpType EmptypeCodeNavigation { get; set; }
        public TblGroupName GroupCodeNavigation { get; set; }
        public TblLocation Locate { get; set; }
        public TblPosition PositionCodeNavigation { get; set; }
        public TblSection SectionCodeNavigation { get; set; }
        public ICollection<TblTrainingDetail> TblTrainingDetail { get; set; }
        public ICollection<TblTrainingRequestDetail> TblTrainingRequestDetail { get; set; }
        public ICollection<TblTrainingRequestMaster> TblTrainingRequestMaster { get; set; }
    }
}
