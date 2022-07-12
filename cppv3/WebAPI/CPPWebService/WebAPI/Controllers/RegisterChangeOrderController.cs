using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	public class RegisterChangeOrderController : ApiController
	{
		public HttpResponseMessage Post([FromBody] List<ChangeOrder> changeOrder_list)
		{

			String status = "";
			foreach (var changeOrder in changeOrder_list)
			{

				if (changeOrder.Operation == 1)
					status += WebAPI.Models.ChangeOrder.registerChangeOrder(changeOrder);

				if (changeOrder.Operation == 2)
					status += WebAPI.Models.ChangeOrder.updateChangeOrder(changeOrder);

				if (changeOrder.Operation == 3)
					status += WebAPI.Models.ChangeOrder.deleteChangeOrder(changeOrder);

				//4 Do nothing
				if (changeOrder.Operation == 4)
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