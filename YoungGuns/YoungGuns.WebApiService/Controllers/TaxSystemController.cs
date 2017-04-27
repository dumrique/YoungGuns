using AutoMapper;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Business;
using YoungGuns.Data;
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

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]PostTaxSystemRequest request)
        {
            var taxSystem = _map.Map<TaxSystem>(request);

            var id = await _dbHelper.UpsertTaxSystem(taxSystem);
            await AdjacencyListBuilder.ExtractAndStoreAdjacencyList(request);
            
            return Ok(id);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, [FromBody]PostTaxSystemRequest request)
        {
            var taxSystem = _map.Map<TaxSystem>(request);
            taxSystem.Id = id;
            await _dbHelper.UpsertTaxSystem(taxSystem);

            await DAGUtilities.DeleteAdjacencyListTable(taxSystem.Name);
            await AdjacencyListBuilder.ExtractAndStoreAdjacencyList(request);

            return Ok();
        }
        
    }
}