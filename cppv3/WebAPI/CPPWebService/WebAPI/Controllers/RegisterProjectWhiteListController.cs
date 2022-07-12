using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	public class RegisterProjectWhiteListController : ApiController
	{
		public HttpResponseMessage Post([FromBody] List<ProjectWhiteList> project_white_list_list)
		{

			String status = "";

            //If operation is 1, then delete and add
            //If operation is 2, then delete only
            if (project_white_list_list.Count > 0)
            {
                if (project_white_list_list[0].Operation == 1 || project_white_list_list[0].Operation == 2)
                {
                    status += WebAPI.Models.ProjectWhiteList.deleteProjectWhiteList(project_white_list_list[0].ProjectID);
                }

                if (project_white_list_list[0].Operation == 1)
                {
                    for (int x = 0; x < project_white_list_list.Count; x++)
                    {
                        status += WebAPI.Models.ProjectWhiteList.registerProjectWhiteList(project_white_list_list[x]);
                    }
                }
            }
			var jsonNew = new
			{
				result = status
			};
			return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
		}
	}
}
