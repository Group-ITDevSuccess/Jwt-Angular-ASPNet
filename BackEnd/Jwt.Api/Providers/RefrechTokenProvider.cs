using Jwt.Business;
using Jwt.Entity;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace Jwt.Api.Providers
{
    public class RefrechTokenProvider : IAuthenticationTokenProvider
    {
        private EntityRepository<Users> _usersRepository;
        public RefrechTokenProvider(EntityRepository<Users> usersRepository)
        {
            this._usersRepository = usersRepository;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            DateTime now = DateTime.UtcNow;
            context.Ticket.Properties.IssuedUtc = now;
            context.Ticket.Properties.ExpiresUtc = now.AddMinutes(int.Parse(ConfigurationManager.AppSettings["REFRESH_TOKEN_EXP_MINUTE"]));
            context.SetToken(context.SerializeTicket());
            return Task.CompletedTask;
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            if (!context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            }

            context.DeserializeTicket(context.Token);
            return Task.CompletedTask;
        }

        public string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}