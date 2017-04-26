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
        public IHttpActionResult Get()
        {
            return Ok("Hello World");
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]TaxSystemDto form)
        {
            string response = "Received fields for form " + form.form_name + " " + form.form_id + ". Fields are ";
            foreach (FieldDto field in form.form_fields)
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
