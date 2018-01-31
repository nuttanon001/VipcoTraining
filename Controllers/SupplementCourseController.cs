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
    [Route("api/SupplementCourse")]
    public class SupplementCourseController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblSupplementCourse> repository;
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

        public SupplementCourseController(IRepository<TblSupplementCourse> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/SupplementCourse
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/SupplementCourse/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }
        // GET: api/SupplementCourse/GetByProgramID/5
        [HttpGet("GetByProgramID/{id}")]
        public IActionResult GetByTrainingProgramID(int id)
        {
            // relates
            Expression<Func<TblSupplementCourse, object>> course = c => c.TrainingCousre;
            var relates = new List<Expression<Func<TblSupplementCourse, object>>> { course };
            // filter
            Expression<Func<TblSupplementCourse, bool>> condition = m => m.TrainingProgramId == id;

            var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            return new JsonResult(this.ConverterTableToViewModel<SupplementCourseViewModel, TblSupplementCourse>(hasData), this.DefaultJsonSettings);
        }
        // POST: api/SupplementCourse
        [HttpPost]
        public IActionResult Post([FromBody]TblSupplementCourse nSupplementCourse)
        {
            return new JsonResult(this.repository.AddAsync(nSupplementCourse).Result, this.DefaultJsonSettings);
        }

        // PUT: api/SupplementCourse/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblSupplementCourse uSupplementCourse)
        {
            return new JsonResult(this.repository.UpdateAsync(uSupplementCourse, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
