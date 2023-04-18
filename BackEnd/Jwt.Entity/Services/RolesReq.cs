using Jwt.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Entity.Services
{
    public class RolesReq
    {
        [JsonProperty("roles")]
        public List<RolesEnum> Roles { get; set; }
    }
}
