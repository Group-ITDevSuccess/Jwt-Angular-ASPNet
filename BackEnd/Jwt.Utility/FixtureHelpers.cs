using Jwt.Entity;
using Jwt.Enum;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Utility
{
    public class FixtureHelpers
    {
        public async Task AddFixturesRoles()
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        await session.SaveOrUpdateAsync(new Roles() { Role = RolesEnum.ADMIN });
                        await session.SaveOrUpdateAsync(new Roles() { Role = RolesEnum.CLIENT });
                        await session.SaveOrUpdateAsync(new Roles() { Role = RolesEnum.USER });
                        transaction.Commit();
                    }
                    catch (Exception e) { throw e; }
                }
            }
        }
    }
}
