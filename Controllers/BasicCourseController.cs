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
    [Route("api/BasicCourse")]
    public class BasicCourseController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblBasicCourse> repository;
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

        public BasicCourseController(IRepository<TblBasicCourse> repo,IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/BasicCourse
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/BasicCourse/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // GET: api/BasicCourse/GetByProgramID/5
        [HttpGet("GetByProgramID/{id}")]
        public IActionResult GetByTrainingProgramID(int id)
        {
            // relates
            Expression<Func<TblBasicCourse, object>> course = c => c.TrainingCousre;
            var relates = new List<Expression<Func<TblBasicCourse, object>>> { course };
            // filter
            Expression<Func<TblBasicCourse, bool>> condition = m => m.TrainingProgramId == id;

            var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            return new JsonResult(this.ConverterTableToViewModel<BasicCourseViewModel, TblBasicCourse>(hasData), this.DefaultJsonSettings);
        }

        // POST: api/BasicCourse
        [HttpPost]
        public IActionResult Post([FromBody]TblBasicCourse nBasicCourse)
        {
            return new JsonResult(this.repository.AddAsync(nBasicCourse).Result, this.DefaultJsonSettings);
        }

        // PUT: api/BasicCourse/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblBasicCourse uBasicCourse)
        {
            return new JsonResult(this.repository.UpdateAsync(uBasicCourse, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
