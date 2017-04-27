using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Business;
using YoungGuns.DataAccess;
using YoungGuns.Shared;
using Microsoft.ServiceFabric.Services;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using YoungGuns.CalcService;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace YoungGuns.WebApi.Controllers
{
    [Route("api/field")]
    public class FieldController : ApiController
    {
        private readonly TaxSystem _taxSystem;
        private readonly CalcDAG _dag;
        private readonly DbHelper _dbHelper;

        public FieldController()
        {
            _dbHelper = new DbHelper();
            //_dag = new CalcDAG(_dbHelper.GetTaxSystem(taxSystemId));
        }

        /// <summary>
        /// Post data into a field and calc
        /// </summary>
        /// <param name="fieldRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]PostFieldRequest fieldRequest)
        {
            CalcChangeset changeset = new CalcChangeset()   // TODO Add BaseVersion, Owner, ReturnId
            {
                NewValues = new Dictionary<uint, float>()
                {
                    { fieldRequest.field_id, fieldRequest.field_value }
                }
            };
            // TODO Look through each CalcService, checking the TaxSystem that is loaded into the CalcService
            // If CalcService.GetLoadedTaxSystem == null || CalcService.GetLoadedTaxSystem == fieldRequest.taxsystem_name
            // Then we can calc here, otherwise find another to load the tax system into or reuse

            //ICalcService calcService = ServiceProxy.Create<ICalcService>();
            _dag.ProcessChangeset(changeset);
            return Ok();
        }
    }
}
