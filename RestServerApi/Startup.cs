using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
//using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using RestServerApi.App_Start;
using RestServerApi.Formats;
using RestServerApi.Providers;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestServerApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);
            //config.MessageHandlers.Add(new PreflightRequestsHandler());

            //WebApiConfig.Register(config);

            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(config);

        }

        public void ConfigureOAuth(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //We enable http only for Dev enviroment. Otherwise we will set it to false.
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(10),
                Provider = new CustomOAuthServerProvider(),
                //AuthenticationType = "Bearer",
                AccessTokenFormat = new JsonWebTokenDataFormat("http://www.RestServerApi.com.bd")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);


            var issuer = "http://www.RestServerApi.com.bd";
            var audience = "IAmTheFirstClient";
            var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");


            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new JsonWebTokenDataFormat("http://www.RestServerApi.com.bd"),
                //AuthenticationMode = AuthenticationMode.Active,
                //AuthenticationType = "Bearer",
                //Description = new AuthenticationDescription()
            });

        }
    }
}