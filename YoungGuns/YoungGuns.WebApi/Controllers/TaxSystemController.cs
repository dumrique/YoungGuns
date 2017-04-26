using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YoungGuns.Shared;

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/taxsystem")]
    public class TaxSystemController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Ok(new TaxSystemDto());
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]TaxSystemDto taxSystem)
        {
            var tsId = Guid.NewGuid().ToString();
            
            // save tax system to DB
            //CreateTaxSystem(tsId, taxSystem);
            
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