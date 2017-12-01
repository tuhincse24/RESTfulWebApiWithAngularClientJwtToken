using RestServerApi.Entities;
using System.Web.Http;

namespace RestServerApi.Controllers
{
    [RoutePrefix("api/clients")]
    public class ClientsController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Client.Clients);
        }
    }
}
