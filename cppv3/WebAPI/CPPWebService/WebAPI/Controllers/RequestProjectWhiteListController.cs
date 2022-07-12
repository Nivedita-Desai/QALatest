using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
	public class RequestProjectWhiteListController : ApiController
	{
		public HttpResponseMessage Get()
		{

			List<ProjectWhiteList> projectWhiteList = new List<ProjectWhiteList>();
			projectWhiteList = WebAPI.Models.ProjectWhiteList.getProjectWhiteList();


			var jsonNew = new
			{
				result = projectWhiteList
			};
			return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
		}
	}
}
