using System;
using System.Web.Http;
using YoungGuns.Shared;

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/taxsystem")]
    public class TaxSystemController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]TaxSystemDto taxSystem)
        {
            var tsId = Guid.NewGuid().ToString();
            

            
            // save tax system to DB
            //CreateTaxSystem(tsId, taxSystem);

            string response = "Received fields for form " + taxSystem.taxsystem_name + " " + taxSystem.taxsystem_id + ". Fields are ";
            foreach (FieldDto field in taxSystem.taxsystem_fields)
            {
                response += field.field_title + " ";
            }
            return Ok(tsId);
        }

        [HttpPut]
        public void Put(string id, [FromBody]TaxSystemDto taxSystem)
        {
            // save tax system to DB
            //SaveTaxSystem(id, taxSystem);
        }
        
    }
}