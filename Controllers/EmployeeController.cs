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
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        #region PrivateMenbers

        private readonly IRepository<TblEmployee> repository;
        private readonly IEmployeeRepository repositoryEmployee;
        private readonly IMapper mapper;

        private JsonSerializerSettings DefaultJsonSettings =>
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        private List<MapType> ConverterTableToViewModel<MapType,TableType>(ICollection<TableType> tables)
        {
            var listData = new List<MapType>();
            foreach (var item in tables)
                listData.Add(this.mapper.Map<TableType, MapType>(item));
            return listData;
        }

        #endregion PrivateMenbers

        #region Constructor

        public EmployeeController(IRepository<TblEmployee> repo,
            IMapper map,
            IEmployeeRepository repoEmp)
        {
            this.repository = repo;
            this.mapper = map;
            this.repositoryEmployee = repoEmp;
        }

        #endregion

        // GET: api/Employee
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.repository.GetAllAsync().Result, this.DefaultJsonSettings);
        }

        // GET: api/Employee/GetWithRelate
        [HttpGet("FindAll")]
        [HttpGet("FindAll/{filter}")]
        public IActionResult GetWithRelate(string filter = "")
        {
            //relates
            Expression<Func<TblEmployee, object>> depRelate = a => a.DeptCodeNavigation;
            Expression<Func<TblEmployee, object>> secRelate = s => s.SectionCodeNavigation;
            Expression<Func<TblEmployee, object>> divRelate = d => d.DivisionCodeNavigation;
            Expression<Func<TblEmployee, object>> groRelate = g => g.GroupCodeNavigation;
            Expression<Func<TblEmployee, object>> typRelate = t => t.EmptypeCodeNavigation;
            Expression<Func<TblEmployee, object>> locRelate = l => l.Locate;

            var relates = new List<Expression<Func<TblEmployee, object>>>
                { depRelate, secRelate, divRelate, groRelate, typRelate, locRelate };

            if (!string.IsNullOrEmpty(filter))
            {
                var filters = filter.ToLower().Split(null);
                //Expression<Func<TblEmployee, bool>> isEmployee = e =>
                //    filters.Any(x => e.NameThai.Contains(x) || e.NameEng.Contains(x) || e.NickName.Contains(x) ||
                //                     e.EmpCard.Contains(x) || e.EmpCode.Contains(x));
                Expression<Func<TblEmployee, bool>> isEmployee = e =>
                  filters.Any(x => (e.NameThai + e.NameEng +
                  e.NickName + e.EmpCard +
                  e.EmpCode + e.DivisionCodeNavigation.DivisionName +
                  e.EmptypeCodeNavigation.EmpTypeDesc + e.GroupCodeNavigation.GroupDesc +
                  e.SectionCodeNavigation.SectionName + e.Locate.LocateDesc).ToLower().Contains(x));

                return new JsonResult(this.ConverterTableToViewModel<EmployeeViewModel,TblEmployee>
                    (this.repository.FindAllWithIncludeAsync(isEmployee,relates).Result), this.DefaultJsonSettings);
            }
            else
            {
                return new JsonResult(this.ConverterTableToViewModel<EmployeeViewModel, TblEmployee>
                    (this.repository.GetAllWithIncludeAsync(relates).Result), this.DefaultJsonSettings);
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return new JsonResult(this.repository.GetAsync(id).Result, this.DefaultJsonSettings);
        }

        // POST: api/Employee/GetWithLazyLoad
        [HttpPost("FindAllLayzLoad")]
        public IActionResult GetWithLazyLoad([FromBody]LazyLoadViewModel LazyLoad)
        {
            // Relate
            // Expression<Func<TblEmployee, object>> depRelate = a => a.DeptCodeNavigation;
            Expression<Func<TblEmployee, object>> secRelate = s => s.SectionCodeNavigation;
            //Expression<Func<TblEmployee, object>> divRelate = d => d.DivisionCodeNavigation;
            Expression<Func<TblEmployee, object>> groRelate = g => g.GroupCodeNavigation;
            //Expression<Func<TblEmployee, object>> typRelate = t => t.EmptypeCodeNavigation;
            //Expression<Func<TblEmployee, object>> locRelate = l => l.Locate;
            //groRelate, typRelate, depRelate, locRelate

            var relates = new List<Expression<Func<TblEmployee, object>>>
                {  groRelate, secRelate };
            // Filter
            Expression<Func<TblEmployee, bool>> condition = null;
            string filter = "";
            string location = "";

            if (LazyLoad.Filter.IndexOf("|") > -1)
            {
                var splie = LazyLoad.Filter.Split('|');
                filter = splie.Length > 0 ? splie[0] : LazyLoad.Filter;
                location = splie.Length > 1 ? splie[1] : "";
            }
            else
                filter = LazyLoad.Filter;

            var filters = string.IsNullOrEmpty(filter) ? new string[] { "" }
                    : filter.ToLower().Split(null);

            if (string.IsNullOrEmpty(location))
            {
                condition = e =>
                      filters.Any(x => (e.NameThai + e.NameEng +
                                        e.NickName + e.EmpCard +
                                        e.EmpCode + e.GroupCodeNavigation.GroupDesc +
                                        e.SectionCodeNavigation.SectionName).ToLower().Contains(x));
            }
            else
            {
                condition = e => e.LocateId == location &&
                    filters.Any(x => (e.NameThai + e.NameEng +
                                      e.NickName + e.EmpCard +
                                      e.EmpCode + e.GroupCodeNavigation.GroupDesc +
                                      e.SectionCodeNavigation.SectionName).ToLower().Contains(x));
            }


            // Order
            Expression<Func<TblEmployee, string>> Order = null;
            Expression<Func<TblEmployee, string>> OrderDesc = null;

            switch (LazyLoad.SortField)
            {
                case "EmpCode":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.EmpCode;
                    else
                        Order = e => e.EmpCode;
                    break;
                case "NameThai":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.NameThai;
                    else
                        Order = e => e.NameThai;
                    break;
                case "SectionString":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.SectionCodeNavigation.SectionName;
                    else
                        Order = e => e.SectionCodeNavigation.SectionName;
                    break;
                case "GroupString":
                    if (LazyLoad.SortOrder == -1)
                        OrderDesc = e => e.GroupCodeNavigation.GroupDesc;
                    else
                        Order = e => e.GroupCodeNavigation.GroupDesc;
                    break;
                default:
                    OrderDesc = e => e.EmpCode;
                    break;
            }

            var Data = new List<EmployeeLazyViewModel>();
            this.repository.FindAllWithLazyLoadAsync(condition, relates, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
                       Order, OrderDesc).Result.ToList().ForEach(item => Data.Add(new EmployeeLazyViewModel(item)));
            //this.ConverterTableToViewModel<EmployeeViewModel, TblEmployee>(this.repository.FindAllWithLazyLoadAsync(condition, relates, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
            // Order, OrderDesc).Result)

            //var hasData = this.repository.FindAllWithLazyLoadAsync(condition, relates, LazyLoad.First ?? 0, LazyLoad.Rows ?? 25,
            //            Order, OrderDesc).Result;

            return new JsonResult(new
            {
                Data = Data,
                TotalRow = this.repository.CountWithMatch(condition)
            }, this.DefaultJsonSettings);
        }

        [HttpPost("FindAllLayzLoad2")]
        public IActionResult GetWithLazyLoad2([FromBody]LazyLoadViewModel LazyLoad)
        {
            var hasData = this.repositoryEmployee.GetEmployeeWithLazy(LazyLoad);
            if (hasData != null)
            {
                return new JsonResult(new
                {
                    Data = hasData.Item1,
                    TotalRow = hasData.Item2
                }, this.DefaultJsonSettings);
            }
            else
                return NotFound(new { Error = "employee not found " });
        }

        // POST: api/Employee
        [HttpPost]
        public IActionResult Post([FromBody]TblEmployee nEmployee)
        {
            return new JsonResult(this.repository.AddAsync(nEmployee).Result, this.DefaultJsonSettings);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]TblEmployee uEmployee)
        {
            return new JsonResult(this.repository.UpdateAsync(uEmployee, id).Result, this.DefaultJsonSettings);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return new JsonResult(this.repository.DeleteAsync(id).Result, this.DefaultJsonSettings);
        }
    }
}
