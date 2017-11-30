using RestServerApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
