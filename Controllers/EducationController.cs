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
    [Route("api/Education")]
    public class EducationController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblEducation> repository;
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

        public EducationController(IRepository<TblEducation> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/Education
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/Education/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }
        // POST: api/Education/GetWithLazyLoad
        [HttpPost("FindAllLayzLoad")]
        public IActionResult GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate

            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblEducation, bool>> condition = e =>
              filters.Any(x => (e.EducationName + e.Detail).ToLower().Contains(x));
            // Order
            Expression<Func<TblEducation, string>> Order = null;
            Expression<Func<TblEducation, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "EducationId":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.EducationId.ToString("00");
                    else
                        Order = e => e.EducationId.ToString("00");
                    break;
                case "EducationName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.EducationName;
                    else
                        Order = e => e.EducationName;
                    break;
                case "Detail":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.Detail;
                    else
                        Order = e => e.Detail;
                    break;
                default:
                    Order = e => e.EducationId.ToString("00");
                    break;
            }

            return new JsonResult(new
            {
                Data = this.repository.FindAllWithLazyLoadAsync(condition, null, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,Order, OrderDesc).Result,
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        // POST: api/Education
        [HttpPost]
        public IActionResult Post([FromBody]TblEducation nEducation)
        {
            nEducation.Creator = nEducation.Creator ?? "Someone";
            nEducation.CreateDate = DateTime.Now;
            return new JsonResult(this.repository.AddAsync(nEducation).Result, this.DefaultJsonSettings);
        }

        // PUT: api/Education/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblEducation uEducation)
        {
            uEducation.Modifyer = uEducation.Modifyer ?? "Someone";
            uEducation.ModifyDate = DateTime.Now;
            return new JsonResult(this.repository.UpdateAsync(uEducation, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
