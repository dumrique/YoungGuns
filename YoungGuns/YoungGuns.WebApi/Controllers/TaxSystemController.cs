using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.DataAccess;
using YoungGuns.Shared;
using YoungGuns.WebApi.Map;

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/taxsystem")]
    public class TaxSystemController : ApiController
    {
        private readonly IMapper _map;

        public TaxSystemController()
        {
            _map = AutomapperConfig.Create();
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var list = new List<TaxSystem>
            {
                new TaxSystem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Tax form 1040"
                },
                new TaxSystem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Tax form Corp"
                }
            };

            return Ok(list);
        }


        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]PostTaxSystemRequest request)
        {
            var taxSystem = _map.Map<TaxSystem>(request);

            var dbHelper = new DbHelper();
            var id = await dbHelper.InsertTaxSystem(taxSystem);
            
            return Ok(id);
        }

        [HttpPut]
        public void Put(string id, [FromBody]PostTaxSystemRequest taxSystem)
        {
            // save tax system to DB
            //SaveTaxSystem(id, taxSystem);
        }
        
    }
}