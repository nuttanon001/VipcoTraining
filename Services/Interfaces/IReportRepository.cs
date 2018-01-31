using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoTraining.Models;
using VipcoTraining.ViewModels;

namespace VipcoTraining.Services.Interfaces
{
    public interface IReportRepository
    {
        Tuple<IEnumerable<TrainingMasterReportViewModel>, TblTrainingMaster> GetReportByTrainingMaster(int TrainingMasterId);
        Tuple<IEnumerable<TrainingMasterReportViewModel>, TblTrainingCousre> GetReportByTrainingCourse(TrainingFilterViewModel TrainingFilter);
        Tuple<IEnumerable<TrainingProgram2ReportViewModel>, string> GetReportByTrainingProgram(int TrainingProgramId);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgram(TrainingFilterViewModel parameter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramBasic(TrainingFilterViewModel parameter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramStandard(TrainingFilterViewModel parameter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramSupplement(TrainingFilterViewModel parameter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingCourse(TrainingFilterViewModel parameter);
        List<List<string>> GetReportByTrainingProgramWithGroup(int ProgramId);
        IDictionary<string, List<List<string>>> GetReportByTrainingProgramWithGroupV2(int ProgramId,List<string> groupLists = null, List<string> positionLists = null, string Location = null);
        Tuple<IDictionary<string, List<List<string>>>, IDictionary<string, List<string>>> GetReportByTrainingProgramWithTrainingFilter(TrainingFilterViewModel TrainingFilter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramBasicV2(TrainingFilterViewModel parameter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramStandardV2(TrainingFilterViewModel parameter);
        IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramSupplementV2(TrainingFilterViewModel parameter);
        Task<IEnumerable<TrainingProgramReportViewModel>> GetTrainingMasterFromEmployee(TrainingFilterViewModel parameter);
        EmpHistoryTrainingViewModel GetEmployeeHistoryTraining(string EmployeeCode);
        IEnumerable<TrainingCostViewModel> GetTrainingCostFromHistory(TrainingFilterViewModel trainingFilter);
    }
}
