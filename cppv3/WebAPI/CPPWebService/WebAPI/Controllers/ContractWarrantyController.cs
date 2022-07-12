using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ContractWarrantyController : ApiController
    {
        // Narayan -  get all Warranty by contract id
        public HttpResponseMessage Get(int? programId = null)
        {
            if (programId != null)
            {
                List<ProgramWarranty> programWarrantiesList = ProgramWarranty.GetProgramWarrantyList((int)programId);
                var jsonNew = new
                {
                    result = "success",
                    data = programWarrantiesList
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] ProgramWarranty programWarranty)
        {
            if (programWarranty.Operation == 1)
            {
                ProgramWarranty.registerProgramWarranty(programWarranty);
            }
            else if(programWarranty.Operation == 2)
            {
                ProgramWarranty.updateProgramWarranty(programWarranty);
            }
            List<ProgramWarranty> programWarrantiesList = ProgramWarranty.GetProgramWarrantyList(programWarranty.ProgramID);
            var jsonNew = new
            {
                result = "success",
                data = programWarrantiesList
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