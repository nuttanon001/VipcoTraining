using AutoMapper;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using VipcoTraining.Models;
using VipcoTraining.ViewModels;
using VipcoTraining.Services.Interfaces;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/TrainingLevel")]
    public class TrainingLevelController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingLevel> repository;
        private readonly IMapper mapper;

        private JsonSerializerSettings DefaultJsonSettings =>
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        #endregion PrivateMenbers

        #region Constructor

        public TrainingLevelController(IRepository<TblTrainingLevel> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/TrainingLevel
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingLevel/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/Employee/GetWithLazyLoad
        [HttpPost("FindAllLayzLoad")]
        public IActionResult GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate

            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblTrainingLevel, bool>> condition = e =>
              filters.Any(x => (e.TrainingLevel + e.Detail).ToLower().Contains(x));
            // Order
            Expression<Func<TblTrainingLevel, string>> Order = null;
            Expression<Func<TblTrainingLevel, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "TrainingLevelId":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingLevelId.ToString("00");
                    else
                        Order = e => e.TrainingLevelId.ToString("00");
                    break;
                case "TrainingLevel":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingLevel;
                    else
                        Order = e => e.TrainingLevel;
                    break;
                case "Detail":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.Detail;
                    else
                        Order = e => e.Detail;
                    break;
                default:
                    Order = e => e.TrainingLevelId.ToString("00");
                    break;
            }

            return new JsonResult(new
            {
                Data = this.repository.FindAllWithLazyLoadAsync(condition, null, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25, Order, OrderDesc).Result,
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        // POST: api/TrainingLevel
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingLevel nTrainingLevel)
        {
            nTrainingLevel.Creator = nTrainingLevel.Creator ?? "Someone";
            nTrainingLevel.CreateDate = DateTime.Now;
            return new JsonResult(this.repository.AddAsync(nTrainingLevel).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingLevel/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingLevel uTrainingLevel)
        {
            uTrainingLevel.Modifyer = uTrainingLevel.Modifyer ?? "Someone";
            uTrainingLevel.ModifyDate = DateTime.Now;
            return new JsonResult(this.repository.UpdateAsync(uTrainingLevel, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
