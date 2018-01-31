using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using VipcoTraining.Models;

namespace VipcoTraining.Services.Interfaces
{
    public interface IRepository2nd<TEntity> where TEntity : class
    {
        Task<ICollection<object>> FindAllWithSelect(
            Expression<Func<TEntity, object>> select,
            Expression<Func<TEntity, bool>> match,
            int Skip, int Row,
            Expression<Func<TEntity, string>> order = null,
            Expression<Func<TEntity, string>> orderDesc = null
        );
    }
}
