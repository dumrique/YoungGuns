using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using YoungGuns.Data.Contracts;
using YoungGuns.Shared;

namespace YoungGuns.Data
{
    public class CalcDAG
    {
        /// <summary>
        /// TaxSystem that corresponds to this DAG
        /// </summary>
        public TaxSystem TaxSystem { get; set; }

        /// <summary>
        /// Adjacency list defining field dependencies
        /// </summary> 
        public Dictionary<uint,List<uint>> AdjacencyList { get; set; }

        /// <summary>
        /// Dictionary of calc field values, 
        ///     sorted in topological sort order by calc dependency
        /// </summary>
        public Dictionary<uint, float> FieldValues { get; set; }

        /// <summary>
        /// Dictionary of calc field formulas, 
        ///     sorted in topological sort order to match FieldValues
        /// </summary>
        public Dictionary<uint, string> FieldFormulas { get; set; }

        /// <summary>
        /// Ordered list of field IDs in a topologically sorted order,
        ///     which guarantees calc dependency order is maintained.
        /// </summary>
        public List<uint> TopoList { get; set; }


        public CalcDAG(TaxSystem taxSystem)
        {
            TaxSystem = taxSystem;
            AdjacencyList = new Dictionary<uint, List<uint>>();
            FieldValues = new Dictionary<uint, float>();
            FieldFormulas = new Dictionary<uint, string>();
        }

        public async void ProcessChangeset(CalcChangeset changeset)
        {
            SortedList<int, uint> fieldsToUpdate = new SortedList<int,uint>();  // key: topo index, value: fieldId

            Parallel.ForEach(changeset.NewValues.Keys, async (fieldId) =>
            {
                // 1. update field values from the changeset itself
                FieldValues[fieldId] = changeset.NewValues[fieldId];

                // 2. traverse graph up from each changeset field
                //    to create a list of fields to update
                List<uint> depList = await GetDependentFields(changeset.ReturnId, changeset.BaseVersion, fieldId);

                // 3. merge this dictionary into the master dictionary for this changeset
                foreach (uint id in depList)
                    fieldsToUpdate.Add(TopoList.FindIndex((k) => k==id), id);
            });

            // 4. Calc the necessary fields in order
            foreach (var depFieldId in fieldsToUpdate.Values)
                await CalcSingleField(depFieldId);
        }

        private async Task CalcSingleField(uint fieldId)
        {
            Expression formula = new Expression(FieldFormulas[fieldId]);

            // set params from dependent fields
            foreach(uint paramId in AdjacencyList[fieldId])
                formula.Parameters.Add(paramId.ToString(), FieldValues[paramId]);

            // update calced value in the dictionary
            FieldValues[fieldId] = (float)formula.Evaluate();
        }

        private async Task<List<uint>> GetDependentFields(string returnId, int version, uint fieldId)
        {
            CloudTable table = await GetCloudTableStorage();
            // Construct the query operation for the field list for the given field
            TableOperation retrieveOperation = TableOperation.Retrieve<AdjacencyListItem>(TaxSystem.Name, fieldId.ToString());

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);
            return ((AdjacencyListItem)retrievedResult.Result)?.DependentFields;
        }

        private async Task<CloudTable> GetCloudTableStorage()
        { 
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=youngguns;AccountKey=NGct+PJexXQ0Eby6DhuOQ555dev7V6Z+lciJyunYM6aXVoEvzQD8Ig2FVv5YGiklTlPLaUENU4Cgg4N2pFzY2A==;EndpointSuffix=core.windows.net");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("TaxSystemAdjacencyLists");

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}