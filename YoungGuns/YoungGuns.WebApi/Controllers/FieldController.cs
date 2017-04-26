using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Shared;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/field")]
    public class FieldController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]TaxSystemDto form)
        {
            string response = "Received fields for form " + form.taxsystem_name + " " + form.taxsystem_id + ". Fields are ";
            foreach (FieldDto field in form.taxsystem_fields)
            {
                response += field.field_title + " ";
            }



            return Ok(response);
        }
    }
}
