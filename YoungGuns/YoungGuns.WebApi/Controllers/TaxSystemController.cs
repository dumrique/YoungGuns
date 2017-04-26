using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using YoungGuns.Business;
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
        public async Task<IHttpActionResult> Post([FromBody]TaxSystemDto taxSystem)
        {
            var tsId = Guid.NewGuid().ToString();

            // save tax system to storage
            //CreateTaxSystem(tsId, taxSystem);

            // save adjacency lists to storage
            await AdjacencyListBuilder.ExtractAndStoreAdjacencyList(taxSystem);

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