using AutoMapper;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoTraining.Models;
using VipcoTraining.ViewModels;
using VipcoTraining.Services.Interfaces;
using System.Linq.Expressions;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/Position")]
    public class PositionController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblPosition> repository;
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

        public PositionController(IRepository<TblPosition> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/Position
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/Position/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // GET: api/Position/GetWithRelate
        [HttpGet("FindAll")]
        [HttpGet("FindAll/{filter}")]
        public IActionResult GetWithRelate(string filter = "")
        {

            if (!string.IsNullOrEmpty(filter))
            {
                var filters = filter.ToLower().Split(null);
                Expression<Func<TblPosition, bool>> isEmployee = e =>
                  filters.Any(x => (e.PositionName + e.PositionCode).ToLower().Contains(x));

                return new JsonResult(this.repository.FindAllAsync(isEmployee).Result, this.DefaultJsonSettings);
            }
            else
            {
                return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
            }
        }

        // POST: api/Position/GetWithLazyLoad
        [HttpPost("FindAllLayzLoad")]
        public IActionResult GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblPosition, bool>> condition = e =>
                  filters.Any(x => (e.PositionName + e.PositionCode).ToLower().Contains(x));
            // Order
            Expression<Func<TblPosition, string>> Order = null;
            Expression<Func<TblPosition, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "PositionCode":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.PositionCode;
                    else
                        Order = e => e.PositionCode;
                    break;
                case "PositionName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.PositionName;
                    else
                        Order = e => e.PositionName;
                    break;
                default:
                    Order = e => e.PositionName;
                    break;
            }

            return new JsonResult(new
            {
                Data = this.repository.FindAllWithLazyLoadAsync(condition, null, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
                       Order, OrderDesc).Result,
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        // POST: api/Position
        [HttpPost]
        public IActionResult Post([FromBody]TblPosition nPosition)
        {
            return new JsonResult(this.repository.AddAsync(nPosition).Result, this.DefaultJsonSettings);
        }

        // PUT: api/Position/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]TblPosition uPosition)
        {
            return new JsonResult(this.repository.UpdateAsync(uPosition, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
