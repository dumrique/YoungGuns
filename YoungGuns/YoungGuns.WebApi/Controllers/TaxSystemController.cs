using System;
using System.Collections.Generic;
using System.Web.Http;
using YoungGuns.Shared;

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/taxsystem")]
    public class TaxSystemController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Get()
        {
            var list = new List<TaxSystem>();
            var taxSystem = new TaxSystem
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Tax form 1040"
            };
            list.Add(taxSystem);
            taxSystem = new TaxSystem
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Tax form Corp"
            };
            list.Add(taxSystem);
            return Ok(list);
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