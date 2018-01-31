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
using Microsoft.EntityFrameworkCore;

namespace VipcoTraining.Controllers
{
    [Produces("application/json")]
    [Route("api/Place")]
    public class PlaceController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblPlace> repository;
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

        public PlaceController(IRepository<TblPlace> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/Place
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/Place/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/Place/FindAllLayzLoad
        [HttpPost("FindAllLayzLoad")]
        public async Task<IActionResult> GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            var Query = this.repository.GetAllAsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                                : LazyLoad.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                Query = Query.Where(x => x.PlaceName.ToLower().Contains(keyword));
            }

            // Order
            switch (LazyLoad.SortField)
            {
                case "PlaceName":
                    if (LazyLoad.SortOrder == -1)
                        Query = Query.OrderByDescending(e => e.PlaceName);
                    else
                        Query = Query.OrderBy(e => e.PlaceName);
                    break;

                default:
                    Query = Query.OrderBy(e => e.PlaceName);
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

        // POST: api/Place
        [HttpPost]
        public IActionResult Post([FromBody]TblPlace nPlace)
        {
            return new JsonResult(this.repository.AddAsync(nPlace).Result, this.DefaultJsonSettings);
        }

        // PUT: api/Place/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblPlace uPlace)
        {
            return new JsonResult(this.repository.UpdateAsync(uPlace, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
