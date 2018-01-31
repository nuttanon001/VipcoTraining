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
    [Route("api/TrainingProgramHasPosition")]
    public class TrainingProgramHasPositionController : Controller
    {

        #region PrivateMenbers

        private readonly IRepository<TblTrainingProgramHasPosition> repository;
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

        public TrainingProgramHasPositionController(IRepository<TblTrainingProgramHasPosition> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/TrainingProgramHasPosition
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingProgramHasPosition/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }


        // GET: api/TrainingProgramHasPosition/GetByProgramID/5
        [HttpGet("GetByProgramID/{id}")]
        public IActionResult GetByTrainingProgramID(int id)
        {
            // relates
            Expression<Func<TblTrainingProgramHasPosition, object>> position = p => p.PositionCodeNavigation;
            var relates = new List<Expression<Func<TblTrainingProgramHasPosition, object>>> { position };
            // filter
            Expression<Func<TblTrainingProgramHasPosition, bool>> condition = m => m.TrainingProgramId == id;

            var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            return new JsonResult(this.ConverterTableToViewModel<ProgramHasPositionViewModel, TblTrainingProgramHasPosition>(hasData), this.DefaultJsonSettings);
        }

        // POST: api/TrainingProgramHasPosition
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingProgramHasPosition nTrainingHasPosition)
        {
            return new JsonResult(this.repository.AddAsync(nTrainingHasPosition).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingProgramHasPosition/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingProgramHasPosition uTrainingHasPosition)
        {
            return new JsonResult(this.repository.UpdateAsync(uTrainingHasPosition, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
