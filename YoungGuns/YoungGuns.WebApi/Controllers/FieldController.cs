using System.Web.Http;
using YoungGuns.Shared;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/field")]
    public class FieldController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]PostTaxSystemRequest form)
        {
            return Ok();
        }
    }
}
