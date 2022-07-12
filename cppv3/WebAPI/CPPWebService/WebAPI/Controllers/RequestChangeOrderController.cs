using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	public class RequestChangeOrderController : ApiController
	{
		public HttpResponseMessage Get()
		{

			List<ChangeOrder> changeOrderList = new List<ChangeOrder>();
			changeOrderList = WebAPI.Models.ChangeOrder.getChangeOrder();


			var jsonNew = new
			{
				result = changeOrderList
			};
			return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
		}
	}
}