using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Business;
using YoungGuns.DataAccess;
using YoungGuns.Shared;
using YoungGuns.WebApi.Map;

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/taxsystem")]
    public class TaxSystemController : ApiController
    {
        private readonly IMapper _map;
        private readonly DbHelper _dbHelper;

        public TaxSystemController()
        {
            _map = AutomapperConfig.Create();
            _dbHelper = new DbHelper();
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var taxSystems = _dbHelper.GetAllTaxSystems();

            return Ok(taxSystems);
        }

        [HttpGet]
        [Route("api/taxsystem/single")]
        public IHttpActionResult Get([FromUri]GetTaxSystemRequest request)
        {
            var taxSystem = _dbHelper.GetTaxSystem(request.Id);
            return Ok(taxSystem);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]PostTaxSystemRequest request)
        {
            var taxSystem = _map.Map<TaxSystem>(request);

            var id = await _dbHelper.UpsertTaxSystem(taxSystem);

            // save adjacency lists to storage
            Dictionary<uint, List<uint>> topoInput = await AdjacencyListBuilder.ExtractAndStoreAdjacencyLists(request);
            await TopoListBuilder.BuildAndStoreTopoList(id, topoInput);

            return Ok(id);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, [FromBody]PostTaxSystemRequest request)
        {
            var taxSystem = _map.Map<TaxSystem>(request);
            taxSystem.Id = id;
            await _dbHelper.UpsertTaxSystem(taxSystem);

            await DAGUtilities.DeleteAdjacencyListTable(taxSystem.Name);
            await AdjacencyListBuilder.ExtractAndStoreAdjacencyLists(request);

            return Ok();
        }
        
    }
}