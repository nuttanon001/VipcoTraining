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
using VipcoTraining.Models;
using VipcoTraining.Services.Interfaces;
using VipcoTraining.ViewModels;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/TrainingPrograms")]
    public class TrainingProgramsController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingPrograms> repository;
        private readonly IRepository<TblBasicCourse> repositoryBasic;
        private readonly IRepository<TblStandardCourse> repositoryStandard;
        private readonly IRepository<TblSupplementCourse> repositorySupplement;
        private readonly IRepository<TblTrainingProgramHasPosition> repositoryHasPosition;
        private readonly IRepository<TblTrainingProgramHasGroup> repositoryHasGroup;
        private readonly IReportRepository repositoryReport;
        private readonly IHostingEnvironment appEnvironment;
        private readonly IMapper mapper;

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

        public TrainingProgramsController(IRepository<TblTrainingPrograms> repo,
            IRepository<TblBasicCourse> repo2,
            IRepository<TblStandardCourse> repo3,
            IRepository<TblSupplementCourse> repo4,
            IRepository<TblTrainingProgramHasPosition> repo5,
            IReportRepository repo6th,
            IRepository<TblTrainingProgramHasGroup> repo7th,
            IMapper map,
            IHostingEnvironment appEnv)
        {
            this.repository = repo;
            this.repositoryBasic = repo2;
            this.repositoryStandard = repo3;
            this.repositorySupplement = repo4;
            this.repositoryHasPosition = repo5;
            this.repositoryReport = repo6th;
            this.repositoryHasGroup = repo7th;
            this.appEnvironment = appEnv;
            this.mapper = map;
        }

        #endregion Constructor

        // GET: api/TrainingPrograms
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingPrograms/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/TrainingPrograms/FindAllLayzLoad
        [HttpPost("FindAllLayzLoad")]
        public async Task<IActionResult> GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            var Query = this.repository.GetAllAsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                                : LazyLoad.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                Query = Query.Where(x => x.Detail.ToLower().Contains(keyword) ||
                                         x.TrainingProgramCode.ToLower().Contains(keyword) ||
                                         x.Remark.ToLower().Contains(keyword) ||
                                         x.TrainingProgramName.ToLower().Contains(keyword));
            }

            // Order
            switch (LazyLoad.SortField)
            {
                case "TrainingProgramCode":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.TrainingProgramCode);
                    else
                        Query = Query.OrderBy(e => e.TrainingProgramCode);
                    break;

                case "TrainingProgramName":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.TrainingProgramName);
                    else
                        Query = Query.OrderBy(e => e.TrainingProgramName);
                    break;

                case "TrainingProgramLeave":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.TrainingProgramLeave.ToString());
                    else
                        Query = Query.OrderBy(e => e.TrainingProgramLeave.ToString());
                    break;

                default:
                    Query = Query.OrderBy(e => e.TrainingProgramCode);
                    break;
            }
            int count = Query.Count();
            // Skip and Take
            Query = Query.Skip(LazyLoad.First ?? 0).Take(LazyLoad.Rows ?? 25);
            // Get Data
            return new JsonResult(new
            {
                Data = await Query.AsNoTracking().ToListAsync(),
                TotalRow = count
            }, this.DefaultJsonSettings);
        }

        // POST: api/TrainingPrograms
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingPrograms nTrainingProgram)
        {
            nTrainingProgram.CreateDate = DateTime.Now;
            nTrainingProgram.Creator = nTrainingProgram.Creator ?? "Someone";

            if (nTrainingProgram.TblBasicCourse != null)
            {
                foreach (var basic in nTrainingProgram.TblBasicCourse)
                {
                    basic.CreateDate = nTrainingProgram.CreateDate;
                    basic.Creator = nTrainingProgram.Creator;
                }
            }

            if (nTrainingProgram.TblStandardCourse != null)
            {
                foreach (var standard in nTrainingProgram.TblStandardCourse)
                {
                    standard.CreateDate = nTrainingProgram.CreateDate;
                    standard.Creator = nTrainingProgram.Creator;
                }
            }

            if (nTrainingProgram.TblSupplementCourse != null)
            {
                foreach (var suppl in nTrainingProgram.TblSupplementCourse)
                {
                    suppl.CreateDate = nTrainingProgram.CreateDate;
                    suppl.Creator = nTrainingProgram.Creator;
                }
            }

            if (nTrainingProgram.TblTrainingProgramHasPosition != null)
            {
                foreach (var hasPo in nTrainingProgram.TblTrainingProgramHasPosition)
                {
                    hasPo.CreateDate = nTrainingProgram.CreateDate;
                    hasPo.Creator = nTrainingProgram.Creator;
                }
            }

            if (nTrainingProgram.TblTrainingProgramHasGroup != null)
            {
                foreach (var hasGp in nTrainingProgram.TblTrainingProgramHasGroup)
                {
                    hasGp.CreateDate = nTrainingProgram.CreateDate;
                    hasGp.Creator = nTrainingProgram.Creator;
                }
            }

            return new JsonResult(this.repository.AddAsync(nTrainingProgram).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingPrograms/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingPrograms uTrainingProgram)
        {
            uTrainingProgram.ModifyDate = DateTime.Now;
            uTrainingProgram.Modifyer = uTrainingProgram.Modifyer ?? "Someone";

            #region Update ModifyDate for child

            if (uTrainingProgram.TblBasicCourse != null)
            {
                foreach (var basic in uTrainingProgram.TblBasicCourse)
                {
                    if (basic.BasicCourseId > 0)
                    {
                        basic.ModifyDate = uTrainingProgram.ModifyDate;
                        basic.Modifyer = uTrainingProgram.Modifyer;
                    }
                    else
                    {
                        basic.CreateDate = uTrainingProgram.ModifyDate;
                        basic.Creator = uTrainingProgram.Modifyer;
                    }
                }
            }
            if (uTrainingProgram.TblStandardCourse != null)
            {
                foreach (var standard in uTrainingProgram.TblStandardCourse)
                {
                    if (standard.StandardCourseId > 0)
                    {
                        standard.ModifyDate = uTrainingProgram.ModifyDate;
                        standard.Modifyer = uTrainingProgram.Modifyer;
                    }
                    else
                    {
                        standard.CreateDate = uTrainingProgram.ModifyDate;
                        standard.Creator = uTrainingProgram.Modifyer;
                    }
                }
            }
            if (uTrainingProgram.TblSupplementCourse != null)
            {
                foreach (var supp in uTrainingProgram.TblSupplementCourse)
                {
                    if (supp.SupplermentCourseId > 0)
                    {
                        supp.ModifyDate = uTrainingProgram.ModifyDate;
                        supp.Modifyer = uTrainingProgram.Modifyer;
                    }
                    else
                    {
                        supp.CreateDate = uTrainingProgram.ModifyDate;
                        supp.Creator = uTrainingProgram.Modifyer;
                    }
                }
            }
            if (uTrainingProgram.TblTrainingProgramHasPosition != null)
            {
                foreach (var hasPo in uTrainingProgram.TblTrainingProgramHasPosition)
                {
                    if (hasPo.ProgramHasPositionId > 0)
                    {
                        hasPo.ModifyDate = uTrainingProgram.ModifyDate;
                        hasPo.Modifyer = uTrainingProgram.Modifyer;
                    }
                    else
                    {
                        hasPo.CreateDate = uTrainingProgram.ModifyDate;
                        hasPo.Creator = uTrainingProgram.Modifyer;
                    }
                }
            }
            if (uTrainingProgram.TblTrainingProgramHasGroup != null)
            {
                foreach (var hasGp in uTrainingProgram.TblTrainingProgramHasGroup)
                {
                    if (!string.IsNullOrEmpty(hasGp.GroupCode))
                    {
                        hasGp.ModifyDate = uTrainingProgram.ModifyDate;
                        hasGp.Modifyer = uTrainingProgram.Modifyer;
                    }
                    else
                    {
                        hasGp.CreateDate = uTrainingProgram.ModifyDate;
                        hasGp.Creator = uTrainingProgram.Modifyer;
                    }
                }
            }

            #endregion Update ModifyDate for child

            var hasData = this.repository.UpdateAsync(uTrainingProgram, id).Result;

            if (hasData != null)
            {
                #region Update and Remove for child

                #region TblBasicCourse

                // filter TblBasicCourse
                Expression<Func<TblBasicCourse, bool>> condition = m => m.TrainingProgramId == id;
                var items = this.repositoryBasic.FindAllAsync(condition).Result;
                //Remove TblBasicCourse if edit remove it
                foreach (var item in items)
                {
                    if (!uTrainingProgram.TblBasicCourse.Any(x => x.TrainingCousreId == item.TrainingCousreId))
                        this.repositoryBasic.Delete(item.BasicCourseId);
                }
                //Update TblBasicCourse
                foreach (var item in uTrainingProgram.TblBasicCourse)
                {
                    if (item.BasicCourseId > 0)
                        this.repositoryBasic.Update(item, item.BasicCourseId);
                    else
                    {
                        item.TrainingProgramId = id;
                        this.repositoryBasic.Add(item);
                    }
                }

                #endregion TblBasicCourse

                #region TblStandardCourse

                // filter TblStandardCourse
                Expression<Func<TblStandardCourse, bool>> condition2 = m => m.TrainingProgramId == id;
                var item2s = this.repositoryStandard.FindAllAsync(condition2).Result;
                //Remove TblStandardCourse if edit remove it
                foreach (var item in item2s)
                {
                    if (!uTrainingProgram.TblStandardCourse.Any(x => x.TrainingCousreId == item.TrainingCousreId))
                        this.repositoryStandard.Delete(item.StandardCourseId);
                }
                //Update TblStandardCourse
                foreach (var item in uTrainingProgram.TblStandardCourse)
                {
                    if (item.StandardCourseId > 0)
                        this.repositoryStandard.Update(item, item.StandardCourseId);
                    else
                    {
                        item.TrainingProgramId = id;
                        this.repositoryStandard.Add(item);
                    }
                }

                #endregion TblStandardCourse

                #region TblSupplementCourse

                // filter TblSupplementCourse
                Expression<Func<TblSupplementCourse, bool>> condition3 = m => m.TrainingProgramId == id;
                var item3s = this.repositorySupplement.FindAllAsync(condition3).Result;
                //Remove TblSupplementCourse if edit remove it
                foreach (var item in item3s)
                {
                    if (!uTrainingProgram.TblSupplementCourse.Any(x => x.TrainingCousreId == item.TrainingCousreId))
                        this.repositorySupplement.Delete(item.SupplermentCourseId);
                }
                //Update TblSupplementCourse
                foreach (var item in uTrainingProgram.TblSupplementCourse)
                {
                    if (item.SupplermentCourseId > 0)
                        this.repositorySupplement.Update(item, item.SupplermentCourseId);
                    else
                    {
                        item.TrainingProgramId = id;
                        this.repositorySupplement.Add(item);
                    }
                }

                #endregion TblSupplementCourse

                #region TblTrainingProgramHasPosition

                // filter TblTrainingProgramHasPosition
                Expression<Func<TblTrainingProgramHasPosition, bool>> condition4 = m => m.TrainingProgramId == id;
                var item4s = this.repositoryHasPosition.FindAllAsync(condition4).Result;
                //Remove TblTrainingProgramHasPosition if edit remove it
                foreach (var item in item4s)
                {
                    if (!uTrainingProgram.TblTrainingProgramHasPosition.Any(x => x.PositionCode == item.PositionCode))
                        this.repositoryHasPosition.Delete(item.ProgramHasPositionId);
                }
                //Update TblTrainingProgramHasPosition
                foreach (var item in uTrainingProgram.TblTrainingProgramHasPosition)
                {
                    if (item.ProgramHasPositionId > 0)
                        this.repositoryHasPosition.Update(item, item.ProgramHasPositionId);
                    else
                    {
                        item.TrainingProgramId = id;
                        this.repositoryHasPosition.Add(item);
                    }
                }

                #endregion TblTrainingProgramHasPosition

                #region TblTrainingProgramHasGroup

                // filter TblTrainingProgramHasGroup
                Expression<Func<TblTrainingProgramHasGroup, bool>> condition5 = m => m.TrainingProgramId == id;
                var item5s = this.repositoryHasGroup.FindAllAsync(condition5).Result;
                //Remove TblTrainingProgramHasGroup if edit remove it
                foreach (var item in item5s)
                {
                    if (!uTrainingProgram.TblTrainingProgramHasGroup.Any(x => x.GroupCode == item.GroupCode))
                        this.repositoryHasGroup.Delete(item.ProgramHasGroupId);
                }
                //Update TblTrainingProgramHasGroup
                foreach (var item in uTrainingProgram.TblTrainingProgramHasGroup)
                {
                    if (item.ProgramHasGroupId > 0)
                        this.repositoryHasGroup.Update(item, item.ProgramHasGroupId);
                    else
                    {
                        item.TrainingProgramId = id;
                        this.repositoryHasGroup.Add(item);
                    }
                }

                #endregion TblTrainingProgramHasGroup

                #endregion Update and Remove for child
            }
            return new JsonResult(hasData, this.DefaultJsonSettings);
        }

        // DELETE: api/TrainingPrograms/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }

        #region Report
        // GET: api/TrainingPrograms/GetReportByTrainingProgram/5
        [HttpGet("GetReportByTrainingProgramWithPosition/{id}")]
        public IActionResult GetReportByTrainingProgramWithPosition(int id)
        {
            try
            {
                var worker = new Worker()
                {
                    TemplateFolder = this.appEnvironment.WebRootPath + "\\reports\\",
                };

                var hasData = this.repositoryReport.GetReportByTrainingProgram(id);
                var creDataTable = new MyDataTable();
                var dataTable = creDataTable.CreateMyDataTable<TrainingProgram2ReportViewModel>(hasData.Item1);

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    { "Detail", hasData.Item2 },
                };

                var stream = worker.Export(dataTable, dic, "TrainingProgramReport");

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = "Training Program report not found " + ex.ToString() });
            }
        }

        [HttpGet("GetReportByTrainingProgramWithGroup/{id}")]
        public IActionResult GetReportByTrainingProgramWithGroup(int id)
        {
            string error = "";
            try
            {
                var hasData = this.repositoryReport.GetReportByTrainingProgramWithGroupV2(id);
                string imagePath = this.appEnvironment.WebRootPath + "\\images\\logoVIPCO.png";
                if (hasData.Any())
                {
                    var stream = new ReportClasses.Report().CreateExcelDoc(hasData, imagePath);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }

            return NotFound(new { Error = "Training Program report not found " + error });
        }

        [HttpGet("GetReportByTrainingProgramWithGroupAndPosition/{id}/{group}/{position}")]
        public IActionResult GetReportByTrainingProgramWithGroupAndPosition(int id, string group, string position)
        {
            string error = "";
            try
            {
                var hasData = this.repositoryReport.GetReportByTrainingProgramWithGroupV2(id, new List<string> { group }, new List<string> { position });
                string imagePath = this.appEnvironment.WebRootPath + "\\images\\logoVIPCO.png";
                if (hasData.Any())
                {
                    var stream = new ReportClasses.Report().CreateExcelDoc(hasData, imagePath);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }

            return NotFound(new { Error = "Training Program report not found " + error });
        }

        #endregion

        #region Do_not_use_this

        public IActionResult GetWithLazyLoad2([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblTrainingPrograms, bool>> condition = e =>
              filters.Any(x => (e.Detail + e.TrainingProgramCode +
                      e.Remark + e.TrainingProgramName).ToLower().Contains(x));
            // Order
            Expression<Func<TblTrainingPrograms, string>> Order = null;
            Expression<Func<TblTrainingPrograms, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "TrainingProgramCode":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingProgramCode;
                    else
                        Order = e => e.TrainingProgramCode;
                    break;

                case "TrainingProgramName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingProgramName;
                    else
                        Order = e => e.TrainingProgramName;
                    break;

                case "TrainingProgramLeave":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingProgramLeave.ToString();
                    else
                        Order = e => e.TrainingProgramLeave.ToString();
                    break;

                default:
                    OrderDesc = e => e.TrainingProgramCode.ToString();
                    break;
            }

            return new JsonResult(new
            {
                Data = this.repository.FindAllWithLazyLoadAsync(condition, null, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
                       Order, OrderDesc).Result,
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        #endregion Do_not_use_this
    }
}