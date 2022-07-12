using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProgramContractController : ApiController
	{
		public HttpResponseMessage Get()
		{

			List<ProgramContract> programContractList = new List<ProgramContract>();
			programContractList = WebAPI.Models.ProgramContract.getProgramContract();


			var jsonNew = new
			{
				result = programContractList
			};
			return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
		}

	}
}