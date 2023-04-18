using Jwt.Entity;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Business
{
    public class EntityRepository<T> where T : Entity.Entity
    {
        public async Task<T> GetById(Guid Id)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                return await session.GetAsync<T>(Id);
            }
        }

        public async Task<List<T>> GetAll(int skip = 0, int take = -1)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                IQueryable<T> query = session.Query<T>();

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }
                if (take > -1)
                {
                    query = query.Take(take);
                }

                var a = await query.ToListAsync<T>();
                return a;
            }
        }

        public void SaveOrUpdate(T entity)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public async void SaveOrUpdateAsynk(T entity)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        await session.SaveOrUpdateAsync(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public async Task DeleteById(Guid Id)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        T entity = session.Get<T>(Id);
                        await session.DeleteAsync(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public async Task Delete(T entity)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        await session.DeleteAsync(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
    }
}
