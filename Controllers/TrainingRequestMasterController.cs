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
    [Route("api/TrainingRequestMaster")]
    public class TrainingRequestMasterController : Controller
    {

        #region PrivateMenbers

        private readonly IRepository<TblTrainingRequestMaster> repository;
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

        public TrainingRequestMasterController(IRepository<TblTrainingRequestMaster> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/TrainingRequestMaster
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/TrainingRequestMaster/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/TrainingRequestMaster
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingRequestMaster nTrainingRequestMaster)
        {
            return new JsonResult(this.repository.AddAsync(nTrainingRequestMaster).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingRequestMaster/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingRequestMaster uTrainingRequestMaster)
        {
            return new JsonResult(this.repository.UpdateAsync(uTrainingRequestMaster, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
