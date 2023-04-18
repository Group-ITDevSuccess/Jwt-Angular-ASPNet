using Jwt.Business;
using Jwt.Entity;
using Jwt.Entity.Services;
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
        public UsersController(EntityRepository<Users> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpPost]
        [Route("api/users")]
        public async Task<HttpResponseMessage> GetAllUsers()
        {
            var allUsers = await _usersRepository.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, allUsers);
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

            try
            {
                _usersRepository.SaveOrUpdate(user);
                return Request.CreateResponse(HttpStatusCode.Created, user.Id);
            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Erreur lors de l'ajout de user : {e.Message}");
            }
        }
    }
}
