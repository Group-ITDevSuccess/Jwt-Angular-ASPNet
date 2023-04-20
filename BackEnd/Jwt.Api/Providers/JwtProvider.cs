using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace Jwt.Api.Providers
{
    public class JwtProvider : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;

        public JwtProvider(string issuer)
        {
            this._issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentException("AuthenticationTicket in JwtProvider");
            }
            string audienceId = data.Properties.Dictionary.ContainsKey("audience") ? data.Properties.Dictionary["audience"] : null;
            if (string.IsNullOrWhiteSpace(audienceId))
            {
                throw new InvalidOperationException("AuthenticationTicket.Properties in JwtProvider does not include audience");
            }

            string symetricKeyAsBase64 = data.Properties.Dictionary.ContainsKey("secret_key") ? data.Properties.Dictionary["secret_key"] : null;
            if (string.IsNullOrWhiteSpace(symetricKeyAsBase64))
            {
                throw new InvalidOperationException("AuthenticationTicket.Properties in JwtProvider does not include secret_key");
            }

            var keyByteArray = TextEncodings.Base64Url.Decode(symetricKeyAsBase64);
            var signingKey = new HmacSigningCredentials(keyByteArray);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime,
                expires.Value.UtcDateTime, signingKey);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException("JwtProvider.Unprotect()");
        }
    }
}