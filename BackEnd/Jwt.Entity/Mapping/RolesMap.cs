using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Entity.Mapping
{
    class RolesMap : SubclassMap<Roles>
    {
        public RolesMap()
        {
            Abstract();
            Map(x => x.Role).Not.Nullable();

            HasManyToMany(x => x.User)
                .Cascade.All()
                .Inverse()
                .Table("Users_Roles")
                .Not.LazyLoad();
        }
    }
}
