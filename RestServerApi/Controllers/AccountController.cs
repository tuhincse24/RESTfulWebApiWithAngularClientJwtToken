using RestServerApi.Dtos;
using RestServerApi.InMemoryDataStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestServerApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(UserDto userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await Task.Run(() => UserStore.AddUser(userModel.Name, userModel.Email, userModel.Password));            ;

            return Ok();
        }
    }
}
