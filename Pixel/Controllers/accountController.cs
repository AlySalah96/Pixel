using Pixel.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Web.Http.Owin;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace Pixel.Controllers
{
  [System.Web.Mvc.AllowAnonymous]
    public class accountController : ApiController
    {

        [HttpGet]
        [System.Web.Http.Route("api/account/IsAdmin/{userName}/{password}")]
        public async Task<Boolean>IsAdmin( string userName, string password)
        {
            PixelContext context = new PixelContext();
            UserStore<IdentityUser> store = new UserStore<IdentityUser>(context);
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
            IdentityUser user = new IdentityUser();
            user.UserName = userName;
            user.PasswordHash =password;
            var Appuser = manager.Find(user.UserName, user.PasswordHash);//(userLogin.Username, userLogin.Password);
            var roles = await manager.GetRolesAsync(Appuser.Id);

            if (roles.Count==0)
                return false;

            return true;


        }


        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> POSTRegisteration(User Account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PixelContext context = new PixelContext();
                UserStore<IdentityUser> store = new UserStore<IdentityUser>(context);
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
                IdentityUser user = new IdentityUser();
                user.UserName = Account.UserName;
                user.PasswordHash = Account.Password;
                IdentityResult result = await manager.CreateAsync(user, user.PasswordHash);

                  if (result.Succeeded )
                    {
                        if (user.UserName == "aly")
                          {

                           manager.AddToRole(user.Id, "Admin");
           
                            //MAnager SignIn
                            IAuthenticationManager authenticationManager =
                                HttpContext.Current.GetOwinContext().Authentication;
                            SignInManager<IdentityUser, string> signinmanager =
                                new SignInManager<IdentityUser, string>
                                (manager, authenticationManager);
                            signinmanager.SignIn(user, true, true);
                          }
                 
                    return Ok("Created successfully !" + Account.UserName);
                    //return Redirect("produts.html");
                }
                else
                {
                    return BadRequest(result.Errors.ToList()[0]);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [System.Web.Http.Route("api/account/GetUserInfo/{username}/{password}")]

        public async Task<IHttpActionResult> GetUserInfo(string username, string password)
        {
            UserStore<IdentityUser> store =
                      new UserStore<IdentityUser>(new PixelContext());

            UserManager<IdentityUser> manager =
                new UserManager<IdentityUser>(store);

            IdentityUser user = await manager.FindAsync(username, password);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);


        }
        [HttpGet]
        [System.Web.Http.Route("api/account/FindUserById/{Id}")]

        public async Task<IHttpActionResult> FindUserById(string Id)
        {
            UserStore<IdentityUser> store =
                      new UserStore<IdentityUser>(new PixelContext());

            UserManager<IdentityUser> manager =
                new UserManager<IdentityUser>(store);

            IdentityUser user = await manager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);


        }


    }
}
