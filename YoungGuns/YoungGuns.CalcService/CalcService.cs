using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using YoungGuns.Shared;
using YoungGuns.DataAccess;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using YoungGuns.Business;

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

        public void LoadTaxSystem(string taxSystemId)
        {    
            // load tax system    
            this.TaxSystem = _dbHelper.GetTaxSystem(taxSystemId);
            
            // reinitialize DAG (loads field formulas too)
            _dag = new CalcDAG(this.TaxSystem);
        }

        /// <summary>
        /// Returns     null for good calc, 
        ///             true for merge conflict,
        ///             false for auto-merge and good calc.
        /// </summary>
        /// <param name="changeset"></param>
        /// <returns></returns>
        public async Task<CalcResult> Calculate(CalcChangeset changeset)
        {
            bool? retVal = null;
            if (_dag == null)
            {
                // TODO: loop and wait till we DO have a _dag?
                //       or perhaps just throw and exception
            }

            // TODO: get current version of most recent ReturnSnapshot.
            //       if changeset.BaseVersion is different, 
            //       run DAGUtilies.CheckForMergeConflicts.
            if (_dag.ReturnVersion != changeset.BaseVersion)
            {
                // retrieve the last changeset from DocumentDb
                var lastChangesetFields = _dbHelper.GetReturnChangesetFields(changeset.ReturnId, _dag.ReturnVersion);

                retVal = await DAGUtilities.CheckForMergeConflicts(TaxSystem.Name, lastChangesetFields, changeset.NewValues.Keys.ToList());
                if (retVal == true)
                    return new CalcResult() { MergeResult = true };
            }

            // otherwise, process the changeset
            return new CalcResult()
            {
                MergeResult = retVal,
                ReturnSnapshot = await _dag.ProcessChangeset(changeset)
            };
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
