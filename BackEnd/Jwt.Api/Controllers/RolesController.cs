using Jwt.Business;
using Jwt.Entity;
using Jwt.Entity.Services;
using Jwt.Enum;
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
        private EntityRepository<Users> _usersRepository = null;
        public RolesController(
            EntityRepository<Roles> rolesRepository,
            EntityRepository<Users> usersRepository
            )
        {
            _rolesRepository = rolesRepository;
            _usersRepository = usersRepository;
        }

        [HttpPost]
        [Route("api/roles")]
        public async Task<HttpResponseMessage> GetAllRoles()
        {
            var allRoles = await _rolesRepository.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, allRoles);
        }

        [HttpPost]
        [Route("api/roles/assign")]
        public async Task<HttpResponseMessage> AssignRoleToUser([FromUri] Guid idUser, [FromBody] RolesReq roleInput )
        {
            var specificUsers = await _usersRepository.GetById(idUser);

            if(specificUsers == null) return Request.CreateResponse(HttpStatusCode.NotFound,"Utilisateur Introuvable pour assigner le Role");

            List<Roles> roles = new List<Roles>();

            foreach (RolesEnum item in roleInput.Roles)
            {
                Roles role = ((RolesRepository)_rolesRepository).FindRolesInDb(item);
                roles.Add(role);
            }

            try
            {
                specificUsers.Role = roles;
                _usersRepository.SaveOrUpdate(specificUsers);
            }
            catch (Exception){throw;}

            return Request.CreateResponse(HttpStatusCode.OK, specificUsers);
        }
    }
}
