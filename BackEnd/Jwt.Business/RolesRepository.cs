using Jwt.Entity;
using Jwt.Enum;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Business
{
    public class RolesRepository : EntityRepository<Roles>
    {
        public Roles FindRolesInDb(RolesEnum roles)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                return session.Query<Roles>().Where(x => x.Role == roles).FirstOrDefault();
            }
        }
    }
}
