using Jwt.Entity;
using Jwt.Entity.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Business
{
    public class UsersRepository : EntityRepository<Users>
    {
        public Users FindUsersInDb(UsersReq user)
        {
            using(ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                return session.Query<Users>().Where(x => x.Email == user.Email).FirstOrDefault();
            }
        }
    }
}
