using Jwt.Business;
using Jwt.Entity;
using Jwt.Interface;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Jwt.Api.Providers
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        private IAuthenticationStrategy _oAuthStrategy;
        private EntityRepository<Users> _usersRepository;
        private IAuthRepository _authRepository;

        public OAuthProvider(
            IAuthenticationStrategy authenticationStrategy, EntityRepository<Users> userRepository,
            IAuthRepository authRepository
            )
        {
            this._oAuthStrategy = authenticationStrategy;
            this._usersRepository = userRepository;
            this._authRepository = authRepository;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid client_id", "client id is not set");
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (!context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            }

            Users user = this._oAuthStrategy.Authenticate(context.ClientId, context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "Email ou mot de passe incorrect");
                return Task.FromResult<object>(null);
            }
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            foreach (var role in user.Role)
            {
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Role.ToString()));
            }

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "cliend_id", context.UserName
                },
                {
                    "audience", ConfigurationManager.AppSettings["JWT_AUDIENCE"]
                },
                {
                    "secret_key", ConfigurationManager.AppSettings["JWT_SECRET_KEY"]
                },
                {
                    "role", string.Join(",",
                        oAuthIdentity.Claims.Where(x => x.Type == ClaimTypes.Role)
                        .Select(x => x.Value.ToString()).ToArray())
                }
            });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
            return base.GrantResourceOwnerCredentials(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}