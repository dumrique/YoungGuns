using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Shared;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FieldController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string form_id, [FromBody]string form_name, [FromBody]List<FieldDto> form_fields)
        {
            string response = "Received fields for form " + form_name + " " + form_id + ". Fields are ";
            foreach (FieldDto field in form_fields)
            {
                response += field.field_title + " ";
            }
            return Ok(response);
        }

        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
