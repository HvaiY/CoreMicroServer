using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aoptest1;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Mvc;

namespace hystrixTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Person p;
        public ValuesController(Person p)
        {
            this.p = p;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            /*
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                //Person p = new Person();
                Person p = proxyGenerator.CreateClassProxy<Person>();
                await p.HelloAsync("rupeng.com");
            }*/
            await p.HelloAsync("rupeng.com");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
