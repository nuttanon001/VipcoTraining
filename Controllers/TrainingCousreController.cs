using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VipcoTraining.Models;
using VipcoTraining.Services.Interfaces;
using VipcoTraining.ViewModels;
//
// using StandardLibrary;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/TrainingCousre")]
    public class TrainingCousreController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingCousre> repository;
        private readonly IRepository<TblAttachFile> repositoryAttach;
        private readonly IRepository<TblCourseHasAttach> repositoryCourseHasAttach;
        private readonly IRepository<TblEducation> repositoryEduction;
        private readonly IRepository<TblTrainingLevel> repositoryLevel;
        private readonly IRepository<TblTrainingType> repositoryType;
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

        public TrainingCousreController(
            IRepository<TblTrainingCousre> repo,
            IRepository<TblAttachFile> repo2nd,
            IRepository<TblCourseHasAttach> repo3rd,
            IRepository<TblEducation> repo4th,
            IRepository<TblTrainingLevel> repo5th,
            IRepository<TblTrainingType> repo6th,
            IHostingEnvironment appEnv,
            IMapper map)
        {
            this.repository = repo;
            this.repositoryAttach = repo2nd;
            this.repositoryCourseHasAttach = repo3rd;
            this.repositoryEduction = repo4th;
            this.repositoryLevel = repo5th;
            this.repositoryType = repo6th;
            this.appEnvironment = appEnv;
            this.mapper = map;
        }

        #endregion Constructor

        // GET: api/TrainingCousre
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }
        // GET: api/TrainingCousre/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var HasData = this.repository.GetAsync(id).Result;
            if (HasData.EducationRequirementId.HasValue)
                HasData.EducationRequirement = this.repositoryEduction.Get(HasData.EducationRequirementId ?? 0);
            if (HasData.TrainingTypeId.HasValue)
                HasData.TrainingType = this.repositoryType.Get(HasData.TrainingTypeId ?? 0);
            if (HasData.TrainingLevelId.HasValue)
                HasData.TrainingLevel = this.repositoryLevel.Get(HasData.TrainingLevelId ?? 0);
            //HasData.TrainingType
            //HasData.TrainingLevel
            return new JsonResult(this.mapper.Map<TblTrainingCousre, TrainingCourseViewModel>(HasData), this.DefaultJsonSettings);
        }
        // GET: api/TrainingCousre/GetAttach/5
        [HttpGet("GetAttach/{TrainingCourseId}")]
        public async Task<IActionResult> GetAttach(int TrainingCourseId)
        {
            var Query = this.repositoryCourseHasAttach.GetAllAsQueryable()
                            .Where(x => x.TrainingCousreId == TrainingCourseId)
                            .Include(x => x.Attact);

            return new JsonResult(await Query.Select(x => x.Attact).AsNoTracking().ToListAsync(), this.DefaultJsonSettings);
        }

        [HttpGet("GetTest")]
        public string GetTest()
        {
            //var Test = new Class1();
            //Test.TestReport();
            return "Complete";
        }

        // POST: api/TrainingCourse/FindAllLayzLoad
        [HttpPost("FindAllLayzLoad")]

        public async Task<IActionResult> GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            string ErrorMessage = "";

            try
            {
                var Query = this.repository.GetAllAsQueryable()
                           .Include(x => x.TrainingType)
                           .Include(x => x.TrainingLevel)
                           .Include(x => x.EducationRequirement)
                           .AsNoTracking()
                           .AsQueryable();

                // Filter
                var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                                    : LazyLoad.Filter.ToLower().Split(null);

                foreach (var keyword in filters)
                {
                    Query = Query.Where(x => x.Detail.ToLower().Contains(keyword) ||
                                             x.EducationRequirement.EducationName.ToLower().Contains(keyword) ||
                                             x.Remark.ToLower().Contains(keyword) ||
                                             x.TrainingCousreCode.ToLower().Contains(keyword) ||
                                             x.TrainingCousreName.ToLower().Contains(keyword) ||
                                             x.TrainingLevel.TrainingLevel.ToLower().Contains(keyword) ||
                                             x.TrainingType.TrainingTypeName.ToLower().Contains(keyword));
                }

                // Order
                switch (LazyLoad.SortField)
                {
                    case "TrainingCousreCode":
                        if (LazyLoad.SortOrder == -1)
                            Query = Query.OrderByDescending(e => e.TrainingCousreCode);
                        else
                            Query = Query.OrderBy(e => e.TrainingCousreCode);
                        break;

                    case "TrainingCousreName":
                        if (LazyLoad.SortOrder == -1)
                            Query = Query.OrderByDescending(e => e.TrainingCousreName);
                        else
                            Query = Query.OrderBy(e => e.TrainingCousreName);
                        break;

                    case "TrainingLevelString":
                        if (LazyLoad.SortOrder == -1)
                            Query = Query.OrderByDescending(e => e.TrainingLevel.TrainingLevel);
                        else
                            Query = Query.OrderBy(e => e.TrainingLevel.TrainingLevel);
                        break;

                    case "EducationRequirementString":
                        if (LazyLoad.SortOrder == -1)
                            Query = Query.OrderByDescending(e => e.EducationRequirement.EducationName);
                        else
                            Query = Query.OrderBy(e => e.EducationRequirement.EducationName);
                        break;

                    case "TrainingTypeString":
                        if (LazyLoad.SortOrder == -1)
                            Query = Query.OrderByDescending(e => e.TrainingType.TrainingTypeName);
                        else
                            Query = Query.OrderBy(e => e.TrainingType.TrainingTypeName);
                        break;

                    default:
                        Query = Query.OrderByDescending(e => e.TrainingCousreCode);
                        break;
                }
                int count = Query.Count();
                // Skip and Take
                Query = Query.Skip(LazyLoad.First ?? 0).Take(LazyLoad.Rows ?? 25);

                return new JsonResult(new
                {
                    Data = this.ConverterTableToViewModel<TrainingCourseViewModel, TblTrainingCousre>(await Query.AsNoTracking().ToListAsync()),
                    TotalRow = count
                }, this.DefaultJsonSettings);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            return new JsonResult(ErrorMessage, this.DefaultJsonSettings);
        }

        // POST: api/TrainingCousre
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingCousre nTrainingCousre)
        {
            nTrainingCousre.Creator = nTrainingCousre.Creator ?? "Someone";
            nTrainingCousre.CreateDate = DateTime.Now;

            return new JsonResult(this.repository.AddAsync(nTrainingCousre).Result, this.DefaultJsonSettings);
        }

        // POST: api/TraniningCousre/Attach/5
        [HttpPost("PostAttach/{TrainingCousreId}/{CreateBy}")]
        public async Task<IActionResult> PostAttac(int TrainingCousreId,string CreateBy, IEnumerable<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath1 = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                string FileName = Path.GetFileName(formFile.FileName).ToLower();
                // create file name for file
                string FileNameForRef = $"{DateTime.Now.ToString("ddMMyyhhmmssfff")}{ Path.GetExtension(FileName).ToLower()}";
                // full path to file in temp location
                var filePath = Path.Combine(this.appEnvironment.WebRootPath + "\\files", FileNameForRef);

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await formFile.CopyToAsync(stream);
                }

                var returnData = this.repositoryAttach.Add(new TblAttachFile()
                {
                    AttachAddress = $"/files/{FileNameForRef}",
                    AttachFileName = FileName,
                    CreateDate = DateTime.Now,
                    Creator = CreateBy ?? "Someone"
                });

                this.repositoryCourseHasAttach.Add(new TblCourseHasAttach()
                {
                    AttactId = returnData.AttactId,
                    CreateDate = DateTime.Now,
                    Creator = CreateBy ?? "Someone",
                    TrainingCousreId = TrainingCousreId
                });
            }

            return Ok(new { count = 1, size, filePath1 });
        }

        // PUT: api/TrainingCousre/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingCousre uTrainingCousre)
        {
            uTrainingCousre.Modifyer = uTrainingCousre.Modifyer ?? "Someone";
            uTrainingCousre.ModifyDate = DateTime.Now;

            return new JsonResult(this.repository.UpdateAsync(uTrainingCousre, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/TrainingCousre/DeleteAttach/5
        [HttpDelete("DeleteAttach/{id}")]
        public void DeleteAttach(int id)
        {
            if (id > 0)
            {
                var AttachFile = this.repositoryAttach.Get(id);
                if (AttachFile != null)
                {
                    var filePath = Path.Combine(this.appEnvironment.WebRootPath + AttachFile.AttachAddress);
                    FileInfo delFile = new FileInfo(filePath);

                    if (delFile.Exists)
                        delFile.Delete();

                    Expression<Func<TblCourseHasAttach, bool>> condition = c => c.AttactId == AttachFile.AttactId;
                    var CourseHasAttach = this.repositoryCourseHasAttach.FindAsync(condition).Result;
                    if (CourseHasAttach != null)
                        this.repositoryCourseHasAttach.Delete(CourseHasAttach.CourseHasAttachId);
                    // remove attach
                    this.repositoryAttach.Delete(AttachFile.AttactId);
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }

        #region Do_not_use_this

        public IActionResult GetWithLazyLoad2([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate
            Expression<Func<TblTrainingCousre, object>> typeRelate = t => t.TrainingType;
            Expression<Func<TblTrainingCousre, object>> levelRelate = l => l.TrainingLevel;
            Expression<Func<TblTrainingCousre, object>> eduRelate = e => e.EducationRequirement;
            var relates = new List<Expression<Func<TblTrainingCousre, object>>>
                { typeRelate, levelRelate, eduRelate };
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblTrainingCousre, bool>> condition = e =>
              filters.Any(x => (e.Detail + e.EducationRequirement.EducationName +
                      e.Remark + e.TrainingCousreCode + e.TrainingCousreName +
                      e.TrainingLevel.TrainingLevel + e.TrainingType.TrainingTypeName).ToLower().Contains(x));
            // Order
            Expression<Func<TblTrainingCousre, string>> Order = null;
            Expression<Func<TblTrainingCousre, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "TrainingCousreCode":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingCousreCode;
                    else
                        Order = e => e.TrainingCousreCode;
                    break;

                case "TrainingCousreName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingCousreName;
                    else
                        Order = e => e.TrainingCousreName;
                    break;

                case "TrainingLevelString":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingLevel.TrainingLevel;
                    else
                        Order = e => e.TrainingLevel.TrainingLevel;
                    break;

                case "EducationRequirementString":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.EducationRequirement.EducationName;
                    else
                        Order = e => e.EducationRequirement.EducationName;
                    break;

                case "TrainingTypeString":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingType.TrainingTypeName;
                    else
                        Order = e => e.TrainingType.TrainingTypeName;
                    break;

                default:
                    Order = e => e.TrainingCousreCode;
                    break;
            }

            return new JsonResult(new
            {
                Data = this.ConverterTableToViewModel<TrainingCourseViewModel, TblTrainingCousre>(
                       this.repository.FindAllWithLazyLoadAsync(condition, relates, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
                       Order, OrderDesc).Result),
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        #endregion
    }
}