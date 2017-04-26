﻿using NCalc;
using YoungGuns.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Data
{
    public class CalcDAG
    {
        /// <summary>
        /// Adjacency list defining field dependencies
        /// </summary> 
        public Dictionary<uint,List<uint>> AdjacencyList { get; set; }

        /// <summary>
        /// Dictionary of calc field values, 
        ///     sorted in topological sort order by calc dependency
        /// </summary>
        public SortedDictionary<uint, float> FieldValues { get; set; }

        /// <summary>
        /// Dictionary of calc field formulas, 
        ///     sorted in topological sort order to match FieldValues
        /// </summary>
        public SortedDictionary<uint, string> FieldFormulas { get; set; }

        /// <summary>
        /// Ordered list of field IDs in a topologically sorted order,
        ///     which guarantees calc dependency order is maintained.
        /// </summary>
        public List<uint> TopoList { get; set; }


        public CalcDAG(uint size)
        {
            AdjacencyList = new Dictionary<uint, List<uint>>();
            FieldValues = new SortedDictionary<uint, float>();
            FieldFormulas = new SortedDictionary<uint, string>();
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
                List<uint> depList = await GetDependentFields(fieldId);

                // 3. merge this dictionary into the master dictionary for this changeset
                foreach (uint id in depList)
                    fieldsToUpdate.Add(FieldValues.ToList().FindIndex((kvp) => kvp.Key==id), id);      //TODO: find better way than FieldValues.ToList()
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

        private async Task<List<uint>> GetDependentFields(uint key)
        {
            // TODO: save this list for each field in Azure Table Storage
            //       and pull from there by this key
            return null;



            //List<uint> list = new List<uint>();

            //GetDependentFieldsWorker(key, list);    // TODO: find a non-recursive way to do this

            //return list.Distinct().ToList();

        }
        //private void GetDependentFieldsWorker(uint key, List<uint> fields)
        //{
        //    fields.AddRange(AdjacencyList[key]);
        //    foreach (uint k in AdjacencyList[key])
        //        GetDependentFieldsWorker(k, fields);
        //}
    }
}