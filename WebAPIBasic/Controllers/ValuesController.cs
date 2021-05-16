using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIBasic.Handlers;

namespace WebAPIBasic.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        // GET: values
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            var getByIdUrl = Url.Link("GetById", new { id = 123 });

            //return new string[] { "value1", "value2", Request.GetApiKey() };

            /*
             * Result:
             *  "https://localhost:44371/values/123",
                "https://mycompany.com:1234/",
                "https://mycompany.com:1234/values/123"
            when request is coming from client with following values in header:
             [{"key":"X-Forwarded-Host","value":"mycompany.com:1234",}]
             [{"key":"X-Forwarded-Proto","value":"https",}]
            */

            return new string[] { getByIdUrl,
            Request.GetSelfReferenceBaseUrl().ToString(),
            Request.RebaseUrlForClient(new Uri(getByIdUrl)).ToString() 
            };
        }

        // GET: values/5
        [HttpGet, Route("{id:int}", Name = "GetById")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Values/5
        public void Delete(int id)
        {
        }
    }
}
