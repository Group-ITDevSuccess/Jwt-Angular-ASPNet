using Jwt.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Jwt.Api.Controllers
{
    public class FixturesController : ApiController
    {
        [HttpGet]
        [Route("api/fixtures/roles")]
        public async Task<HttpResponseMessage> AddFixtureRole()
        {
            try
            {
                FixtureHelpers fixture = new FixtureHelpers();
                await fixture.AddFixturesRoles();
                return Request.CreateResponse(HttpStatusCode.OK, "Roles Enum ajouter");
            }
            catch (Exception) {throw;}
        }
    }
}
