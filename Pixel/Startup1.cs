using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Pixel.Models;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(Pixel.Startup1))]

namespace Pixel
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCors(CorsOptions.AllowAll);

            //create the token 
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                // when i write /login come here and make the token
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(360),
                AllowInsecureHttp = true,  // call by http not https protocal
                // how to create token (fields stored in token )
                Provider = new TokenCreate()


            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration configu = new HttpConfiguration();
            configu.MapHttpAttributeRoutes();
            

      configu.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
                );



            configu.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888


            app.UseWebApi(configu);

        }
    }

    internal class TokenCreate : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();//any clientid Valid
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add(" Access - Control - Allow - Origin ", new[] { "*" });            //chech if user valid or not 

            UserStore<IdentityUser> store = new UserStore<IdentityUser>( new PixelContext());
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);

            // username and pass com from postman in context obj
            IdentityUser user = await manager.FindAsync(context.UserName, context.Password);
              
          
      if (user == null)
            {
                context.SetError("grant_error", "User name or pass is not valid !");
            }
            else
            {
                // create token 
                ClaimsIdentity claims = new ClaimsIdentity(context.Options.AuthenticationType);
                //fileds 
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

                context.Validated(claims);


            }




        }
    }
}

