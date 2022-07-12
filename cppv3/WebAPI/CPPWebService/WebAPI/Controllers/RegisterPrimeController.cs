using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterPrimeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Prime> prime_list)
        {

            String status = "";
            foreach (var prime in prime_list)
            {

                if (prime.Operation == 1)
                    status += WebAPI.Models.Prime.registerPrime(prime);

                if (prime.Operation == 2)
                    status += WebAPI.Models.Prime.updatePrime(prime);

                if (prime.Operation == 3)
                    status += WebAPI.Models.Prime.deletePrime(prime);

                //4 Do nothing
                if (prime.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
