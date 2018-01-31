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
    [Route("api/Division")]
    public class DivisionController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblDivision> repository;
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

        public DivisionController(IRepository<TblDivision> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/Division
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/Division/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/Division
        [HttpPost]
        public IActionResult Post([FromBody]TblDivision nDivision)
        {
            return new JsonResult(this.repository.AddAsync(nDivision).Result, this.DefaultJsonSettings);
        }

        // PUT: api/Division/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]TblDivision uDivision)
        {
            return new JsonResult(this.repository.UpdateAsync(uDivision, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
