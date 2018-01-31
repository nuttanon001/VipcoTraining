using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using VipcoTraining.Models;
using VipcoTraining.ViewModels;
using System.Diagnostics;
using VipcoTraining.Services.Interfaces;

namespace VipcoTraining.Services.Classes
{
    public class EmployeeRepository: IEmployeeRepository
    {
        #region PrivateMembers
        private ApplicationContext Context;
        #endregion

        #region Constructor
        public EmployeeRepository(ApplicationContext ctx)
        {
            this.Context = ctx;
        }
        #endregion Constructor

        public Tuple<IEnumerable<EmployeeLazyViewModel2>,int> GetEmployeeWithLazy(LazyLoadViewModel LazyLoad)
        {
            try
            {
                if (LazyLoad != null)
                {
                    var Query = this.Context.TblEmployee
                                    .Include(x => x.GroupCodeNavigation)
                                    .Include(x => x.SectionCodeNavigation)
                                    .Include(x => x.Locate).AsQueryable();

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

                    //var filters = string.IsNullOrEmpty(filter) ? new string[] { "" }
                    //        : filter.ToLower().Split(null);

                    if (string.IsNullOrEmpty(location))
                    {
                        // Too Slow
                        //Query = Query.Where(e => filters.Any(x => (e.NameThai + e.NameEng +
                        //                        e.NickName + e.EmpCard +
                        //                        e.EmpCode + e.GroupCodeNavigation.GroupDesc +
                        //                        e.SectionCodeNavigation.SectionName).ToLower().Contains(x)));

                        foreach(var keyword in filter.Trim().ToLower().Split(null))
                        {
                            Query = Query.Where(e => e.NameThai.ToLower().Contains(keyword) || e.NameEng.ToLower().Contains(keyword) ||
                                              e.NickName.ToLower().Contains(keyword) || e.EmpCard.ToLower().Contains(keyword) ||
                                              e.EmpCode.ToLower().Contains(keyword) || e.GroupCodeNavigation.GroupDesc.ToLower().Contains(keyword) ||
                                              e.SectionCodeNavigation.SectionName.ToLower().Contains(keyword));
                        }
                    }
                    else
                    {
                        Query = Query.Where(e => e.LocateId == location);
                        foreach (var keyword in filter.Trim().ToLower().Split(null))
                        {
                            Query = Query.Where(e => e.NameThai.ToLower().Contains(keyword) || e.NameEng.ToLower().Contains(keyword) ||
                                              e.NickName.ToLower().Contains(keyword) || e.EmpCard.ToLower().Contains(keyword) ||
                                              e.EmpCode.ToLower().Contains(keyword) || e.GroupCodeNavigation.GroupDesc.ToLower().Contains(keyword) ||
                                              e.SectionCodeNavigation.SectionName.ToLower().Contains(keyword));
                        }
                    }

                    switch (LazyLoad.SortField)
                    {
                        case "EmpCode":
                            if (LazyLoad.SortOrder == -1)
                                Query = Query.OrderByDescending(x => x.EmpCode.Length).ThenByDescending(x => x.EmpCode);
                            else
                                Query = Query.OrderBy(x => x.EmpCode.Length).ThenBy(x => x.EmpCode);
                            break;
                        case "NameThai":
                            if (LazyLoad.SortOrder == -1)
                                Query = Query.OrderByDescending(x => x.NameThai);
                            else
                                Query = Query.OrderBy(x => x.NameThai);
                            break;
                        case "SectionString":
                            if (LazyLoad.SortOrder == -1)
                                Query = Query.OrderByDescending(x => x.SectionCodeNavigation.SectionName);
                            else
                                Query = Query.OrderBy(x => x.SectionCodeNavigation.SectionName);
                            break;
                        case "GroupString":
                            if (LazyLoad.SortOrder == -1)
                                Query = Query.OrderByDescending(x => x.GroupCodeNavigation.GroupDesc);
                            else
                                Query = Query.OrderBy(x => x.GroupCodeNavigation.GroupDesc);
                            break;
                        default:
                            Query = Query.OrderByDescending(x => x.EmpCode.Length).ThenByDescending(x => x.EmpCode);
                            break;
                    }

                    int TotalRow = Query.Count();
                    Query = Query.Skip(LazyLoad.First ?? 0).Take(LazyLoad.Rows ?? 25);

                    return new Tuple<IEnumerable<EmployeeLazyViewModel2>, int>
                        (
                            Query.AsEnumerable<TblEmployee>().Select(x => new EmployeeLazyViewModel2()
                                {
                                    EmpCode = x.EmpCode,
                                    NameThai = x.NameThai,
                                    GroupString = x.GroupCodeNavigation.GroupDesc,
                                    SectionString = x.SectionCodeNavigation.SectionName,
                                }).AsEnumerable(), TotalRow
                        );
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Has error {ex.ToString()}");
            }
            return null;
        }
    }
}
