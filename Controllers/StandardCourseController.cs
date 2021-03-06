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
    [Route("api/StandardCourse")]
    public class StandardCourseController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblStandardCourse> repository;
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

        public StandardCourseController(IRepository<TblStandardCourse> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/StandardCourse
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/StandardCourse/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }
        // GET: api/StandardCourse/GetByProgramID/5
        [HttpGet("GetByProgramID/{id}")]
        public IActionResult GetByTrainingProgramID(int id)
        {
            // relates
            Expression<Func<TblStandardCourse, object>> course = c => c.TrainingCousre;
            var relates = new List<Expression<Func<TblStandardCourse, object>>> { course };
            // filter
            Expression<Func<TblStandardCourse, bool>> condition = m => m.TrainingProgramId == id;

            var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            return new JsonResult(this.ConverterTableToViewModel<StandardCourseViewModel, TblStandardCourse>(hasData), this.DefaultJsonSettings);
        }
        // POST: api/StandardCourse
        [HttpPost]
        public IActionResult Post([FromBody]TblStandardCourse nStandardCourse)
        {
            return new JsonResult(this.repository.AddAsync(nStandardCourse).Result, this.DefaultJsonSettings);
        }

        // PUT: api/StandardCourse/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblStandardCourse uStandardCourse)
        {
            return new JsonResult(this.repository.UpdateAsync(uStandardCourse, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
