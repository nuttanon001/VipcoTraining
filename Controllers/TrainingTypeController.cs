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
    [Route("api/TrainingType")]
    public class TrainingTypeController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblTrainingType> repository;
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

        public TrainingTypeController(IRepository<TblTrainingType> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
        }

        #endregion

        // GET: api/TrainingType
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }
        // GET: api/TrainingType/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var hasData = this.repository.GetAsync(id).Result;
            if (hasData.TrainingTypeParentId != null)
            {
                if (hasData.TrainingTypeParentId.Value > 0)
                    hasData.TrainingTypeParent = this.repository.Get(hasData.TrainingTypeParentId.Value);

            }

            return new JsonResult(this.mapper.Map<TblTrainingType,TrainingTypeViewModel>(hasData), this.DefaultJsonSettings);
        }

        // GET: api/TrainingType/NodeFindAll
        [HttpGet("NodeFindAll")]
        [HttpGet("NodeFindAll/{filter}")]
        public IActionResult GetDataToNode(string filter = "")
        {
            // condition
            //Expression<Func<TblTrainingType, bool>> condition = e => e.TrainingTypeParentId == null || e.TrainingTypeParentId < 1;

            var HasData = new List<TreeNodeViewModel<TblTrainingType>>();
            foreach(var data in this.repository.GetAllWithRelateAsync(null).Result)
            {
                if (data.TrainingTypeParentId != null || data?.TrainingTypeParentId < 1)
                    continue;

                var newTree = new TreeNodeViewModel<TblTrainingType>(data);
                if (data.InverseTrainingTypeParent != null)
                {
                    foreach(var node in data.InverseTrainingTypeParent)
                    {
                        var newNode = new TreeNodeViewModel<TblTrainingType>(node);
                        if (node.InverseTrainingTypeParent != null)
                        {
                            foreach(var subNode in node.InverseTrainingTypeParent)
                                newNode.AddChild(new TreeNodeViewModel<TblTrainingType>(subNode));
                        }
                        newNode.data.InverseTrainingTypeParent = null;
                        newTree.AddChild(newNode);
                    }
                }
                newTree.data.InverseTrainingTypeParent = null;
                HasData.Add(newTree);
            }
            return new JsonResult(HasData, this.DefaultJsonSettings);
        }

        // POST: api/TrainingType/GetWithLazyLoad
        [HttpPost("FindAllLayzLoad")]
        public IActionResult GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate

            // Filter
            var filters = string.IsNullOrEmpty(LazyLoad.Filter) ? new string[] { "" }
                    : LazyLoad.Filter.ToLower().Split(null);

            Expression<Func<TblTrainingType, bool>> condition = e =>
              filters.Any(x => (e.TrainingTypeName + e.Detail).ToLower().Contains(x));
            // Order
            Expression<Func<TblTrainingType, string>> Order = null;
            Expression<Func<TblTrainingType, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "TrainingTypeId":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingTypeId.ToString("00");
                    else
                        Order = e => e.TrainingTypeId.ToString("00");
                    break;
                case "TrainingTypeName":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingTypeName;
                    else
                        Order = e => e.TrainingTypeName;
                    break;
                case "TrainingTypeParentString":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.TrainingTypeParent.TrainingTypeName;
                    else
                        Order = e => e.TrainingTypeParent.TrainingTypeName;
                    break;
                default:
                    Order = e => e.TrainingTypeId.ToString("00");
                    break;
            }

            return new JsonResult(new
            {
                Data = this.ConverterTableToViewModel<TrainingTypeViewModel,TblTrainingType>(
                    this.repository.FindAllWithLazyLoadAsync(condition, null, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25, Order, OrderDesc).Result),
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }
        // POST: api/TrainingType
        [HttpPost]
        public IActionResult Post([FromBody]TblTrainingType nTrainingType)
        {
            nTrainingType.Creator = nTrainingType.Creator ?? "Someone";
            nTrainingType.CreateDate = DateTime.Now;

            return new JsonResult(this.repository.AddAsync(nTrainingType).Result, this.DefaultJsonSettings);
        }

        // PUT: api/TrainingType/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]TblTrainingType uTrainingType)
        {
            uTrainingType.Modifyer = uTrainingType.Modifyer ?? "Someone";
            uTrainingType.ModifyDate = DateTime.Now;

            return new JsonResult(this.repository.UpdateAsync(uTrainingType, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
