using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ContractInsuranceController : ApiController
    {
        
        // GET api/<controller>/5
        public HttpResponseMessage Get(int? programId = null)
        {
            if (programId != null)
            {
                List<ContractInsurance> contractInsurances = ContractInsurance.GetContractInsurances((int)programId);
                var jsonNew = new
                {
                    result = "success",
                    data = contractInsurances
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] ContractInsurance contractInsurance)
        {
            if (contractInsurance.Operation == 1)
            {
                ContractInsurance.SaveInsurance(contractInsurance);
            }
            else if (contractInsurance.Operation == 2)
            {
                ContractInsurance.UpdateInsurance(contractInsurance);
            }
            List<ContractInsurance> contractInsurances = ContractInsurance.GetContractInsurances(contractInsurance.ProgramID);
            var jsonNew = new
            {
                result = "success",
                data = contractInsurances
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}