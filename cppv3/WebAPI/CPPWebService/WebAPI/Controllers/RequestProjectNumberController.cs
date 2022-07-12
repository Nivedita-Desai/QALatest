using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProjectNumberController: ApiController
    {
        public HttpResponseMessage Get(string OrganizationID)
        {

           String ProjectID = "";
           String userName = RequestContext.Principal.Identity.Name;
           using(var ctx = new CPPDbContext())
            {
                String number = ctx.ProjectNumber.Where(x=> x.OrganizationID == OrganizationID).Max(a => a.projectNumber);
                if(number != null)
                {
                    number = (Convert.ToInt16(number) + 1).ToString();
                    //Check if the numner already assigned to the project
                    Project project = null;
                    Boolean isExist = true;
                    while(isExist)
                    {
                        project =  ctx.Project.Where(a => a.ProjectNumber == number).FirstOrDefault();
                        if (project == null)
                            isExist = false;
                        else
                        {
                            ProjectNumber projectNumber = new ProjectNumber() //Create
                            {
                                projectNumber = number,
                                isUSed = true,
                                CreatedBy = userName,
                                UpdatedBy = userName,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                OrganizationID = OrganizationID
                            };
                            ctx.ProjectNumber.Add(projectNumber);
                            ctx.SaveChanges();

                            number = (int.Parse(number) + 1).ToString();
                        }
                        //
                    }
                  

                   
                    if(number.Length < 4)
                    {
                        int diff = 4 - number.Length;
                        for(int i = 0; i< diff; i++)
                        {
                            number = "0" + number;
                        }
                    }
                }else
                {
                    number = "0001";
                }
                ProjectID = number;

            }


            var jsonNew = new
            {
                result = ProjectID
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}