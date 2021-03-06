using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.IDAL
{
    public interface IBaseRepository<T> where T : class,new()
    {
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

        //IQueryable<T> LoadPageEntities<model>(int pageIdex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, string orderby, bool? isAsc);

        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc);


        bool DeleteEntity(T entity);
        bool EditEntity(T entity);
        T AddEntity(T entity);

        IQueryable<T> LoadEntitiesAll(string entity);
    }
}
