using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungGuns.Shared;
using YoungGuns.DataAccess;

namespace YoungGuns.Business
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
        public Dictionary<uint, List<uint>> AdjacencyList { get; set; }

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

            FieldValues = new Dictionary<uint, float>();

            //Load calc adjacency list from table storage
            AdjacencyList = DbHelper.GetCalcAdjacencyList(taxSystem.Name).GetAwaiter().GetResult();

            //Load field formulas from TaxSystem
            FieldFormulas = new Dictionary<uint, string>();
            foreach (var field in taxSystem.Fields.ToList())
            {
                FieldFormulas[field.field_id] = field.field_calculation;
            }
        }

        public async void ProcessChangeset(CalcChangeset changeset)
        {
            SortedList<int, uint> fieldsToUpdate = new SortedList<int, uint>();  // key: topo index, value: fieldId

            Parallel.ForEach(changeset.NewValues.Keys, async (fieldId) =>
            {
                // 1. update field values from the changeset itself
                FieldValues[fieldId] = changeset.NewValues[fieldId];

                // 2. traverse graph up from each changeset field
                //    to create a list of fields to update
                List<uint> depList = await DbHelper.GetDependentFields(TaxSystem.Name, fieldId);

                // 3. merge this dictionary into the master dictionary for this changeset
                foreach (uint id in depList)
                    fieldsToUpdate.Add(TopoList.FindIndex((k) => k == id), id);
            });

            // 4. Calc the necessary fields in order
            foreach (var depFieldId in fieldsToUpdate.Values)
                await CalcSingleField(depFieldId);
        }

        private async Task CalcSingleField(uint fieldId)
        {
            Expression formula = new Expression(FieldFormulas[fieldId]);

            // set params from dependent fields
            foreach (uint paramId in AdjacencyList[fieldId])
                formula.Parameters.Add(paramId.ToString(), FieldValues[paramId]);

            // update calced value in the dictionary
            FieldValues[fieldId] = (float)formula.Evaluate();
        }
    }       
}