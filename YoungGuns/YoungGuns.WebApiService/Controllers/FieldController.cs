﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YoungGuns.Business;
using YoungGuns.DataAccess;
using YoungGuns.Shared;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace YoungGuns.WebApiService.Controllers
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
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]PostFieldRequest field)
        {
            CalcChangeset changeset = new CalcChangeset()   // TODO Add baseVersion, Owner, returnId
            {
                newValues = new Dictionary<uint, float>()
                {
                    { field.field_id, field.field_value }
                }
            };

            //_dag.ProcessChangeset(changeset);
            return Ok();
        }
    }
}
