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
using Microsoft.EntityFrameworkCore;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/GroupName")]
    public class GroupNameController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblGroupName> repository;
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

        public GroupNameController(IRepository<TblGroupName> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion
        // GET: api/GroupName
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/GroupName/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/GroupName/FindAllLayzLoad
        [HttpPost("FindAllLayzLoad")]
        public async Task<IActionResult> GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            var Query = this.repository.GetAllAsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                                : LazyLoad.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                Query = Query.Where(x => x.GroupCode.ToLower().Contains(keyword) ||
                                         x.GroupDesc.ToLower().Contains(keyword) ||
                                         x.GroupRemark.ToLower().Contains(keyword) ||
                                         x.GroupEdesc.ToLower().Contains(keyword));
            }

            // Order
            switch (LazyLoad.SortField)
            {
                case "GroupCode":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.GroupCode);
                    else
                        Query = Query.OrderBy(e => e.GroupCode);
                    break;
                case "GroupDesc":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.GroupDesc);
                    else
                        Query = Query.OrderBy(e => e.GroupDesc);
                    break;
                default:
                    Query = Query.OrderBy(e => e.GroupCode);
                    break;
            }
            int count = Query.Count();
            // Skip and Take
            Query = Query.Skip(LazyLoad.First ?? 0).Take(LazyLoad.Rows ?? 25);
            // Get Data
            return new JsonResult(new
            {
                Data = await Query.AsNoTracking().ToListAsync(),
                TotalRow = count
            }, this.DefaultJsonSettings);
        }

        // POST: api/GroupName
        [HttpPost]
        public IActionResult Post([FromBody]TblGroupName nGroupName)
        {
            return new JsonResult(this.repository.AddAsync(nGroupName).Result, this.DefaultJsonSettings);
        }

        // PUT: api/GroupName/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]TblGroupName uGroupName)
        {
            return new JsonResult(this.repository.UpdateAsync(uGroupName, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
