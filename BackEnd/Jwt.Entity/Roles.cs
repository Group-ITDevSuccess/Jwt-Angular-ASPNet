using Jwt.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Entity
{
    public class Roles : Entity
    {
        public virtual RolesEnum Role { get; set; }
    }
}
