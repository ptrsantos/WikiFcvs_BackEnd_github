using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WikiFCVS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string ambiente;
        private readonly string connectionString;
        private readonly string IdGoogle;
        private readonly string IdFacebook;
        private readonly string SecretFacebook;
        private readonly string Emissor;

        public ValuesController(IConfiguration configuration)
        {
            ambiente = configuration.GetValue<string>("Parametros:Local");
            connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            IdGoogle = configuration.GetValue<string>("Parametros:IdGoogle");
            IdFacebook = configuration.GetValue<string>("Parametros:IdFacebook");
            SecretFacebook = configuration.GetValue<string>("Parametros:SecretFacebook");
            Emissor = configuration.GetValue<string>("AppSettings:Emissor");
        }

        //GET api/values
       [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //return new string[] { "value1", "value2" };
            return new string[] { ambiente, connectionString, IdGoogle, IdFacebook, SecretFacebook, Emissor};
        }

        // GET api/values
        //[HttpGet]
        //public string Get([FromServices] IConfiguration configuration)
        //{
        //    return Content(configuration["Parametros:Local"]).ToString();
        //}

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
