using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	public class RegisterMilestoneController : ApiController
	{
		public HttpResponseMessage Post([FromBody] List<Milestone> milestone_list)
		{

			String status = "";
			foreach (var milestone in milestone_list)
			{

				if (milestone.Operation == 1)
					status += WebAPI.Models.Milestone.registerMilestone(milestone);

				if (milestone.Operation == 2)
					status += WebAPI.Models.Milestone.updateMilestone(milestone);

				if (milestone.Operation == 3)
					status += WebAPI.Models.Milestone.deleteMilestone(milestone);

				//4 Do nothing
				if (milestone.Operation == 4)
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