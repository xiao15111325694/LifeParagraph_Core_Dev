using Life_Web.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Life_Core_Repositories
{
    public class Repository<T> : IRepository<T> where T:class
    {
        #region 数据上下文

        /// <summary>
        /// 数据上下文
        /// </summary>
        private ApplicationDbContext _Context;
        public Repository(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        #endregion

        #region 单模型 CRUD 操作

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool Save(T entity, bool IsCommit = true)
        {
            _Context.Set<T>().Add(entity);
            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 增加一条记录(返回记录)
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual T Save(T entity)
        {
            _Context.Set<T>().Add(entity);
            if (_Context.SaveChanges() > 0)
                return entity;
            else
                return null;
        }
        /// <summary>
        /// 增加一条记录(异步方式)
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> SaveAsync(T entity, bool IsCommit = true)
        {
            _Context.Set<T>().Add(entity);
            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool Update(T entity, bool IsCommit = true)
        {
            _Context.Set<T>().Attach(entity);
            _Context.Entry<T>(entity).State = EntityState.Modified;
            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 更新一条记录（异步方式）
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity, bool IsCommit = true)
        {
            _Context.Set<T>().Attach(entity);
            _Context.Entry<T>(entity).State = EntityState.Modified;
            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 增加或更新一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsSave">是否增加</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool SaveOrUpdate(T entity, bool IsSave, bool IsCommit = true)
        {
            return IsSave ? Save(entity, IsCommit) : Update(entity, IsCommit);
        }
        /// <summary>
        /// 增加或更新一条记录（异步方式）
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsSave">是否增加</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> SaveOrUpdateAsync(T entity, bool IsSave, bool IsCommit = true)
        {
            return IsSave ? await SaveAsync(entity, IsCommit) : await UpdateAsync(entity, IsCommit);
        }

        /// <summary>
        /// 通过Lamda表达式获取实体
        /// </summary>
        /// <param name="predicate">Lamda表达式（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _Context.Set<T>().AsNoTracking().SingleOrDefault(predicate);
        }
        /// <summary>
        /// 通过Lamda表达式获取实体（异步方式）
        /// </summary>
        /// <param name="predicate">Lamda表达式（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => _Context.Set<T>().AsNoTracking().SingleOrDefault(predicate));
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool Delete(T entity, bool IsCommit = true)
        {
            if (entity == null) return false;
            _Context.Set<T>().Attach(entity);
            _Context.Set<T>().Remove(entity);

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 删除一条记录（异步方式）
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(T entity, bool IsCommit = true)
        {
            if (entity == null) return await Task.Run(() => false);
            _Context.Set<T>().Attach(entity);
            _Context.Set<T>().Remove(entity);
            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false); ;
        }

        #endregion

        #region 多模型 操作

        /// <summary>
        /// 增加多条记录，同一模型
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool SaveList(List<T> T1, bool IsCommit = true)
        {
            if (T1 == null || T1.Count == 0) return false;

            T1.ToList().ForEach(item =>
            {
                _Context.Set<T>().Add(item);
            });

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 增加多条记录，同一模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> SaveListAsync(List<T> T1, bool IsCommit = true)
        {
            if (T1 == null || T1.Count == 0) return await Task.Run(() => false);

            T1.ToList().ForEach(item =>
            {
                _Context.Set<T>().Add(item);
            });

            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 增加多条记录，独立模型
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool SaveList<T1>(List<T1> T, bool IsCommit = true) where T1 : class
        {
            if (T == null || T.Count == 0) return false;
            var tmp = _Context.ChangeTracker.Entries<T>().ToList();
            foreach (var x in tmp)
            {
                var properties = typeof(T).GetTypeInfo().GetProperties();
                foreach (var y in properties)
                {
                    var entry = x.Property(y.Name);
                    entry.CurrentValue = entry.OriginalValue;
                    entry.IsModified = false;
                    y.SetValue(x.Entity, entry.OriginalValue);
                }
                x.State = EntityState.Unchanged;
            }
            T.ToList().ForEach(item =>
            {
                _Context.Set<T1>().Add(item);
            });
            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 增加多条记录，独立模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> SaveListAsync<T1>(List<T1> T, bool IsCommit = true) where T1 : class
        {
            if (T == null || T.Count == 0) return await Task.Run(() => false);
            var tmp = _Context.ChangeTracker.Entries<T>().ToList();
            foreach (var x in tmp)
            {
                var properties = typeof(T).GetTypeInfo().GetProperties();
                foreach (var y in properties)
                {
                    var entry = x.Property(y.Name);
                    entry.CurrentValue = entry.OriginalValue;
                    entry.IsModified = false;
                    y.SetValue(x.Entity, entry.OriginalValue);
                }
                x.State = EntityState.Unchanged;
            }
            T.ToList().ForEach(item =>
            {
                _Context.Set<T1>().Add(item);
            });
            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 更新多条记录，同一模型
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool UpdateList(List<T> T1, bool IsCommit = true)
        {
            if (T1 == null || T1.Count == 0) return false;

            T1.ToList().ForEach(item =>
            {
                _Context.Set<T>().Attach(item);
                _Context.Entry<T>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 更新多条记录，同一模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateListAsync(List<T> T1, bool IsCommit = true)
        {
            if (T1 == null || T1.Count == 0) return await Task.Run(() => false);

            T1.ToList().ForEach(item =>
            {
                _Context.Set<T>().Attach(item);
                _Context.Entry<T>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 更新多条记录，独立模型
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool UpdateList<T1>(List<T1> T, bool IsCommit = true) where T1 : class
        {
            if (T == null || T.Count == 0) return false;

            T.ToList().ForEach(item =>
            {
                _Context.Set<T1>().Attach(item);
                _Context.Entry<T1>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 更新多条记录，独立模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateListAsync<T1>(List<T1> T, bool IsCommit = true) where T1 : class
        {
            if (T == null || T.Count == 0) return await Task.Run(() => false);

            T.ToList().ForEach(item =>
            {
                _Context.Set<T1>().Attach(item);
                _Context.Entry<T1>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 删除多条记录，同一模型
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool DeleteList(List<T> T1, bool IsCommit = true)
        {
            if (T1 == null || T1.Count == 0) return false;

            T1.ToList().ForEach(item =>
            {
                _Context.Set<T>().Attach(item);
                _Context.Set<T>().Remove(item);
            });

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 删除多条记录，同一模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteListAsync(List<T> T1, bool IsCommit = true)
        {
            if (T1 == null || T1.Count == 0) return await Task.Run(() => false);

            T1.ToList().ForEach(item =>
            {
                _Context.Set<T>().Attach(item);
                _Context.Set<T>().Remove(item);
            });

            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 删除多条记录，独立模型
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual bool DeleteList<T1>(List<T1> T, bool IsCommit = true) where T1 : class
        {
            if (T == null || T.Count == 0) return false;

            T.ToList().ForEach(item =>
            {
                _Context.Set<T1>().Attach(item);
                _Context.Set<T1>().Remove(item);
            });

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 删除多条记录，独立模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteListAsync<T1>(List<T1> T, bool IsCommit = true) where T1 : class
        {
            if (T == null || T.Count == 0) return await Task.Run(() => false);

            T.ToList().ForEach(item =>
            {
                _Context.Set<T1>().Attach(item);
                _Context.Set<T1>().Remove(item);
            });

            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        /// <summary>
        /// 通过Lamda表达式，删除一条或多条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool Delete(Expression<Func<T, bool>> predicate, bool IsCommit = true)
        {
            IQueryable<T> entry = (predicate == null) ? _Context.Set<T>().AsQueryable() : _Context.Set<T>().Where(predicate);
            List<T> list = entry.ToList();

            if (list != null && list.Count == 0) return false;
            list.ForEach(item => {
                _Context.Set<T>().Attach(item);
                _Context.Set<T>().Remove(item);
            });

            if (IsCommit)
                return _Context.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// 通过Lamda表达式，删除一条或多条记录（异步方式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, bool IsCommit = true)
        {
            IQueryable<T> entry = (predicate == null) ? _Context.Set<T>().AsQueryable() : _Context.Set<T>().Where(predicate);
            List<T> list = entry.ToList();

            if (list != null && list.Count == 0) return await Task.Run(() => false);
            list.ForEach(item => {
                _Context.Set<T>().Attach(item);
                _Context.Set<T>().Remove(item);
            });

            if (IsCommit)
                return await Task.Run(() => _Context.SaveChanges() > 0);
            else
                return await Task.Run(() => false);
        }

        #endregion

        #region 获取多条数据操作

        /// <summary>
        /// Lamda返回IQueryable集合，延时加载数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> LoadAll(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? _Context.Set<T>().Where(predicate).AsNoTracking<T>() : _Context.Set<T>().AsQueryable<T>().AsNoTracking<T>();
        }
        /// <summary>
        /// 返回IQueryable集合，延时加载数据（异步方式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> LoadAllAsync(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? await Task.Run(() => _Context.Set<T>().Where(predicate).AsNoTracking<T>()) : await Task.Run(() => _Context.Set<T>().AsQueryable<T>().AsNoTracking<T>());
        }

        /// <summary>
        /// 返回List<T>集合,不采用延时加载
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual List<T> LoadListAll(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? _Context.Set<T>().Where(predicate).AsNoTracking().ToList() : _Context.Set<T>().AsQueryable<T>().AsNoTracking().ToList();
        }
        // <summary>
        /// 返回List<T>集合,不采用延时加载（异步方式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> LoadListAllAsync(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? await Task.Run(() => _Context.Set<T>().Where(predicate).AsNoTracking().ToList()) : await Task.Run(() => _Context.Set<T>().AsQueryable<T>().AsNoTracking().ToList());
        }



        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回实体对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <typeparam name="TResult">数据结果，与TEntity一致</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>实体集合</returns>
        public virtual List<TResult> QueryEntity<TEntity, TOrderBy, TResult>
            (Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TOrderBy>> orderby,
            Expression<Func<TEntity, TResult>> selector,
            bool IsAsc)
            where TEntity : class
            where TResult : class
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = IsAsc ? query.OrderBy(orderby) : query.OrderByDescending(orderby);
            }
            if (selector == null)
            {
                return query.Cast<TResult>().AsNoTracking().ToList();
            }
            return query.Select(selector).AsNoTracking().ToList();
        }
        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回实体对象集合（异步方式）
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <typeparam name="TResult">数据结果，与TEntity一致</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>实体集合</returns>
        public virtual async Task<List<TResult>> QueryEntityAsync<TEntity, TOrderBy, TResult>
            (Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TOrderBy>> orderby,
            Expression<Func<TEntity, TResult>> selector,
            bool IsAsc)
            where TEntity : class
            where TResult : class
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = IsAsc ? query.OrderBy(orderby) : query.OrderByDescending(orderby);
            }
            if (selector == null)
            {
                return await Task.Run(() => query.Cast<TResult>().AsNoTracking().ToList());
            }
            return await Task.Run(() => query.Select(selector).AsNoTracking().ToList());
        }

        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回Object对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>自定义实体集合</returns>
        public virtual List<object> QueryObject<TEntity, TOrderBy>
            (Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TOrderBy>> orderby,
            Func<IQueryable<TEntity>,
            List<object>> selector,
            bool IsAsc)
            where TEntity : class
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = IsAsc ? query.OrderBy(orderby) : query.OrderByDescending(orderby);
            }
            if (selector == null)
            {
                return query.AsNoTracking().ToList<object>();
            }
            return selector(query);
        }
        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回Object对象集合（异步方式）
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>自定义实体集合</returns>
        public virtual async Task<List<object>> QueryObjectAsync<TEntity, TOrderBy>
            (Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TOrderBy>> orderby,
            Func<IQueryable<TEntity>,
            List<object>> selector,
            bool IsAsc)
            where TEntity : class
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = IsAsc ? query.OrderBy(orderby) : query.OrderByDescending(orderby);
            }
            if (selector == null)
            {
                return await Task.Run(() => query.AsNoTracking().ToList<object>());
            }
            return await Task.Run(() => selector(query));
        }


        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回动态类对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>动态类</returns>
        public virtual dynamic QueryDynamic<TEntity, TOrderBy>
            (Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TOrderBy>> orderby,
            Func<IQueryable<TEntity>,
            List<object>> selector,
            bool IsAsc)
            where TEntity : class
        {
            List<object> list = QueryObject<TEntity, TOrderBy>
                 (where, orderby, selector, IsAsc);
            return list;
        }
        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回动态类对象集合（异步方式）
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>动态类</returns>
        public virtual async Task<dynamic> QueryDynamicAsync<TEntity, TOrderBy>
            (Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TOrderBy>> orderby,
            Func<IQueryable<TEntity>,
            List<object>> selector,
            bool IsAsc)
            where TEntity : class
        {
            List<object> list = QueryObject<TEntity, TOrderBy>
                 (where, orderby, selector, IsAsc);
            return await Task.Run(() => list);
        }

        #endregion

        #region 验证是否存在

        /// <summary>
        /// 验证当前条件是否存在相同项
        /// </summary>
        public virtual bool IsExist(Expression<Func<T, bool>> predicate)
        {
            var entry = _Context.Set<T>().Where(predicate);
            return (entry.Any());
        }
        /// <summary>
        /// 验证当前条件是否存在相同项（异步方式）
        /// </summary>
        public virtual async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            var entry = _Context.Set<T>().Where(predicate);
            return await Task.Run(() => entry.Any());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public IQueryable<T> LoadAllBySql(string sql, params DbParameter[] para)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public Task<IQueryable<T>> LoadAllBySqlAsync(string sql, params DbParameter[] para)
        {
            throw new NotImplementedException();
        }

        public List<T> LoadListAllBySql(string sql, params DbParameter[] para)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> LoadListAllBySqlAsync(string sql, params DbParameter[] para)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string sql, params DbParameter[] para)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistAsync(string sql, params DbParameter[] para)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
           return _Context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Task.Run(() => _Context.Set<T>().Find(id));
        }

        public List<T> GetAll()
        {
            return _Context.Set<T>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }


      /// <summary>
      /// 数据分页
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="pageIndex"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
        public List<T> GetPageList(List<T> entity, int pageIndex, int pageSize)
        {

            if (entity.Count() > 0)
            {

                return entity.Skip(pageIndex - 1).Take(pageIndex * pageSize).ToList();
            }
            else
            {
                return entity;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="entity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> GetPageList(Func<T, bool> predicate, List<T> entity, int pageIndex, int pageSize)
        {

            if (entity.Count() > 0)
            {

                return entity.Skip(pageIndex - 1).Where(predicate).Take(pageIndex * pageSize).ToList();
            }
            else
            {
                return entity;
            }

        }

        /// <summary>
        /// 数据分页
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(List<T> entity, int pageIndex, int pageSize)
        {
            if (entity.Count() > 0)
            {

                return await Task.Run(() => entity.Skip(pageIndex - 1).Take(pageIndex * pageSize).ToList());
            }
            else
            {
                return await Task.Run(() => entity);
            }
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="predicate"></param>
      /// <param name="entity"></param>
      /// <param name="pageIndex"></param>
      /// <param name="pageSize"></param>
      /// <returns></returns>
        public async Task<List<T>> GetPageListAsync(Func<T, bool> predicate, List<T> entity, int pageIndex, int pageSize)
        {
            if (entity.Count() > 0)
            {

                return await Task.Run(() => entity.Skip(pageIndex - 1).Where(predicate).Take(pageIndex * pageSize).ToList());
            }
            else
            {
                return await Task.Run(() => entity);
            }
        }

        #endregion

    }
}
