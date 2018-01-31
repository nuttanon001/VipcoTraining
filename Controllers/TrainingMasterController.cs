using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReportClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VipcoTraining.Classes;
using VipcoTraining.Models;
using VipcoTraining.Services.Interfaces;
using VipcoTraining.ViewModels;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/TrainingMaster")]
    public class TrainingMasterController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingMaster> repository;
        private readonly IRepository<TblTrainingDetail> repositoryDetail;
        private readonly IRepository<TblTrainingCousre> repositoryCousre;
        private readonly IRepository<TblTrainingMasterHasPlace> repositoryHasPlace;
        private readonly IReportRepository repositoryReport;
        private readonly IMapper mapper;
        private readonly IHostingEnvironment appEnvironment;

        private JsonSerializerSettings DefaultJsonSettings =>
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        private List<MapType> ConverterTableToViewModel<MapType, TableType>(ICollection<TableType> tables)
        {
            var listData = new List<MapType>();
            foreach (var item in tables)
                listData.Add(this.mapper.Map<TableType, MapType>(item));
            return listData;
        }

        #endregion PrivateMenbers

        #region Constructor

        public TrainingMasterController(IRepository<TblTrainingMaster> repo,
            IRepository<TblTrainingDetail> repo2nd,
            IRepository<TblTrainingCousre> repo3rd,
            IRepository<TblTrainingMasterHasPlace> repo5th,
            IReportRepository repo4th,
            IMapper map,
            IHostingEnvironment appEnv)
        {
            this.repository = repo;
            this.repositoryDetail = repo2nd;
            this.repositoryCousre = repo3rd;
            this.repositoryReport = repo4th;
            this.repositoryHasPlace = repo5th;
            this.mapper = map;
            this.appEnvironment = appEnv;
        }

        #endregion Constructor

        // GET: api/TrainingMaster
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingMaster/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //relates
            //Expression<Func<TblTrainingMaster, object>> detailRelate = d => d.TblTrainingDetail;
            //var relates = new List<Expression<Func<TblTrainingMaster, object>>> { detailRelate };
            // filter
            //Expression<Func<TblTrainingMaster, bool>> condition = m => m.TrainingMasterId == id;

            //var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            //return new JsonResult(hasData, this.DefaultJsonSettings);

            var HasData = await this.repository.GetAllAsQueryable()
                                        .Include(x => x.TblTrainingMasterHasPlace)
                                        .ThenInclude(x => x.Place)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.TrainingMasterId == id);

            return new JsonResult(this.mapper.Map<TblTrainingMaster, TrainingMasterViewModel>(HasData), this.DefaultJsonSettings);
        }

        // POST: api/TrainingCourse/GetWithLazyLoad
        [HttpPost("FindAllLayzLoad")]
        public async Task<IActionResult> GetWithLazyLoadV2([FromBody]LazyLoadViewModel LazyLoad)
        {
            var Query = this.repository.GetAllAsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                                : LazyLoad.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                Query = Query.Where(x => x.Detail.ToLower().Contains(keyword) ||
                                         x.TrainingCode.ToLower().Contains(keyword) ||
                                         x.Remark.ToLower().Contains(keyword) ||
                                         x.TrainingName.ToLower().Contains(keyword) ||
                                         x.LecturerName.ToLower().Contains(keyword));
            }

            // Order
            switch (LazyLoad.SortField)
            {
                case "TrainingCode":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.TrainingCode);
                    else
                        Query = Query.OrderBy(e => e.TrainingCode);
                    break;

                case "TrainingName":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.TrainingName);
                    else
                        Query = Query.OrderBy(e => e.TrainingName);
                    break;

                case "TrainingDateString":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.TrainingDate);
                    else
                        Query = Query.OrderBy(e => e.TrainingDate);
                    break;

                case "LecturerName":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.LecturerName);
                    else
                        Query = Query.OrderBy(e => e.LecturerName);
                    break;

                default:
                    Query = Query.OrderByDescending(e => e.TrainingDate);
                    break;
            }
            int count = Query.Count();
            // Skip and Take
            Query = Query.Skip(LazyLoad.First ?? 0).Take(LazyLoad.Rows ?? 25);

            //this.ConverterTableToViewModel<TrainingMasterViewModel, TblTrainingMaster>(await Query.AsNoTracking().ToListAsync());
            // Get Data
            return new JsonResult(new
            {
                Data = this.ConverterTableToViewModel
                <TrainingMasterViewModel, TblTrainingMaster>
                (await Query.AsNoTracking().ToListAsync()),//await Query.AsNoTracking().ToListAsync(),
                TotalRow = count
            }, this.DefaultJsonSettings);
        }

        // POST: api/TrainingMaster
        [HttpPost]
        public IActionResult Post([FromBody]TrainingMasterViewModel nTrainingMaster)
        {
            if (nTrainingMaster.TrainingDate != null)
            {
                var tempDate = nTrainingMaster.TrainingDate.Value;
                System.TimeSpan time = System.TimeSpan.Parse(nTrainingMaster.TrainingDateTime);
                nTrainingMaster.TrainingDate = new System.DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hours, time.Minutes, 0);
            }

            if (nTrainingMaster.TrainingDateEnd != null)
            {
                var tempDate = nTrainingMaster.TrainingDateEnd.Value;
                System.TimeSpan time = System.TimeSpan.Parse(nTrainingMaster.TrainingDateEndTime);
                nTrainingMaster.TrainingDateEnd = new System.DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hours, time.Minutes, 0);
            }

            nTrainingMaster.CreateDate = DateTime.Now;
            nTrainingMaster.Creator = nTrainingMaster.Creator ?? "Someone";

            var course = this.repositoryCousre.Get(nTrainingMaster.TrainingCousreId.Value);

            if (nTrainingMaster.TblTrainingDetail != null)
            {
                foreach (var detail in nTrainingMaster.TblTrainingDetail)
                {
                    detail.MinScore = course.MinimunScore;
                    detail.CreateDate = nTrainingMaster.CreateDate;
                    detail.Creator = nTrainingMaster.Creator;
                    if (detail.Score != null)
                        detail.StatusForTraining = (byte)(detail.Score >= detail.MinScore ? 1 : 2);
                }
            }

            if (nTrainingMaster.PlaceId.HasValue)
            {
                nTrainingMaster.TblTrainingMasterHasPlace.Add(new TblTrainingMasterHasPlace()
                {
                    CreateDate = nTrainingMaster.CreateDate,
                    Creator = nTrainingMaster.Creator ?? "Someone",
                    PlaceId = nTrainingMaster.PlaceId.Value,
                });
            }

            return new JsonResult(
                this.repository.AddAsync(this.mapper.Map<TrainingMasterViewModel, TblTrainingMaster>(nTrainingMaster)).Result,
                this.DefaultJsonSettings);
        }

        // PUT: api/TrainingMaster/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TrainingMasterViewModel uTrainingMaster)
        {
            if (uTrainingMaster.TrainingDate != null)
            {
                var tempDate = uTrainingMaster.TrainingDate.Value;
                System.TimeSpan time = System.TimeSpan.Parse(uTrainingMaster.TrainingDateTime);
                uTrainingMaster.TrainingDate = new System.DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hours, time.Minutes, 0);
            }

            if (uTrainingMaster.TrainingDateEnd != null)
            {
                var tempDate = uTrainingMaster.TrainingDateEnd.Value;
                System.TimeSpan time = System.TimeSpan.Parse(uTrainingMaster.TrainingDateEndTime);
                uTrainingMaster.TrainingDateEnd = new System.DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hours, time.Minutes, 0);
            }

            uTrainingMaster.ModifyDate = DateTime.Now;
            uTrainingMaster.Modifyer = uTrainingMaster.Modifyer ?? "Someone";

            var course = this.repositoryCousre.Get(uTrainingMaster.TrainingCousreId.Value);

            if (uTrainingMaster.TblTrainingDetail != null)
            {
                foreach (var detail in uTrainingMaster.TblTrainingDetail)
                {
                    detail.MinScore = course.MinimunScore;
                    if (detail.TrainingDetailId > 0)
                    {
                        detail.ModifyDate = uTrainingMaster.ModifyDate;
                        detail.Modifyer = uTrainingMaster.Modifyer;
                    }
                    else
                    {
                        detail.CreateDate = uTrainingMaster.ModifyDate;
                        detail.Creator = uTrainingMaster.Modifyer;
                    }
                    if (detail.Score != null)
                        detail.StatusForTraining = (byte)(detail.Score >= detail.MinScore ? 1 : 2);
                }
            }

            // Update Place
            if (uTrainingMaster.PlaceId.HasValue)
            {
                Expression<Func<TblTrainingMasterHasPlace, bool>> condition = e => e.TrainingMasterId == uTrainingMaster.TrainingMasterId;

                var HasPlace = this.repositoryHasPlace.Find(condition);
                if (HasPlace != null)
                {
                    HasPlace.ModifyDate = uTrainingMaster.ModifyDate;
                    HasPlace.Modifyer = uTrainingMaster.Modifyer;
                    HasPlace.PlaceId = uTrainingMaster.PlaceId;

                    this.repositoryHasPlace.Update(HasPlace, HasPlace.TrainingMasterHasPlaceId);
                }
                else
                {
                    uTrainingMaster.TblTrainingMasterHasPlace.Add(new TblTrainingMasterHasPlace()
                    {
                        CreateDate = uTrainingMaster.ModifyDate,
                        Creator = uTrainingMaster.Modifyer ?? "Someone",
                        PlaceId = uTrainingMaster.PlaceId,
                    });
                }
            }

            var hasData = this.repository.Update(
                this.mapper.Map<TrainingMasterViewModel, TblTrainingMaster>(uTrainingMaster), id);

            if (hasData != null)
            {
                // filter
                Expression<Func<TblTrainingDetail, bool>> condition = m => m.TrainingMasterId == id;
                var dbTrainingDetails = this.repositoryDetail.FindAll(condition);

                //Remove TrainingDetail if edit remove it
                foreach (var dbTrainingDetail in dbTrainingDetails)
                {
                    if (!uTrainingMaster.TblTrainingDetail.Any(x => x.EmployeeTraining == dbTrainingDetail.EmployeeTraining))
                        this.repositoryDetail.Delete(dbTrainingDetail.TrainingDetailId);
                }
                //Update TrainingDetail
                foreach (var detail in uTrainingMaster.TblTrainingDetail)
                {
                    if (detail.TrainingDetailId > 0)
                        this.repositoryDetail.Update(detail, detail.TrainingDetailId);
                    else
                    {
                        detail.TrainingMasterId = uTrainingMaster.TrainingMasterId;
                        this.repositoryDetail.Add(detail);
                    }
                }
            }

            return new JsonResult(hasData, this.DefaultJsonSettings);
        }

        // DELETE: api/TrainingMaster/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }

        #region Report

        // GET: api/TrainingMaster/GetReportByTrainingMaster/5
        [HttpGet("GetReportByTrainingMaster/{id}")]
        public IActionResult GetReport(int id)
        {
            try
            {
                var worker = new Worker()
                {
                    TemplateFolder = this.appEnvironment.WebRootPath + "\\reports\\",
                };

                var hasData = this.repositoryReport.GetReportByTrainingMaster(id);
                var creDataTable = new MyDataTable();
                var dataTable = creDataTable.CreateMyDataTable<TrainingMasterReportViewModel>(hasData.Item1);

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    { "TrainingCourse", hasData.Item2.TrainingCode },
                    { "TrainingCourseName", hasData.Item2.TrainingName }
                };

                var stream = worker.Export(dataTable, dic, "TrainingMasterReport");

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = "Training Master report not found " + ex.ToString() });
            }
        }

        // POST: test
        [HttpPost("GetReportExcelByTrainingCourseAll")]
        public IActionResult GetReportExcelByTrainingCourseAll([FromBody]TrainingFilterViewModel trainingFilter)
        {
            try
            {
                var worker = new Worker()
                {
                    TemplateFolder = this.appEnvironment.WebRootPath + "\\reports\\",
                };

                var hasData = this.repositoryReport
                    .GetReportByTrainingCourse(trainingFilter);

                var creDataTable = new MyDataTable();
                var dataTable = creDataTable.CreateMyDataTable<TrainingMasterReportViewModel>(hasData.Item1);

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    { "TrainingCourse", hasData.Item2.TrainingCousreCode },
                    { "TrainingCourseName", hasData.Item2.TrainingCousreName }
                };

                var stream = worker.Export(dataTable, dic, "TrainingMasterReport");

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = "Training Master report not found " + ex.ToString() });
            }
        }

        // POST: api/TrainingMaster/GetReportByTrainingProgram
        [HttpPost("GetReportByTrainingProgram")]
        public IActionResult GetReportByTrainingProgram([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
                return new JsonResult(this.repositoryReport.GetEmployeeFromTrainingProgram(TrainingFilter), this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Training program not found " });
        }

        // POST: api/TrainingMaster/GetReportByTrainingProgramForBasic
        [HttpPost("GetReportByTrainingProgramForBasic")]
        public IActionResult GetReportByTrainingProgramForBasic([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
                return new JsonResult(this.repositoryReport.GetEmployeeFromTrainingProgramBasicV2(TrainingFilter), this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Training program not found " });
        }

        // POST: api/TrainingMaster/GetReportByTrainingProgramForStandard
        [HttpPost("GetReportByTrainingProgramForStandard")]
        public IActionResult GetReportByTrainingProgramForStandard([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
                return new JsonResult(this.repositoryReport.GetEmployeeFromTrainingProgramStandardV2(TrainingFilter), this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Training program not found " });
        }

        // POST: api/TrainingMaster/GetReportByTrainingProgramForStandard
        [HttpPost("GetReportByTrainingProgramForSupplement")]
        public IActionResult GetReportByTrainingProgramForSupplement([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
                return new JsonResult(this.repositoryReport.GetEmployeeFromTrainingProgramSupplementV2(TrainingFilter), this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Training program not found " });
        }

        // POST: api/TrainingMaster/GetReportByTrainingCourse
        [HttpPost("GetReportByTrainingCourse")]
        public IActionResult GetReportByTrainingCourse([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
                return new JsonResult(this.repositoryReport.GetEmployeeFromTrainingCourse(TrainingFilter), this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Training course not found " });
        }

        // POST: api/TrainingMaster/GetReportByEmployeeCode
        [HttpPost("GetReportByEmployeeCode")]
        public IActionResult GetReportByEmployeeCode([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
                return new JsonResult(this.repositoryReport.GetTrainingMasterFromEmployee(TrainingFilter).Result, this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Employee history not found" });
        }

        // POST: api/TrainingMaster/GetReportByEmployeeCodeExcel
        [HttpGet("GetReportByEmployeeCodeExcel/{EmployeeCode}")]
        public IActionResult GetReportByEmployeeCodeExcel(string EmployeeCode)
        {
            string ErrorMessage = "";
            try
            {
                if (!string.IsNullOrEmpty(EmployeeCode))
                {
                    var HasData = this.repositoryReport.GetEmployeeHistoryTraining(EmployeeCode);
                    if (HasData != null)
                    {
                        var worker = new Worker()
                        {
                            TemplateFolder = this.appEnvironment.WebRootPath + "\\reports\\",
                        };

                        var creDataTable = new MyDataTable();
                        var dataTable = creDataTable.CreateMyDataTable<CourseHistory>(HasData.HistoryCourses);

                        Dictionary<string, string> dic = new Dictionary<string, string>()
                        {
                            { "EmpCode", HasData.EmpCode },
                            { "EmpName", HasData.EmpName },
                            { "Position", HasData.Position },
                            { "Section", HasData.Section },
                            { "StartDate", HasData.StartDate },
                        };

                        var stream = worker.Export(dataTable, dic, "TrainingEmployeeReport");

                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            return NotFound(new { Error = "Employee history not found" + ErrorMessage });
        }

        [HttpPost("GetReportByTrainingProgramWithTrainingFilter")]
        public IActionResult GetReportByTrainingProgramWithTrainingFilter([FromBody]TrainingFilterViewModel TrainingFilter)
        {
            string error = "";
            try
            {
                if (TrainingFilter != null)
                {
                    var hasData = this.repositoryReport.GetReportByTrainingProgramWithTrainingFilter(TrainingFilter);
                    //(TrainingFilter.TrainingId,
                    //string.IsNullOrEmpty(TrainingFilter.GroupCode) ? null : new List<string>() { TrainingFilter.GroupCode },
                    //string.IsNullOrEmpty(TrainingFilter.PositionCode) ? null : new List<string>() { TrainingFilter.PositionCode },
                    //TrainingFilter.LocateID);

                    string imagePath = this.appEnvironment.WebRootPath + "\\images\\logoVIPCO.png";
                    if (hasData.Item1.Any())
                    {
                        var stream = new ReportClasses.Report().CreateExcelDoc(hasData.Item1, imagePath, hasData.Item2);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }

            return NotFound(new { Error = "Training Program report not found " + error });
        }

        [HttpPost("GetTrainingCostFromHistory")]
        public IActionResult GetTrainingCostFromHistory([FromBody] TrainingFilterViewModel TrainingFilter)
        {
            if (TrainingFilter != null)
            {
                return new JsonResult(this.repositoryReport.GetTrainingCostFromHistory(TrainingFilter), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Training filter not found " });
        }

        [HttpPost("GetTrainingCostFromHistoryExcel")]
        public IActionResult GetTrainingCostFromHistoryExcel([FromBody] TrainingFilterViewModel TrainingFilter)
        {
            string ErrorMessage = "";
            try
            {
                if (TrainingFilter != null)
                {
                    if (TrainingFilter.AfterDate.HasValue && TrainingFilter.EndDate.HasValue)
                    {
                        var HasData = this.repositoryReport.GetTrainingCostFromHistory(TrainingFilter);
                        if (HasData != null)
                        {
                            var worker = new Worker()
                            {
                                TemplateFolder = this.appEnvironment.WebRootPath + "\\reports\\",
                            };

                            var creDataTable = new MyDataTable();
                            var dataTable = creDataTable.CreateMyDataTable<TrainingCostViewModel>(HasData);

                            var StartDate = HelperClass.ConverterDate(TrainingFilter.AfterDate.Value.AddHours(7));
                            var EndDate = HelperClass.ConverterDate(TrainingFilter.EndDate.Value.AddHours(7));

                            Dictionary<string, string> dic = new Dictionary<string, string>()
                            {
                                { "StartDate", "วันที่  " + StartDate + "   "},
                                { "EndDate", "   ถึงวันที่  " + EndDate },
                            };

                            var stream = worker.Export(dataTable, dic, "TrainingCostReport");

                            stream.Seek(0, SeekOrigin.Begin);
                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            return NotFound(new { Error = "Employee history not found" + ErrorMessage });
        }

        #endregion Report

        #region Do_not_use

        public IActionResult GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblTrainingMaster, bool>> condition = e =>
              filters.Any(x => (e.Detail + e.TrainingCode +
                      e.Remark + e.TrainingName + e.LecturerName).ToLower().Contains(x));
            // Order
            Expression<Func<TblTrainingMaster, string>> Order = null;
            Expression<Func<TblTrainingMaster, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "TrainingCode":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingCode;
                    else
                        Order = e => e.TrainingCode;
                    break;

                case "TrainingName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingName;
                    else
                        Order = e => e.TrainingName;
                    break;

                case "TrainingDate":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingDate.ToString();
                    else
                        Order = e => e.TrainingDate.ToString();
                    break;

                case "LecturerName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.LecturerName;
                    else
                        Order = e => e.LecturerName;
                    break;

                default:
                    OrderDesc = e => e.TrainingDate.ToString();
                    break;
            }

            return new JsonResult(new
            {
                Data = this.ConverterTableToViewModel<TrainingMasterViewModel, TblTrainingMaster>(this.repository.FindAllWithLazyLoadAsync(condition, null, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
                       Order, OrderDesc).Result),
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        #endregion Do_not_use
    }
}