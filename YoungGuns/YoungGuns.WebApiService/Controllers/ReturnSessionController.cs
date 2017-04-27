using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Business;
using YoungGuns.DataAccess;
using YoungGuns.Shared;


namespace YoungGuns.WebApiService.Controllers
{
    [Route("api/returnsession")]
    public class ReturnSessionController : ApiController
    {
        private readonly TaxSystem _taxSystem;
        private readonly CalcDAG _dag;
        private readonly DbHelper _dbHelper;
        private readonly FabricClient _fabricClient;
        public ReturnSessionController()
        {
            _dbHelper = new DbHelper();
            _fabricClient = new FabricClient();
        }

        [HttpGet]
        [Route("api/returnsession")]
        public async Task<IHttpActionResult> Get()
        {
            string sessionGuid = Guid.NewGuid().ToString();

            // spin up a CalcService
            await _fabricClient.ServiceManager.CreateServiceAsync(
                new StatefulServiceDescription
                {
                    ApplicationName = new Uri("fabric:/YoungGunsApp"),
                    ServiceName = new Uri($"fabric:/YoungGunsApp/CalcService_{sessionGuid}"),
                    ServiceTypeName = "CalcServiceType",
                    PartitionSchemeDescription = new SingletonPartitionSchemeDescription(),
                    MinReplicaSetSize = 2,
                    TargetReplicaSetSize = 2,
                    HasPersistedState = true,
                    DefaultMoveCost = MoveCost.High
                },
                TimeSpan.FromSeconds(20),
                CancellationToken.None);

            return Ok(sessionGuid);
        }


        /// <summary>
        /// Post to start a return session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]PostReturnSessionRequest session)
        {
            // get reference to the right CalcService
            var calcService = ServiceProxy.Create<ICalcService>(new Uri($"fabric:/YoungGunsApp/CalcService_{session.SessionGuid}"));
            // set TaxSystem on the CalcService
            await calcService.LoadTaxSystem(session.TaxSystemId);

            return Ok(session);
        }
    }
}
