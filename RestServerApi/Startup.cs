using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using RestServerApi.Formats;
using RestServerApi.Providers;
using System;
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
                AccessTokenFormat = new JsonWebTokenDataFormat()
            };
            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new JsonWebTokenDataFormat()
            });

        }
    }
}