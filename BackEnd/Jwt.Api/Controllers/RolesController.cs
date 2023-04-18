using Jwt.Business;
using Jwt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Jwt.Api.Controllers
{
    public class RolesController : ApiController
    {
        private EntityRepository<Roles> _rolesRepository = null;
        public RolesController(EntityRepository<Roles> rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        [HttpPost]
        [Route("api/roles")]
        public async Task<HttpResponseMessage> GetAllRoles()
        {
            var allRoles = await _rolesRepository.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, allRoles);
        }
    }
}
