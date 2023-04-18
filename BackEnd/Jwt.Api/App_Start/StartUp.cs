using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

[assembly: OwinStartup(typeof(Jwt.Api.App_Start.StartUp))]
namespace Jwt.Api.App_Start
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            /*UnityContainer unityContainer = new UnityContainer();*/
           /* SetDependencies(unityContainer);
            ConfigureOAuth(app, unityContainer);*/
        }
/*
        private void SetDependencies(UnityContainer container)
        {
            container.RegisterType<IAuthRepository, AuthRepository>();
            container.RegisterType<EntityRepository<Users>, UsersRepository>();
            container.RegisterType<IAuthenticationStrategy, DefaultAuthenticationStrategy>();
        }

        private string audienceKey = ConfigurationManager.AppSettings["JWT_AUDIENCE"];
        private string issuerKey = ConfigurationManager.AppSettings["JWT_ISSUER"];
        private string secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
        private string expMinute = ConfigurationManager.AppSettings["JWT_EXP_MINUTE"];
        private string getTokenUri = ConfigurationManager.AppSettings["JWT_GETTOKEN_URI"];

        public void ConfigureOAuth(IAppBuilder app, UnityContainer container)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(getTokenUri),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Int32.Parse(expMinute)),
                Provider = container.Resolve<OAuthProvider>(),
                RefreshTokenProvider = container.Resolve<RefrechTokenProvider>(),
                AccessTokenFormat = new JwtProvider(issuerKey)
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AllowedAudiences = new[] { audienceKey },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuerKey, TextEncodings.Base64Url.Decode(secretKey))
                    },
                });
        }*/
    }
}