using Jwt.Entity;
using Jwt.Interface;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Business
{
    public class AuthRepository : IAuthRepository
    {
        public Users FindByUserEmail(string email)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                return session.Query<Users>().Where(x => x.Email == email).FirstOrDefault();
            }
        }

        public void DisableGeneratedToken(string email)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                Users u = session.Query<Users>().Where(x => x.Email == email).FirstOrDefault();

                if (u != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        try
                        {
                            u.HasToken = false;
                            session.SaveOrUpdate(u);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }

        public Users EnableGeneretedToken(string email)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                Users u = session.Query<Users>().Where(x => x.Email == email).FirstOrDefault();

                if (u != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        try
                        {
                            u.HasToken = true;
                            session.SaveOrUpdate(u);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
                return u;
            }
        }

        public bool HasToken(string email)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                Users u = session.Query<Users>().Where(x => x.Email == email).FirstOrDefault();
                if (u != null)
                {
                    return u.HasToken;
                }
                return false;
            }
        }
    }
}
