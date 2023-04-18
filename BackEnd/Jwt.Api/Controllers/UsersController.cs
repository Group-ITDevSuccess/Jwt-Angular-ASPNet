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
    public class UsersController : ApiController
    {
        private EntityRepository<Users> _usersRepository = null;
        private EntityRepository<Roles> _rolesRepository = null;
        public UsersController(
            EntityRepository<Users> usersRepository,
            EntityRepository<Roles> rolesRepository
            )
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
        }

        [HttpPost]
        [Route("api/users")]
        public async Task<HttpResponseMessage> GetAllUsers()
        {
            var allUsers = await _usersRepository.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, allUsers);
        }

        [HttpPost]
        [Route("api/users")]
        public async Task<HttpResponseMessage> GetUserById([FromUri] Guid idUser)
        {

            var specificUsers = await _usersRepository.GetById(idUser);

            if (specificUsers == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Utilisateur Introuvable");

            return Request.CreateResponse(HttpStatusCode.OK, specificUsers);
        }

        [HttpPost]
        [Route("api/users/add")]
        public HttpResponseMessage AddUsers([FromBody] UsersReq usersInput)
        {
            Users findInDb = ((UsersRepository)_usersRepository).FindUsersInDb(usersInput);

            if (findInDb != null)
            {
                return Request.CreateResponse(HttpStatusCode.Found, $"Utilisateur existe deja : {findInDb.Id}");
            }
            
            Users user = new Users();

            user.FirstName = usersInput.FirstName;
            user.LastName = usersInput.LastName;
            user.Email = usersInput.Email;
            user.PassWord = usersInput.PassWord;
            user.CreatedAt = usersInput.CreatedAt;
            user.IsDeleted = usersInput.IsDeleted;

            try
            {
                _usersRepository.SaveOrUpdate(user);
                return Request.CreateResponse(HttpStatusCode.Created, user.Id);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("api/users/update")]
        public async Task<HttpResponseMessage> UpdateUsers([FromUri] Guid idUser, [FromBody] UsersReq usersInput)
        {
            var specificUsers = await _usersRepository.GetById(idUser);

            if (specificUsers == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Utilisateur Introuvable pour le mise a jour");

            specificUsers.FirstName = usersInput.FirstName;
            specificUsers.LastName = usersInput.LastName;
            specificUsers.Email = usersInput.Email;
            specificUsers.PassWord = usersInput.PassWord;
            List<Roles> roles = new List<Roles>();

            foreach (RolesEnum item in usersInput.Role)
            {
                Roles role = ((RolesRepository)_rolesRepository).FindRolesInDb(item);
                roles.Add(role);
            }

            try
            {
                specificUsers.Role = roles;
                _usersRepository.SaveOrUpdateAsynk(specificUsers);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, specificUsers);
        }

        [HttpGet]
        [Route("api/users/remove")]
        public async Task<HttpResponseMessage> RemoveUser([FromUri] Guid idUser)
        {
            try
            {
                var specificUsers = await _usersRepository.GetById(idUser);

                if (specificUsers == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Utilisateur Introuvable pour la suppression");

                specificUsers.IsDeleted = true;

                _usersRepository.SaveOrUpdateAsynk(specificUsers);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, $"Utilisateur Effacer !");
        }

        [HttpGet]
        [Route("api/users/destroy")]
        public async Task<HttpResponseMessage> DestroyUser([FromUri] Guid idUser)
        {
            try
            {
                var specificUser = await _usersRepository.GetById(idUser);
                if (specificUser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"User with id {idUser} not found.");
                }

                foreach (var item in specificUser.Role.ToList())
                {
                    specificUser.Role.Remove(item);
                }

                await _usersRepository.Delete(specificUser);

                return Request.CreateResponse(HttpStatusCode.OK, $"User with id {idUser} has been deleted.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }


    }
}
