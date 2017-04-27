using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using YoungGuns.Data;
using YoungGuns.Shared;
using YoungGuns.DataAccess;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace YoungGuns.CalcService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class CalcService : StatefulService, ICalcService
    {
        private CalcDAG _dag;
        private DbHelper _dbHelper;

        public TaxSystem TaxSystem { get; set; }

        public CalcService(StatefulServiceContext context)
            : base(context)
        {
            _dbHelper = new DbHelper();
        }

        public async Task LoadTaxSystem(string taxSystemId)
        {    
            // load tax system    
            this.TaxSystem = _dbHelper.GetTaxSystem(taxSystemId);
            
            // reinitialize DAG (loads field formulas too)
            _dag = new CalcDAG(this.TaxSystem);

            // load AdjacencyLists
            _dag.AdjacencyList = await DbHelper.GetCalcAdjacencyList(TaxSystem.Name);

            // load TopoList for this tax system
            _dag.TopoList = _dbHelper.GetTopoList(TaxSystem.Id);
        }

        public void Calculate(CalcChangeset changeset)
        {
            if (_dag == null)
            {
                // TODO Load DAG
                //_dag.
            }

            _dag.ProcessChangeset(changeset);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[] { new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context)) };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
