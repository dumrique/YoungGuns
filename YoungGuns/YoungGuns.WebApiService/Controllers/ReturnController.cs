﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using YoungGuns.DataAccess;
using YoungGuns.Shared;

namespace YoungGuns.WebApiService.Controllers
{
    [Route("api/return")]
    public class ReturnController : ApiController
    {
        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody]CalcChangeset calcChangeset)
        {
            CalcResult result = null;
            var calcService = ServiceProxy.Create<ICalcService>(new Uri($"fabric:/YoungGunsApp/CalcService_{calcChangeset.sessionGuid}"));
            try
            {
                result = await calcService.Calculate(calcChangeset);
            }
            catch (Exception ex)
            {
                return Ok(1);
            }
            return Ok(result);
        }
    }
}
