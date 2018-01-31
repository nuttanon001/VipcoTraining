using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using VipcoTraining.Models;
using VipcoTraining.Services.Interfaces;

namespace VipcoTraining.Services.Classes
{
    public class Repository2nd<TEntity> :
        IRepository2nd<TEntity> where TEntity:class
    {
        #region PrivateMembers

        private ApplicationContext Context;

        #endregion

        #region Constructor
        /// <summary>
        /// The contructor requires an open DataContext to work with
        /// </summary>
        /// <param name="context">An open DataContext</param>

        public Repository2nd(ApplicationContext context)
        {
            this.Context = context;
        }
        #endregion

        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <param name="select">A linq expression select to find one</param>
        /// <returns>An ICollection of object which match the expression filter</returns>
        public async Task<ICollection<object>> FindAllWithSelect(
            Expression<Func<TEntity, object>> select,
            Expression<Func<TEntity, bool>> match,
            int Skip, int Row,
            Expression<Func<TEntity, string>> order = null,
            Expression<Func<TEntity, string>> orderDesc = null)
        {
            var Query = this.Context.Set<TEntity>().Where(match);
            //Order
            if (order != null)
                Query = Query.OrderBy(order);
            else if (orderDesc != null)
                Query = Query.OrderByDescending(orderDesc);
            //Skip Take
            Query = Query.Skip(Skip).Take(Row);

            return await Query.AsNoTracking().Select(select).ToListAsync();
        }
    }
}
