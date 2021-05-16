using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIBasic.Controllers
{
    // Cannot have multiple routre prefix attribute for one class
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        // GET: api/Products
        // GetAllProducts name still works because it contain the Get Http verb in the name
        /* 
         * With attribute routing, dependency on class name is gone and name without Http Verb
         * like ReturnAllProducts also works.
         */
        [HttpGet, Route("")] // Default route
        // We can have multiple routes defined for same method
        [Route("~/prods")] // With ` sign, we override the route prefix and now the url with this /prod
        public IEnumerable<string> ReturnAllProducts()
        {
            return new string[] { "product1", "product2" };
        }

        // GET: api/Products/5
        /* We can put single or multiple constraint in the route defination itself,
         * but if only put the constraint in one place for one Http Verb and not other
         * then, parameter matching might not work correctly, and we will get a 
         * 405 error that the Http verb(eg. Get) doesn't exist, but ideally, we want to
         * have 404 Not found error, which doesn't reveal much information to the attacker
         * 
         * We can also give a name to this route(along with constraints), and get refer to this route config
         * by its name rather than writing the whole config(url pattern, constraints, etc) again eg
         * in self referencing link
        */
        [HttpGet, Route("{id:int:range(1000,3000)}", Name="GetProductById")]
        public string Get(int id)
        {
            return "product";
        }

        // GET: api/Products/5/orders/custid
        /* 
        * With attribute routing, we can define complex routes easily
        */
        // Copying route id constraint in all places if it has been specified in atleast one place
        [HttpGet, Route("{id:int:range(1000,3000)}/orders/{custid}")]
        public string Get(int id, string custid)
        {
            return "product-orders-" + custid;
        }

        // POST: api/Products
        /* CreateProduct name works without having http verb in the name
         * 
         */
        [HttpPost, Route("")]
        // values comes from the body and not the url as specified in atrribute [FromBody]
        public void CreateProduct([FromBody]string value)
        {
        }

        [HttpPost, Route("{prodId:int:range(1000,3000)}")]
        // values comes from the body and not the url as specified in atrribute [FromBody]
        public HttpResponseMessage CreateProduct([FromUri] int prodId)
        {
            // some logic here

            var reponse = Request.CreateResponse(HttpStatusCode.Created);

            // Create a self-referencing link to the new item and set the response Location Header
            // The generated url is from server's perspective(eg.https://10.22.0.57:9778/product/5)
            // and not from the client perspective(eg. same site on client https://www.myservice.com/product/5)
            string uri = Url.Link("GetProductById", new { id = prodId });
            reponse.Headers.Location = new Uri(uri);
            return reponse;
        }

        // PUT: api/Products/5
        // Copying route id constraint in all places if it has been specified in atleast one place
        [HttpPut, Route("{id:int:range(1000,3000)}")]
        // values comes from the body and not the url as specified in atrribute [FromBody]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        // Copying route id constraint in all places if it has been specified in atleast one place
        [HttpDelete, Route("{id:int:range(1000,3000)}")]
        public void Delete(int id)
        {
        }
    }
}
