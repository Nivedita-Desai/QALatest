using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestContractController : ApiController
	{
		public HttpResponseMessage Get()
		{

			List<Contract> contractList = new List<Contract>();
			contractList = WebAPI.Models.Contract.getContract();


			var jsonNew = new
			{
				result = contractList
			};
			return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
		}
	}
}