using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoungGuns.Shared;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FieldController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "value";
        }
        
        [HttpPost]
        public void Post([FromBody]string form_id, [FromBody]string form_name, [FromBody]List<FieldDto> fields)
        {

        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
