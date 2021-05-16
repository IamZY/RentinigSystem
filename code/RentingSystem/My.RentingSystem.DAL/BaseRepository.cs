using My.RentingSystem.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.DAL
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class,new()
    {
        /// <summary>
        /// EF上下文对象
        /// </summary>
        DbContext Db = DbContextFactory.CreateDbContext();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where<T>(whereLambda);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            Db.Set<T>().Attach(entity);
            Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            Db.Set<T>().Add(entity);
            return entity;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntitiesAll(string entity)
        {
            return string.IsNullOrEmpty(entity) ? Db.Set<T>().AsQueryable() : Db.Set<T>().Include(entity).AsQueryable();
        }

        /// <summary>
        /// 实现对数据的分页查询
        /// </summary>
        /// <typeparam name="model">实体对象</typeparam>
        /// <param name="pageIdex">页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="toalCount">总记录数</param>
        /// <param name="whereLambda">条件表达式</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="isAsc">是否升序（true:表示升序, false：降序）</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int start, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            /**
             * 为这个表和另一个表是有一对多关系的,当序列化表1的时候,会找到和另一个表2关联的字段,就会到另一个表2中序列化,然后另一个表2中也有一个字段和表1相关联.这样.序列化就会发生这种错误! 
             */
            Db.Configuration.LazyLoadingEnabled = false;
            Db.Configuration.ProxyCreationEnabled = false;
            var temp = Db.Set<T>().Where<T>(whereLambda.Compile()).AsQueryable();
            totalCount = temp.Count();
            if (isAsc)//升序
            {
                // temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>(start - 1).Take<T>(pageSize);
            }
            else
            {
                //temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
                temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>(start - 1).Take<T>(pageSize);
            }
            return temp;

        }
    }
}
