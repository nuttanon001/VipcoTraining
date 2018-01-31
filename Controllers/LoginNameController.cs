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
    [Route("api/LoginName")]
    public class LoginNameController : Controller
    {

        #region PrivateMenbers

        private readonly IRepository<TblLoginName> repository;
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

        public LoginNameController(IRepository<TblLoginName> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/LoginName
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/LoginName/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/LoginName/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserViewModel login)
        {
            // filter
            Expression<Func<TblLoginName, bool>> condition = m => m.UserName == login.UserName &&
                                                                  m.Password.ToLower() == login.PassWord.ToLower();
            var hasData = this.repository.FindAsync(condition).Result;
            if (hasData != null)
                return new JsonResult(hasData, this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "user or password not match" });
        }

        // POST: api/LoginName
        [HttpPost]
        public IActionResult Post([FromBody]TblLoginName nLoginName)
        {
            return new JsonResult(this.repository.AddAsync(nLoginName).Result, this.DefaultJsonSettings);
        }

        // PUT: api/LoginName/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]TblLoginName uLoginName)
        {
            return new JsonResult(this.repository.UpdateAsync(uLoginName, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
