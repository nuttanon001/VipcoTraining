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
    [Route("api/TrainingDetail")]
    public class TrainingDetailController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingDetail> repository;
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

        public TrainingDetailController(IRepository<TblTrainingDetail> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/TrainingDetail
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingDetail/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }
        // GET: api/TrainingDetail/GetByMasterID/5
        [HttpGet("GetByMasterID/{id}")]
        public IActionResult GetByTrainingMasterID(int id)
        {
            // relates
            Expression<Func<TblTrainingDetail, object>> employeeRelate = e => e.EmployeeTrainingNavigation;
            var relates = new List<Expression<Func<TblTrainingDetail, object>>> { employeeRelate };
            // filter
            Expression<Func<TblTrainingDetail, bool>> condition = m => m.TrainingMasterId == id;

            var hasData = this.repository.FindAllWithIncludeAsync(condition, relates).Result;
            return new JsonResult(this.ConverterTableToViewModel<TrainingDetailViewModel,TblTrainingDetail>(hasData), this.DefaultJsonSettings);
        }

        // POST: api/TrainingDetail
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingDetail nTrainingDetail)
        {
            nTrainingDetail.CreateDate = DateTime.Now;
            nTrainingDetail.Creator = nTrainingDetail.Creator ?? "Someone";
            return new JsonResult(this.repository.AddAsync(nTrainingDetail).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingDetail/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingDetail uTrainingDetail)
        {
            uTrainingDetail.ModifyDate = DateTime.Now;
            uTrainingDetail.Modifyer = uTrainingDetail.Modifyer ?? "Someone";
            return new JsonResult(this.repository.UpdateAsync(uTrainingDetail, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
