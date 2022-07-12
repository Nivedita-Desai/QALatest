using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProgramContractController : ApiController
	{
		public HttpResponseMessage Post([FromBody] List<ProgramContract> program_contract_list)
		{

			String status = "";
			foreach (var program_contract in program_contract_list)
			{

				if (program_contract.Operation == 1)
					status += WebAPI.Models.ProgramContract.registerProgramContractList(program_contract);

				if (program_contract.Operation == 2)
					status += WebAPI.Models.ProgramContract.updateProgramContract(program_contract);

				if (program_contract.Operation == 3)
					status += WebAPI.Models.ProgramContract.deleteProgramContract(program_contract);

				//4 Do nothing
				if (program_contract.Operation == 4)
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