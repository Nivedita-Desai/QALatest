using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterContractController : ApiController
	{
		public HttpResponseMessage Post([FromBody] List<Contract> contract_list)
		{

			String status = "";
			foreach (var contract in contract_list)
			{

				if (contract.Operation == 1)
					status += WebAPI.Models.Contract.registerContract(contract);

				if (contract.Operation == 2)
					status += WebAPI.Models.Contract.updateContract(contract);

				if (contract.Operation == 3)
					status += WebAPI.Models.Contract.deleteContract(contract);

				//4 Do nothing
				if (contract.Operation == 4)
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