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

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/TrainingRequestDetail")]
    public class TrainingRequestDetailController : Controller
    {

        #region PrivateMenbers

        private readonly IRepository<TblTrainingRequestDetail> repository;
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

        public TrainingRequestDetailController(IRepository<TblTrainingRequestDetail> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/TrainingRequestDetail
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingRequestDetail/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/TrainingRequestDetail
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingRequestDetail nTrainingRequestDetail)
        {
            return new JsonResult(this.repository.AddAsync(nTrainingRequestDetail).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingRequestDetail/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingRequestDetail uTrainingRequestDetail)
        {
            return new JsonResult(this.repository.UpdateAsync(uTrainingRequestDetail, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
