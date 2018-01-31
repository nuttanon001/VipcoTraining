using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VipcoTraining.Models;
using VipcoTraining.Services.Interfaces;
using VipcoTraining.ViewModels;

namespace VipcoTraining.Services.Classes
{
    public class ReportRepository : IReportRepository
    {
        #region PrivateMembers

        private ApplicationContext Context;

        #endregion PrivateMembers

        #region Constructor

        public ReportRepository(ApplicationContext ctx)
        {
            this.Context = ctx;
        }

        #endregion Constructor

        public Tuple<IEnumerable<TrainingMasterReportViewModel>, TblTrainingMaster>
            GetReportByTrainingMaster(int TrainingMasterId)
        {
            try
            {
                //var ListDetail = this.Context.TblTrainingDetail.Where(x => x.TrainingMasterId == TrainingMasterId).AsQueryable();

                var TrainingMaster = this.Context.TblTrainingMaster
                    .Where(x => x.TrainingMasterId == TrainingMasterId)
                    .Include(x => x.TrainingCousre)
                        .ThenInclude(x => x.TrainingType)
                    .Include(x => x.TblTrainingDetail)
                        .ThenInclude(x => x.EmployeeTrainingNavigation)
                        .ThenInclude(x => x.GroupCodeNavigation)
                    .FirstOrDefault();

                if (TrainingMaster != null)
                {
                    var Time = TrainingMaster.TrainingDurationHour != null &&
                                TrainingMaster.TrainingDurationHour > 5 ?
                                (TrainingMaster.TrainingDurationHour / 6).Value.ToString("0.0").Replace('.', ':') :
                                $" 0:{TrainingMaster.TrainingDurationHour}";

                    var ListData = TrainingMaster.TblTrainingDetail
                        .AsEnumerable<TblTrainingDetail>().OrderBy(x => x.EmployeeTrainingNavigation.GroupCode)
                        .Select((x, i) => new TrainingMasterReportViewModel
                        {
                            Id = (i + 1).ToString(),
                            EmployeeCode = x.EmployeeTraining,
                            EmployeeName = x.EmployeeTrainingNavigation?.NameThai ?? "-",
                            Position = x.EmployeeTrainingNavigation.PositionCode != null ?
                                this.Context.TblPosition.SingleOrDefault(p => p.PositionCode == x.EmployeeTrainingNavigation.PositionCode).PositionName :
                                "-",
                            WorkGroup = x.EmployeeTrainingNavigation?.GroupCodeNavigation?.GroupDesc ?? "-",
                            TrainingType = x.TrainingMaster?.TrainingCousre?.TrainingType?.TrainingTypeName ?? "-",
                            TrainingDate = x?.TrainingMaster?.TrainingDate?.ToString("dd/MM/yy HH:mm"),
                            TrainingTime = Time,
                            LecturerName = x?.TrainingMaster.LecturerName ?? "-",
                            Score = x.StatusForTraining == null ? "ยังไม่ระบุ" : (x.StatusForTraining.Value == 1 ? "ผ่าน" : "ไม่ผ่าน") //x.Score.ToString() ?? "-",
                        });

                    var NewMaster = new TblTrainingMaster()
                    {
                        TrainingCode = TrainingMaster.TrainingCode,
                        TrainingName = TrainingMaster.TrainingName
                    };
                    return new Tuple<IEnumerable<TrainingMasterReportViewModel>, TblTrainingMaster>(ListData, NewMaster);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public Tuple<IEnumerable<TrainingMasterReportViewModel>, TblTrainingCousre>
            GetReportByTrainingCourse(TrainingFilterViewModel TrainingFilter)
        {
            try
            {
                IEnumerable<TrainingMasterReportViewModel> ListData = new List<TrainingMasterReportViewModel>();
                var TrainingCourse = this.Context.TblTrainingCousre
                                                 .Include(x => x.TrainingType)
                                                 .FirstOrDefault(x => x.TrainingCousreId == TrainingFilter.TrainingId);

                if (TrainingFilter.GetTypeProgram == 1)
                {
                    var QueryData = this.Context.TblTrainingDetail
                                            .Where(x => x.TrainingMaster.TrainingCousreId == TrainingFilter.TrainingId)
                                            .Include(x => x.TrainingMaster.TrainingCousre.TrainingType)
                                            .Include(x => x.EmployeeTrainingNavigation.GroupCodeNavigation).AsQueryable();

                    if (!string.IsNullOrEmpty(TrainingFilter.LocateID))
                        QueryData = QueryData.Where(x => x.EmployeeTrainingNavigation.LocateId == TrainingFilter.LocateID);
                    if (!string.IsNullOrEmpty(TrainingFilter.GroupCode))
                        QueryData = QueryData.Where(x => x.EmployeeTrainingNavigation.GroupCode == TrainingFilter.GroupCode);
                    if (TrainingFilter.AfterDate != null)
                        QueryData = QueryData.Where(x => x.TrainingMaster.TrainingDate.Value.Date >= TrainingFilter.AfterDate.Value.Date);

                    ListData = QueryData.AsEnumerable<TblTrainingDetail>()
                                        .OrderBy(x => x.EmployeeTrainingNavigation.GroupCode)
                                        .ThenBy(x => x.EmployeeTrainingNavigation.PositionCode)
                                        .Select((x, i) => new TrainingMasterReportViewModel
                                        {
                                            Id = (i + 1).ToString(),
                                            EmployeeCode = x.EmployeeTraining ?? "-",
                                            EmployeeName = x.EmployeeTrainingNavigation?.NameThai ?? "-",
                                            Position = x.EmployeeTrainingNavigation?.PositionCode != null ?
                                                this.Context.TblPosition.FirstOrDefault(p => p.PositionCode == x.EmployeeTrainingNavigation.PositionCode)?.PositionName ?? "-" :
                                                "-",
                                            WorkGroup = x.EmployeeTrainingNavigation?.GroupCodeNavigation?.GroupDesc ?? "-",
                                            TrainingType = x.TrainingMaster?.TrainingCousre?.TrainingType?.TrainingTypeName ?? "-",
                                            TrainingDate = x?.TrainingMaster?.TrainingDate?.ToString("dd/MM/yy HH:mm"),
                                            TrainingTime = x?.TrainingMaster?.TrainingDurationHour != null &&
                                                                x.TrainingMaster.TrainingDurationHour > 5 ?
                                                                (x.TrainingMaster.TrainingDurationHour / 6).Value.ToString("0.0").Replace('.', ':') :
                                                                $" 0:{x.TrainingMaster.TrainingDurationHour}",
                                            LecturerName = x?.TrainingMaster.LecturerName ?? "-",
                                            Score = x.StatusForTraining == null ? "ยังไม่ระบุ" : (x.StatusForTraining.Value == 1 ? "ผ่าน" : "ไม่ผ่าน")
                                        });
                }
                else
                {
                    var QueryData = this.Context.TblEmployee
                                                .Where(x => !x.TblTrainingDetail
                                                              .Any(z => z.TrainingMaster.TrainingCousreId == TrainingFilter.TrainingId))
                                                .Include(x => x.GroupCodeNavigation)
                                                .Include(x => x.PositionCodeNavigation)
                                                .AsQueryable();

                    if (!string.IsNullOrEmpty(TrainingFilter.LocateID))
                        QueryData = QueryData.Where(x => x.LocateId == TrainingFilter.LocateID);
                    if (!string.IsNullOrEmpty(TrainingFilter.GroupCode))
                        QueryData = QueryData.Where(x => x.GroupCode == TrainingFilter.GroupCode);

                    ListData = QueryData.AsEnumerable<TblEmployee>()
                                        .OrderBy(x => x.GroupCodeNavigation.GroupCode)
                                        .ThenBy(x => x.PositionCode)
                                        .Select((x, i) => new TrainingMasterReportViewModel
                                        {
                                            Id = (i + 1).ToString(),
                                            EmployeeCode = x.EmpCode ?? "-",
                                            EmployeeName = x.NameThai ?? "-",
                                            Position = x.PositionCodeNavigation?.PositionName ?? "-",
                                            WorkGroup = x.GroupCodeNavigation?.GroupDesc ?? "-",
                                            TrainingType = TrainingCourse?.TrainingType?.TrainingTypeName ?? "-",
                                            TrainingDate = "-",
                                            TrainingTime = "-",
                                            LecturerName = "-",
                                            Score = "ยังไม่อบรม"
                                        });
                }

                    return new Tuple<IEnumerable<TrainingMasterReportViewModel>, TblTrainingCousre>(ListData, TrainingCourse);
                }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public Tuple<IEnumerable<TrainingProgram2ReportViewModel>, string> GetReportByTrainingProgram(int TrainingProgramId)
        {
            try
            {
                var TrainingProgram = this.Context.TblTrainingPrograms
                    .Where(x => x.TrainingProgramId == TrainingProgramId)
                    .Include(x => x.TblBasicCourse)
                        .ThenInclude(x => x.TrainingCousre)
                    .Include(x => x.TblStandardCourse)
                        .ThenInclude(x => x.TrainingCousre)
                    .Include(x => x.TblSupplementCourse)
                        .ThenInclude(x => x.TrainingCousre)
                    .Include(x => x.TblTrainingProgramHasPosition)
                        .ThenInclude(x => x.PositionCodeNavigation)
                    .FirstOrDefault();

                if (TrainingProgram != null)
                {
                    List<int> maxRow = new List<int> {
                        TrainingProgram.TblBasicCourse.Count,
                        TrainingProgram.TblStandardCourse.Count,
                        TrainingProgram.TblSupplementCourse.Count,
                        TrainingProgram.TblTrainingProgramHasPosition.Count
                    };
                    var totalRow = maxRow.Max(x => x);
                    var ListData = new List<TrainingProgram2ReportViewModel>();
                    var ListLevel = new List<string>();

                    if (!string.IsNullOrEmpty(TrainingProgram.TrainingProgramLevelString))
                    {
                        var temp = TrainingProgram.TrainingProgramLevelString.Trim();

                        if (temp.IndexOf(',') > -1)
                            ListLevel = temp.Split(',').OrderBy(x => x).ToList();
                        else if (temp.IndexOf(';') > -1)
                            ListLevel = temp.Split(';').OrderBy(x => x).ToList();
                        else if (temp.IndexOf(' ') > -1)
                            ListLevel = temp.Split(null).OrderBy(x => x).ToList();
                        else
                            ListLevel.Add(temp);
                    }

                    for (int i = 0; i < totalRow; i++)
                    {
                        var Basic = TrainingProgram.TblBasicCourse.Count > i
                            ? TrainingProgram.TblBasicCourse.ToList()[i] : null;
                        var Standard = TrainingProgram.TblStandardCourse.Count > i
                            ? TrainingProgram.TblStandardCourse.ToList()[i] : null;
                        var Supplement = TrainingProgram.TblSupplementCourse.Count > i
                            ? TrainingProgram.TblSupplementCourse.ToList()[i] : null;
                        var Position = TrainingProgram.TblTrainingProgramHasPosition.Count > i
                            ? TrainingProgram.TblTrainingProgramHasPosition.ToList()[i] : null;

                        ListData.Add(new TrainingProgram2ReportViewModel()
                        {
                            Row = i == 0 ? "1" : "",
                            Level = i < ListLevel.Count() ? ListLevel[i] : "",
                            Position = Position != null ? $"{Position.PositionCodeNavigation.PositionName}" : "",
                            Standard = Standard != null ? $"{i + 1}. {Standard.TrainingCousre.TrainingCousreName}" : "",
                            Basic = Basic != null ? $"{i + 1}. {Basic.TrainingCousre.TrainingCousreName}" : "",
                            Supplement = Supplement != null ? $"{i + 1}. {Supplement.TrainingCousre.TrainingCousreName}" : ""
                        });
                    }

                    return new Tuple<IEnumerable<TrainingProgram2ReportViewModel>, string>(ListData, TrainingProgram.Detail);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public List<List<string>> GetReportByTrainingProgramWithGroup(int ProgramId)
        {
            try
            {
                List<int> CourseIds = new List<int>();
                // BasicCourse
                CourseIds.AddRange(this.Context.TblBasicCourse
                                .Where(x => x.TrainingProgramId == ProgramId &&
                                            x.TrainingCousreId.HasValue)
                                .Select(x => x.TrainingCousreId.Value));
                // StandardCourse
                CourseIds.AddRange(this.Context.TblStandardCourse
                                        .Where(x => x.TrainingProgramId == ProgramId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // Supploup
                CourseIds.AddRange(this.Context.TblSupplementCourse
                                        .Where(x => x.TrainingProgramId == ProgramId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // CourseName
                var Courses = this.Context.TblTrainingCousre
                                        .Where(x => CourseIds.Contains(x.TrainingCousreId)).ToList();
                IDictionary<string, List<List<string>>> DicData = new Dictionary<string, List<List<string>>>();
                var DataExcel = new List<List<string>>();
                // GroupCode
                var GroupCodes = this.Context.TblTrainingProgramHasGroup
                                        .Where(x => x.TrainingProgramId == ProgramId)
                                        .Select(x => x.GroupCode).ToList();
                int index = 1;
                // Add Column
                var Column = new List<string>()
                {
                    "",
                    "",
                    "",
                    "",
                    ""
                };
                Column.AddRange(Courses.OrderBy(x => x.TrainingCousreCode).Select(x => x.TrainingCousreName));
                DataExcel.Add(Column);
                // Employees
                foreach (var employee in this.Context.TblEmployee
                                            .Where(x => GroupCodes.Contains(x.GroupCode))
                                            .OrderBy(x => x.LevelId).ToList())
                {
                    var Data = new List<string>
                    {
                        index.ToString("00"),
                        employee.EmpCode,
                        employee.NameThai,
                        this.Context.TblPosition
                        .SingleOrDefault(x => x.PositionCode == employee.PositionCode).PositionName,
                        employee.BeginDate?.ToString("dd/MM/yyyy") ?? "",
                    };

                    foreach (var Course in Courses.OrderBy(x => x.TrainingCousreCode))
                    {
                        var HasTraining = this.Context.TblTrainingDetail
                                                .Where(x => x.TrainingMaster.TrainingCousreId == Course.TrainingCousreId &&
                                                            x.EmployeeTraining == employee.EmpCode);
                        string Result = "";

                        if (HasTraining.Any())
                        {
                            Result = HasTraining.Any(x => x.StatusForTraining == 1) ? "ผ่าน" : "";
                        }
                        Data.Add(Result);
                    }
                    DataExcel.Add(Data);

                    // Count index
                    index++;
                }

                return DataExcel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public IDictionary<string, List<List<string>>>
            GetReportByTrainingProgramWithGroupV2(int ProgramId, List<string> groupLists = null, List<string> positionLists = null, string Location = null)
        {
            try
            {
                List<int> CourseIds = new List<int>();
                // BasicCourse
                CourseIds.AddRange(this.Context.TblBasicCourse
                                        .Where(x => x.TrainingProgramId == ProgramId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // StandardCourse
                CourseIds.AddRange(this.Context.TblStandardCourse
                                        .Where(x => x.TrainingProgramId == ProgramId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // Supplement
                CourseIds.AddRange(this.Context.TblSupplementCourse
                                        .Where(x => x.TrainingProgramId == ProgramId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // CourseName
                var Courses = this.Context.TblTrainingCousre
                                        .Where(x => CourseIds.Contains(x.TrainingCousreId)).ToList();
                // New Dictionary
                IDictionary<string, List<List<string>>> DicData = new Dictionary<string, List<List<string>>>();

                // GroupCode
                var Groups = this.Context.TblTrainingProgramHasGroup
                                        .Where(x => x.TrainingProgramId == ProgramId)
                                        .Select(x => x.GroupCodeNavigation);

                if (groupLists != null)
                    Groups = Groups.Where(x => groupLists.Contains(x.GroupCode));

                foreach (var group in Groups.ToList())
                {
                    var DataExcel = new List<List<string>>();
                    int index = 1;
                    // Add Column
                    var Column = new List<string>()
                    {
                        "",
                        "",
                        "",
                        "",
                        ""
                    };
                    Column.AddRange(Courses.OrderBy(x => x.TrainingCousreCode).Select(x => x.TrainingCousreName));
                    DataExcel.Add(Column);
                    var QueryData = this.Context.TblEmployee.AsQueryable();

                    if (positionLists != null)
                        QueryData = QueryData.Where(x => positionLists.Contains(x.PositionCode));

                    if (!string.IsNullOrEmpty(Location))
                        QueryData = QueryData.Where(x => x.LocateId == Location);

                    // Employees
                    foreach (var employee in QueryData.Where(x => x.GroupCode == group.GroupCode)
                                                      .OrderBy(x => x.LevelId).ToList())
                    {
                        var Data = new List<string>
                    {
                        index.ToString("00"),
                        employee.EmpCode,
                        employee.NameThai,
                        this.Context.TblPosition
                        .SingleOrDefault(x => x.PositionCode == employee.PositionCode).PositionName,
                        employee.BeginDate?.ToString("dd/MM/yyyy") ?? "",
                    };

                        foreach (var Course in Courses.OrderBy(x => x.TrainingCousreCode))
                        {
                            var HasTraining = this.Context.TblTrainingDetail
                                                    .Where(x => x.TrainingMaster.TrainingCousreId == Course.TrainingCousreId &&
                                                                x.EmployeeTraining == employee.EmpCode);
                            string Result = "";

                            if (HasTraining.Any())
                            {
                                Result = HasTraining.Any(x => x.StatusForTraining == 1) ? "ผ่าน" : "";
                            }
                            Data.Add(Result);
                        }
                        DataExcel.Add(Data);

                        // Count index
                        index++;
                    }
                    // Add data to dic
                    DicData.Add(group.GroupDesc, DataExcel);
                }

                return DicData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public Tuple<IDictionary<string, List<List<string>>>, IDictionary<string, List<string>>>
            GetReportByTrainingProgramWithTrainingFilter(TrainingFilterViewModel TrainingFilter)
        {
            try
            {
                List<int> CourseIds = new List<int>();
                // BasicCourse
                CourseIds.AddRange(this.Context.TblBasicCourse
                                        .Where(x => x.TrainingProgramId == TrainingFilter.TrainingId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // StandardCourse
                CourseIds.AddRange(this.Context.TblStandardCourse
                                        .Where(x => x.TrainingProgramId == TrainingFilter.TrainingId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // Supplement
                CourseIds.AddRange(this.Context.TblSupplementCourse
                                        .Where(x => x.TrainingProgramId == TrainingFilter.TrainingId &&
                                                    x.TrainingCousreId.HasValue)
                                        .Select(x => x.TrainingCousreId.Value));
                // CourseName
                var Courses = this.Context.TblTrainingCousre
                                        .Where(x => CourseIds.Contains(x.TrainingCousreId)).ToList();
                // New Dictionary
                IDictionary<string, List<List<string>>> DicData = new Dictionary<string, List<List<string>>>();
                IDictionary<string, List<string>> DicHeader = new Dictionary<string, List<string>>();

                var QueryData = this.Context.TblEmployee
                                            .Include(x => x.TblTrainingDetail)
                                                .ThenInclude(x => x.TrainingMaster)
                                            .Include(x => x.GroupCodeNavigation)
                                            .AsQueryable();

                if (!string.IsNullOrEmpty(TrainingFilter.PositionCode))
                    QueryData = QueryData.Where(x => x.PositionCode == TrainingFilter.PositionCode);

                if (!string.IsNullOrEmpty(TrainingFilter.LocateID))
                    QueryData = QueryData.Where(x => x.LocateId == TrainingFilter.LocateID);

                if (!string.IsNullOrEmpty(TrainingFilter.GroupCode))
                    QueryData = QueryData.Where(x => x.GroupCode == TrainingFilter.GroupCode);

                //else if (TrainingFilter.GetTypeProgram == 2)
                //    QueryData = QueryData.Where(x => !x.TblTrainingDetail.Any(z => z.StatusForTraining = 1));

                //var GroupList = QueryData.GroupBy(x => x.GroupCode)
                //    .Select(x => new
                //    {
                //        group = x.Key,
                //        employees = x.ToList()
                //    }).ToList();

                var GroupList = QueryData.GroupBy(x => x.GroupCodeNavigation)
                    .Select(x => x.Key).ToList();

                foreach (var group in GroupList)
                {
                    var DataExcel = new List<List<string>>();

                    int index = 1;
                    // Add Column
                    var Column = new List<string>()
                    {
                        "",
                        "",
                        "",
                        "",
                        ""
                    };
                    Column.AddRange(Courses.OrderBy(x => x.TrainingCousreCode).Select(x => x.TrainingCousreName));
                    DataExcel.Add(Column);

                    // Employees
                    var EmployeeData = QueryData.Where(x => x.GroupCode == group.GroupCode).OrderBy(x => x.LevelId);
                    var EmpTemp = this.Context.TblEmployee
                           .Include(x => x.PositionCodeNavigation)
                           .Include(x => x.SectionCodeNavigation)
                           .Include(x => x.DivisionCodeNavigation)
                           .Include(x => x.DeptCodeNavigation)
                           .Include(x => x.Locate)
                           .FirstOrDefault(x => x.EmpCode == EmployeeData.FirstOrDefault().EmpCode);

                    var DataHeader = new List<string>()
                    {
                        EmpTemp?.PositionCodeNavigation?.PositionName ?? "-",
                        group.GroupDesc  ?? "-",
                        EmpTemp?.SectionCodeNavigation?.SectionName  ?? "-",
                        EmpTemp?.DivisionCodeNavigation?.DivisionName  ?? "-",
                        EmpTemp?.DeptCodeNavigation?.DeptName  ?? "-",
                        EmpTemp?.Locate?.LocateDesc  ?? "-",
                    };

                    foreach (var employee in EmployeeData)
                    {
                        var Data = new List<string>
                        {
                            index.ToString("00"),
                            employee.EmpCode,
                            employee.NameThai,
                            this.Context.TblPosition
                                .SingleOrDefault(x => x.PositionCode == employee.PositionCode).PositionName,
                            employee.BeginDate?.ToString("dd/MM/yyyy") ?? "",
                        };

                        foreach (var Course in Courses.OrderBy(x => x.TrainingCousreCode))
                        {
                            var HasTraining = this.Context.TblTrainingDetail
                                                    .Where(x => x.TrainingMaster.TrainingCousreId == Course.TrainingCousreId &&
                                                                x.EmployeeTraining == employee.EmpCode);
                            string Result = "";

                            if (HasTraining.Any())
                            {
                                Result = HasTraining.Any(x => x.StatusForTraining == 1) ? "ผ่าน" : "";
                            }
                            Data.Add(Result);
                        }
                        DataExcel.Add(Data);

                        // Count index
                        index++;
                    }
                    // Add data to dic
                    DicData.Add(group.GroupDesc, DataExcel);
                    DicHeader.Add(group.GroupDesc, DataHeader);
                }

                return new Tuple<IDictionary<string, List<List<string>>>, IDictionary<string, List<string>>>(DicData, DicHeader);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        /// <summary>
        /// Get employee who pass , fail or not training upper condition
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgram(TrainingFilterViewModel parameter)
        {
            try
            {
                var Positions = this.Context.TblTrainingProgramHasPosition
                    .Where(x => x.TrainingProgramId == parameter.TrainingId)
                    .Select(x => x.PositionCode).ToList();

                var TotalCourse = new List<int?>();

                // Add BasicCourse
                var BasicCourse = this.Context.TblBasicCourse
                    .Where(x => x.TrainingProgramId == parameter.TrainingId)
                    .Select(x => x.TrainingCousreId).AsEnumerable();
                if (BasicCourse != null)
                    TotalCourse.AddRange(BasicCourse);
                // End *********\\

                // Add StandardCourse
                var StandardCourse = this.Context.TblStandardCourse
                    .Where(x => x.TrainingProgramId == parameter.TrainingId)
                    .Select(x => x.TrainingCousreId).AsEnumerable();
                if (StandardCourse != null)
                    TotalCourse.AddRange(StandardCourse);
                // End *********\\

                // Add SupplementCourse
                var SupplementCourse = this.Context.TblSupplementCourse
                    .Where(x => x.TrainingProgramId == parameter.TrainingId)
                    .Select(x => x.TrainingCousreId).AsEnumerable();
                if (SupplementCourse != null)
                    TotalCourse.AddRange(SupplementCourse);
                // End *********\\

                // 1 Get Employee Pass TrainingProgram
                if (parameter.GetTypeProgram == 1)
                {
                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => TotalCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        //TypeProgram = BasicCourse.Contains(x.Course.TrainingCousreId) ? "Basic" :
                        //                (StandardCourse.Contains(x.Course.TrainingCousreId) ? "Standard" :
                        //                    "Supplement")
                    }).AsEnumerable();
                }
                else
                {
                    List<TrainingProgramReportViewModel> HasData = new List<TrainingProgramReportViewModel>();
                    foreach (var Course in this.Context.TblTrainingCousre
                        .Where(x => TotalCourse.Contains(x.TrainingCousreId)))
                    {
                        var exclude = this.Context.TblTrainingDetail
                        .Where(x => x.TrainingMaster.TrainingCousreId == parameter.TrainingId &&
                                    x.StatusForTraining == 1)
                        .Select(x => x.EmployeeTraining).ToList();

                        //var QueryFail = this.Context.TblEmployee.Where(x => Positions.Contains(x.PositionCode) &&
                        //                                                x.TblTrainingDetail.Where(z => z.StatusForTraining != 1 &&
                        //                                                z.TrainingMaster.TrainingCousreId == Course.TrainingCousreId).Any());

                        var QueryFail = this.Context.TblEmployee.Where(x => Positions.Contains(x.PositionCode) && !exclude.Contains(x.EmpCode));

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            CourseId = Course.TrainingCousreId,
                            CourseCode = Course.TrainingCousreCode,
                            CourseName = Course.TrainingCousreName,
                            CourseDate = "-",
                            //TypeProgram = BasicCourse.Contains(Course.TrainingCousreId) ? "Basic" :
                            //                (StandardCourse.Contains(Course.TrainingCousreId) ? "Standard" :
                            //                    "Supplement")
                        }));
                    }

                    return HasData;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        #region TrainingProgramV1

        public IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramBasic(TrainingFilterViewModel parameter)
        {
            try
            {
                var Positions = this.Context.TblTrainingProgramHasPosition
                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                   .Select(x => x.PositionCode).ToList();

                if (parameter.GetTypeProgram == 1)
                {
                    var BasicCourse = this.Context.TblBasicCourse
                                           .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                           .Select(x => x.TrainingCousreId).ToList();

                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => BasicCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        TypeProgram = "Basic",
                        Pass = true
                    }).AsEnumerable();
                }
                else
                {
                    var HasData = new List<TrainingProgramReportViewModel>();

                    foreach (var BasicCourse in this.Context.TblBasicCourse
                                                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                                   .Select(x => x.TrainingCousre))
                    {
                        // !e.TblTrainingDetail.Any(x => BasicCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId)

                        var QueryFail = this.Context.TblEmployee
                                        .Where(e => Positions.Contains(e.PositionCode) &&
                                                    !e.TblTrainingDetail.Any(x => BasicCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId &&
                                                                x.StatusForTraining == 1))
                                        .Select(e => e);

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel()
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            CourseId = BasicCourse.TrainingCousreId,
                            CourseCode = BasicCourse.TrainingCousreCode,
                            CourseName = BasicCourse.TrainingCousreName,
                            CourseDate = "-",
                            TypeProgram = "Basic",
                            Pass = false
                        }).AsEnumerable());
                    }

                    return HasData.OrderBy(x => x.CourseCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramStandard(TrainingFilterViewModel parameter)
        {
            try
            {
                var Positions = this.Context.TblTrainingProgramHasPosition
                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                   .Select(x => x.PositionCode).ToList();

                if (parameter.GetTypeProgram == 1)
                {
                    var StandardCourse = this.Context.TblStandardCourse
                                           .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                           .Select(x => x.TrainingCousreId).ToList();

                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => StandardCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        TypeProgram = "Standard",
                        Pass = true
                    }).AsEnumerable();
                }
                else
                {
                    var HasData = new List<TrainingProgramReportViewModel>();

                    foreach (var StandardCourse in this.Context.TblStandardCourse
                                                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                                   .Select(x => x.TrainingCousre))
                    {
                        var QueryFail = this.Context.TblEmployee
                                        .Where(e => Positions.Contains(e.PositionCode) &&
                                                    !e.TblTrainingDetail
                                                    .Any(x => StandardCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId &&
                                                                x.StatusForTraining == 1))
                                        .Select(e => e);

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel()
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            CourseId = StandardCourse.TrainingCousreId,
                            CourseCode = StandardCourse.TrainingCousreCode,
                            CourseName = StandardCourse.TrainingCousreName,
                            CourseDate = "-",
                            TypeProgram = "Standard",
                            Pass = false
                        }).AsEnumerable());
                    }

                    return HasData.OrderBy(x => x.CourseCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public IEnumerable<TrainingProgramReportViewModel> GetEmployeeFromTrainingProgramSupplement(TrainingFilterViewModel parameter)
        {
            try
            {
                var Positions = this.Context.TblTrainingProgramHasPosition
                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                   .Select(x => x.PositionCode).ToList();

                if (parameter.GetTypeProgram == 1)
                {
                    var SupplementCourse = this.Context.TblSupplementCourse
                                           .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                           .Select(x => x.TrainingCousreId).ToList();

                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => SupplementCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        TypeProgram = "Supplement",
                        Pass = true
                    }).AsEnumerable();
                }
                else
                {
                    var HasData = new List<TrainingProgramReportViewModel>();

                    foreach (var SupplementCourse in this.Context.TblSupplementCourse
                                                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                                   .Select(x => x.TrainingCousre))
                    {
                        var QueryFail = this.Context.TblEmployee
                                        .Where(e => Positions.Contains(e.PositionCode) &&
                                                    !e.TblTrainingDetail
                                                    .Any(x => SupplementCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId &&
                                                                x.StatusForTraining == 1))
                                        .Select(e => e);

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel()
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            CourseId = SupplementCourse.TrainingCousreId,
                            CourseCode = SupplementCourse.TrainingCousreCode,
                            CourseName = SupplementCourse.TrainingCousreName,
                            CourseDate = "-",
                            TypeProgram = "Supplement",
                            Pass = false
                        }).AsEnumerable());
                    }

                    return HasData.OrderBy(x => x.CourseCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        #endregion TrainingProgramV1

        #region TrainingProgramV2

        public IEnumerable<TrainingProgramReportViewModel>
            GetEmployeeFromTrainingProgramBasicV2(TrainingFilterViewModel parameter)
        {
            try
            {
                //var Positions = this.Context.TblTrainingProgramHasPosition
                //   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                //   .Select(x => x.PositionCode).ToList();

                if (parameter.GetTypeProgram == 1)
                {
                    var BasicCourse = this.Context.TblBasicCourse
                                           .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                           .Select(x => x.TrainingCousreId).ToList();

                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => BasicCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    //Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (!string.IsNullOrEmpty(parameter.PositionCode))
                        QueryPass = QueryPass.Where(x => x.Employee.PositionCode == parameter.PositionCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        PositionName = x.Employee.PositionCodeNavigation.PositionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        TypeProgram = "Basic",
                        Pass = true
                    }).AsEnumerable();
                }
                else
                {
                    var HasData = new List<TrainingProgramReportViewModel>();

                    foreach (var BasicCourse in this.Context.TblBasicCourse
                                                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                                   .Select(x => x.TrainingCousre))
                    {
                        // !e.TblTrainingDetail.Any(x => BasicCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId)

                        var QueryFail = this.Context.TblEmployee
                                        .Where(e => //Positions.Contains(e.PositionCode) &&
                                                    !e.TblTrainingDetail.Any(x => BasicCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId &&
                                                                x.StatusForTraining == 1))
                                        .Select(e => e);

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);
                        if (!string.IsNullOrEmpty(parameter.PositionCode))
                            QueryFail = QueryFail.Where(x => x.PositionCode == parameter.PositionCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel()
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            PositionName = x.PositionCodeNavigation.PositionName,
                            CourseId = BasicCourse.TrainingCousreId,
                            CourseCode = BasicCourse.TrainingCousreCode,
                            CourseName = BasicCourse.TrainingCousreName,
                            CourseDate = "-",
                            TypeProgram = "Basic",
                            Pass = false
                        }).AsEnumerable());
                    }

                    return HasData.OrderBy(x => x.CourseCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public IEnumerable<TrainingProgramReportViewModel>
            GetEmployeeFromTrainingProgramStandardV2(TrainingFilterViewModel parameter)
        {
            try
            {
                //var Positions = this.Context.TblTrainingProgramHasPosition
                //   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                //   .Select(x => x.PositionCode).ToList();

                if (parameter.GetTypeProgram == 1)
                {
                    var StandardCourse = this.Context.TblStandardCourse
                                           .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                           .Select(x => x.TrainingCousreId).ToList();

                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => StandardCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    //Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (!string.IsNullOrEmpty(parameter.PositionCode))
                        QueryPass = QueryPass.Where(x => x.Employee.PositionCode == parameter.PositionCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        PositionName = x.Employee.PositionCodeNavigation.PositionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        TypeProgram = "Standard",
                        Pass = true
                    }).AsEnumerable();
                }
                else
                {
                    var HasData = new List<TrainingProgramReportViewModel>();

                    foreach (var StandardCourse in this.Context.TblStandardCourse
                                                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                                   .Select(x => x.TrainingCousre))
                    {
                        var QueryFail = this.Context.TblEmployee
                                        .Where(e => //Positions.Contains(e.PositionCode) &&
                                                    !e.TblTrainingDetail
                                                    .Any(x => StandardCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId &&
                                                                x.StatusForTraining == 1))
                                        .Select(e => e);

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);
                        if (!string.IsNullOrEmpty(parameter.PositionCode))
                            QueryFail = QueryFail.Where(x => x.PositionCode == parameter.PositionCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel()
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            PositionName = x.PositionCodeNavigation.PositionName,
                            CourseId = StandardCourse.TrainingCousreId,
                            CourseCode = StandardCourse.TrainingCousreCode,
                            CourseName = StandardCourse.TrainingCousreName,
                            CourseDate = "-",
                            TypeProgram = "Standard",
                            Pass = false
                        }).AsEnumerable());
                    }

                    return HasData.OrderBy(x => x.CourseCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public IEnumerable<TrainingProgramReportViewModel>
            GetEmployeeFromTrainingProgramSupplementV2(TrainingFilterViewModel parameter)
        {
            try
            {
                //var Positions = this.Context.TblTrainingProgramHasPosition
                //   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                //   .Select(x => x.PositionCode).ToList();

                if (parameter.GetTypeProgram == 1)
                {
                    var SupplementCourse = this.Context.TblSupplementCourse
                                           .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                           .Select(x => x.TrainingCousreId).ToList();

                    var QueryPass = this.Context.TblTrainingDetail
                                        .Where(d => SupplementCourse.Contains(d.TrainingMaster.TrainingCousreId) &&
                                                    //Positions.Contains(d.EmployeeTrainingNavigation.PositionCode) &&
                                                    d.StatusForTraining == 1)
                                        .Select(x => new
                                        {
                                            Employee = x.EmployeeTrainingNavigation,
                                            Course = x.TrainingMaster.TrainingCousre,
                                            Date = x.TrainingMaster.TrainingDate,
                                        });

                    if (!string.IsNullOrEmpty(parameter.LocateID))
                        QueryPass = QueryPass.Where(x => x.Employee.LocateId == parameter.LocateID);
                    if (!string.IsNullOrEmpty(parameter.GroupCode))
                        QueryPass = QueryPass.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                    if (!string.IsNullOrEmpty(parameter.PositionCode))
                        QueryPass = QueryPass.Where(x => x.Employee.PositionCode == parameter.PositionCode);
                    if (parameter.AfterDate != null)
                        QueryPass = QueryPass.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                    return QueryPass.Select(x => new TrainingProgramReportViewModel()
                    {
                        EmpCode = x.Employee.EmpCode,
                        NameThai = x.Employee.NameThai,
                        GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                        SectionName = x.Employee.SectionCodeNavigation.SectionName,
                        PositionName = x.Employee.PositionCodeNavigation.PositionName,
                        CourseId = x.Course.TrainingCousreId,
                        CourseCode = x.Course.TrainingCousreCode,
                        CourseName = x.Course.TrainingCousreName,
                        CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                        TypeProgram = "Supplement",
                        Pass = true
                    }).AsEnumerable();
                }
                else
                {
                    var HasData = new List<TrainingProgramReportViewModel>();

                    foreach (var SupplementCourse in this.Context.TblSupplementCourse
                                                   .Where(x => x.TrainingProgramId == parameter.TrainingId)
                                                   .Select(x => x.TrainingCousre))
                    {
                        var QueryFail = this.Context.TblEmployee
                                        .Where(e => //Positions.Contains(e.PositionCode) &&
                                                    !e.TblTrainingDetail
                                                    .Any(x => SupplementCourse.TrainingCousreId == x.TrainingMaster.TrainingCousreId &&
                                                                x.StatusForTraining == 1))
                                        .Select(e => e);

                        if (!string.IsNullOrEmpty(parameter.LocateID))
                            QueryFail = QueryFail.Where(x => x.LocateId == parameter.LocateID);
                        if (!string.IsNullOrEmpty(parameter.GroupCode))
                            QueryFail = QueryFail.Where(x => x.GroupCode == parameter.GroupCode);
                        if (!string.IsNullOrEmpty(parameter.PositionCode))
                            QueryFail = QueryFail.Where(x => x.PositionCode == parameter.PositionCode);

                        HasData.AddRange(QueryFail.Select(x => new TrainingProgramReportViewModel()
                        {
                            EmpCode = x.EmpCode,
                            NameThai = x.NameThai,
                            GroupName = x.GroupCodeNavigation.GroupDesc,
                            SectionName = x.SectionCodeNavigation.SectionName,
                            PositionName = x.PositionCodeNavigation.PositionName,
                            CourseId = SupplementCourse.TrainingCousreId,
                            CourseCode = SupplementCourse.TrainingCousreCode,
                            CourseName = SupplementCourse.TrainingCousreName,
                            CourseDate = "-",
                            TypeProgram = "Supplement",
                            Pass = false
                        }).AsEnumerable());
                    }

                    return HasData.OrderBy(x => x.CourseCode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        #endregion TrainingProgramV2

        /// <summary>
        /// Get employee who pass or fail training course
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<TrainingProgramReportViewModel>
            GetEmployeeFromTrainingCourse(TrainingFilterViewModel parameter)
        {
            try
            {
                IQueryable<TemplateClass> QueryData;

                if (parameter.GetTypeProgram == 1)
                {
                    QueryData = this.Context.TblTrainingDetail
                                  .Where(x => x.TrainingMaster.TrainingCousreId == parameter.TrainingId &&
                                              x.StatusForTraining == parameter.GetTypeProgram)
                                  .Select(x => new TemplateClass()
                                  {
                                      Employee = x.EmployeeTrainingNavigation,
                                      Course = x.TrainingMaster.TrainingCousre,
                                      Date = x.TrainingMaster.TrainingDate
                                  });
                }
                else
                {
                    //var exclude = this.Context.TblTrainingDetail
                    //                    .Where(x => x.TrainingMaster.TrainingCousreId == parameter.TrainingId &&
                    //                                x.StatusForTraining == 1)
                    //                    .Select(x => x.EmployeeTraining).ToList();

                    //QueryData = this.Context.TblTrainingDetail
                    //                 .Where(x => x.TrainingMaster.TrainingCousreId == parameter.TrainingId &&
                    //                             x.StatusForTraining == parameter.GetTypeProgram &&
                    //                             !exclude.Contains(x.EmployeeTraining))
                    //                 .Select(x => new TemplateClass()
                    //                 {
                    //                     Employee = x.EmployeeTrainingNavigation,
                    //                     Course = x.TrainingMaster.TrainingCousre,
                    //                     Date = x.TrainingMaster.TrainingDate
                    //                 });

                    var Course = this.Context.TblTrainingCousre.FirstOrDefault(x => x.TrainingCousreId == parameter.TrainingId);

                    QueryData = this.Context.TblEmployee
                                            .Where(x => !x.TblTrainingDetail
                                                         .Any(z => z.TrainingMaster.TrainingCousreId == parameter.TrainingId))
                                            .Select(x => new TemplateClass()
                                            {
                                                Employee = x,
                                                Course = Course,
                                                Date = null
                                            });
                }

                if (!string.IsNullOrEmpty(parameter.LocateID))
                    QueryData = QueryData.Where(x => x.Employee.LocateId == parameter.LocateID);
                if (!string.IsNullOrEmpty(parameter.GroupCode))
                    QueryData = QueryData.Where(x => x.Employee.GroupCode == parameter.GroupCode);
                if (parameter.AfterDate != null && parameter.GetTypeProgram == 1)
                    QueryData = QueryData.Where(x => x.Date.Value.Date >= parameter.AfterDate.Value.Date);

                return QueryData.Select(x => new TrainingProgramReportViewModel()
                {
                    EmpCode = x.Employee.EmpCode,
                    NameThai = $"{x.Employee.NameThai}",
                    GroupName = x.Employee.GroupCodeNavigation.GroupDesc,
                    SectionName = x.Employee.SectionCodeNavigation.SectionName,
                    CourseId = x.Course.TrainingCousreId,
                    CourseCode = x.Course.TrainingCousreCode,
                    CourseName = x.Course.TrainingCousreName,
                    CourseDate = x.Date != null ? x.Date.Value.ToString("dd/MM/yy") : "",
                    Pass = parameter.GetTypeProgram == 1
                }).AsEnumerable();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public async Task<IEnumerable<TrainingProgramReportViewModel>>
            GetTrainingMasterFromEmployee(TrainingFilterViewModel parameter)
        {
            try
            {
                var QueryData = this.Context.TblTrainingDetail.Where(x => x.EmployeeTraining == parameter.EmployeeCode);

                if (parameter.AfterDate != null)
                    QueryData = QueryData.Where(x => x.TrainingMaster.TrainingDate.Value.Date >= parameter.AfterDate.Value.Date);
                if (parameter.TrainingId > 0)
                    QueryData = QueryData.Where(x => x.TrainingMaster.TrainingCousreId == parameter.TrainingId);

                return await QueryData.Select(x => new TrainingProgramReportViewModel()
                {
                    EmpCode = x.EmployeeTraining,
                    NameThai = $"{x.EmployeeTrainingNavigation.NameThai}",
                    GroupName = x.EmployeeTrainingNavigation.GroupCodeNavigation.GroupDesc,
                    SectionName = x.EmployeeTrainingNavigation.SectionCodeNavigation.SectionName,
                    CourseId = x.TrainingMaster.TrainingCousreId ?? 0,
                    CourseCode = x.TrainingMaster.TrainingCousre.TrainingCousreCode,
                    CourseName = x.TrainingMaster.TrainingCousre.TrainingCousreName,
                    CourseDate = x.TrainingMaster.TrainingDate != null ? x.TrainingMaster.TrainingDate.Value.ToString("dd/MM/yy") : "",
                    Pass = x.StatusForTraining == null ? false : (x.StatusForTraining == 1 ? true : false)
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        public EmpHistoryTrainingViewModel GetEmployeeHistoryTraining(string EmployeeCode)
        {
            try
            {
                var QueryData = this.Context.TblTrainingDetail
                    .Where(x => x.EmployeeTraining == EmployeeCode)
                    .Include(x => x.TrainingMaster.TrainingCousre.TrainingType);

                //if (parameter.AfterDate != null)
                //    QueryData = QueryData.Where(x => x.TrainingMaster.TrainingDate.Value.Date >= parameter.AfterDate.Value.Date);
                //if (parameter.TrainingId > 0)
                //    QueryData = QueryData.Where(x => x.TrainingMaster.TrainingCousreId == parameter.TrainingId);

                var Employee = this.Context.TblEmployee
                    .Include(x => x.SectionCodeNavigation)
                    .FirstOrDefault(x => x.EmpCode == EmployeeCode);

                var EmpHistory = new EmpHistoryTrainingViewModel()
                {
                    EmpCode = Employee.EmpCode,
                    EmpName = Employee.NameThai,
                    Position = this.Context.TblPosition.SingleOrDefault(z => z.PositionCode == Employee.PositionCode).PositionName,
                    Section = Employee.SectionCodeNavigation?.SectionName ?? "-",
                    StartDate = Employee.BeginDate != null ? Employee.BeginDate.Value.ToString("dd/MM/yy") : "-",
                    HistoryCourses = new List<CourseHistory>()
                };

                int CountRow = 0;
                foreach (var detail in QueryData.OrderBy(x => x.TrainingMaster.TrainingDate))
                {
                    if (detail.TrainingMaster == null)
                        continue;

                    CountRow++;
                    var Time = detail.TrainingMaster.TrainingDurationHour != null &&
                               detail.TrainingMaster.TrainingDurationHour > 5 ?
                               Math.Round((detail.TrainingMaster.TrainingDurationHour / 6).Value, 1).ToString("0.0").Replace('.', ':') :
                               $"0:{detail.TrainingMaster.TrainingDurationHour}";

                    EmpHistory.HistoryCourses.Add(new CourseHistory()
                    {
                        Course = detail.TrainingMaster.TrainingCousre.TrainingCousreName,
                        CourseType = detail.TrainingMaster.TrainingCousre.TrainingType.TrainingTypeName,
                        EndDate = detail.TrainingMaster.TrainingDateEnd != null ?
                            detail.TrainingMaster.TrainingDateEnd.Value.ToString("dd/MM/yy") :
                            "-",
                        LecturerName = detail.TrainingMaster.LecturerName ?? "-",
                        Result = detail.StatusForTraining == null ? "ไม่ผ่าน" : (detail.StatusForTraining == 1 ? "ผ่าน" : "ไม่ผ่าน"),
                        Row = CountRow.ToString(),
                        StartDate = detail.TrainingMaster.TrainingDate != null ?
                            detail.TrainingMaster.TrainingDate.Value.ToString("dd/MM/yy") :
                            "-",
                        TotalTime = Time
                    });
                }

                return EmpHistory;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }

        // TrainingCost
        public IEnumerable<TrainingCostViewModel> GetTrainingCostFromHistory(TrainingFilterViewModel trainingFilter)
        {
            try
            {
                var QueryData = this.Context.TblTrainingMaster
                                            .Include(x => x.TblTrainingDetail)
                                            .Include(x => x.TrainingCousre)
                                            .AsQueryable();

                if (trainingFilter.TrainingId > 0)
                    QueryData = QueryData.Where(x => x.TrainingCousreId == trainingFilter.TrainingId);
                if (trainingFilter.AfterDate.HasValue)
                    QueryData = QueryData.Where(x => x.TrainingDate.Value.Date >= trainingFilter.AfterDate.Value.Date);
                if (trainingFilter.EndDate.HasValue)
                    QueryData = QueryData.Where(x => x.TrainingDate.Value.Date <= trainingFilter.EndDate.Value.Date);

                // User TrainingCourse
                //var QueryData = this.Context.TblTrainingCousre
                //                            .Include(x => x.TblTrainingMaster)
                //                                .ThenInclude(x => x.TblTrainingDetail)
                //                            .AsQueryable();

                //if (trainingFilter.TrainingId > 0)
                //    QueryData = QueryData.Where(x => x.TrainingCousreId == trainingFilter.TrainingId);
                //if (trainingFilter.AfterDate.HasValue)
                //    QueryData = QueryData.Where(x => x.TblTrainingMaster.Any(z => z.TrainingDate.Value.Date >= trainingFilter.AfterDate.Value.Date));
                //if (trainingFilter.EndDate.HasValue)
                //    QueryData = QueryData.Where(x => x.TblTrainingMaster.Any(z => z.TrainingDate.Value.Date <= trainingFilter.EndDate.Value.Date));

                return QueryData.AsEnumerable<TblTrainingMaster>().OrderBy(x => x.TrainingDate)
                    .Select((x, i) => new TrainingCostViewModel()
                    {
                        Row = (i + 1).ToString(),
                        TrianingId = x.TrainingCousreId ?? 0,
                        TrainingName = x.TrainingCousre.TrainingCousreName ?? "-",
                        TrainingDate = x.TrainingDate != null ? x.TrainingDate.Value.ToString("dd/MM/yy") : "-",
                        People = x.TblTrainingDetail.Any() ? x.TblTrainingDetail.Count : 0,
                        Cost = x.TrainingCost != null ? x.TrainingCost : x.TrainingCousre.BaseCost ?? 0,
                        Remark = "",
                    }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }
    }

    public class TemplateClass
    {
        public TblEmployee Employee { get; set; }
        public TblTrainingCousre Course { get; set; }
        public DateTime? Date { get; set; }
    }
}