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
    [Route("api/TrainingProgramHasGroup")]
    public class TrainingProgramHasGroupController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingProgramHasGroup> repository;
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

        public TrainingProgramHasGroupController(IRepository<TblTrainingProgramHasGroup> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion
        // GET: api/TrainingProgramHasGroup
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingProgramHasGroup/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }


        // GET: api/TrainingProgramHasGroup/GetByProgramID/5
        [HttpGet("GetByProgramID/{id}")]
        public IActionResult GetByTrainingProgramID(int id)
        {
            // relates
            Expression<Func<TblTrainingProgramHasGroup, object>> group = g => g.GroupCodeNavigation;
            var relates = new List<Expression<Func<TblTrainingProgramHasGroup, object>>> { group };
            // filter
            Expression<Func<TblTrainingProgramHasGroup, bool>> condition = m => m.TrainingProgramId == id;

            var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            return new JsonResult(this.ConverterTableToViewModel<ProgramHasGroupViewModel, TblTrainingProgramHasGroup>(hasData), this.DefaultJsonSettings);
        }

        // POST: api/TrainingProgramHasGroup
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingProgramHasGroup nTrainingHasGroup)
        {
            return new JsonResult(this.repository.AddAsync(nTrainingHasGroup).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingProgramHasGroup/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingProgramHasGroup uTrainingHasGroup)
        {
            return new JsonResult(this.repository.UpdateAsync(uTrainingHasGroup, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/TrainingProgramHasGroup/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
