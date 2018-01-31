using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoTraining.ViewModels;

namespace VipcoTraining.Services.Interfaces
{
    public interface IEmployeeRepository
    {
        Tuple<IEnumerable<EmployeeLazyViewModel2>, int> GetEmployeeWithLazy(LazyLoadViewModel LazyLoad);
    }
}
