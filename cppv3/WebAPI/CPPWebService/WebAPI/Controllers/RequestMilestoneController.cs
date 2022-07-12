using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	public class RequestMilestoneController : ApiController
	{
		public HttpResponseMessage Get()
		{

			List<Milestone> milestoneList = new List<Milestone>();
			milestoneList = WebAPI.Models.Milestone.getMilestone();


			var jsonNew = new
			{
				result = milestoneList
			};
			return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
		}
	}
}