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
    public class RequestProjectElementNumberController : ApiController
    {
        public HttpResponseMessage Get(String ProjectNumber, string OrganizationID)
        {

            String ProjectID = "";
            String userName = RequestContext.Principal.Identity.Name;
            using (var ctx = new CPPDbContext())
            {
                String number = ctx.ProjectNumber.Where(b => b.projectNumber == ProjectNumber && b.OrganizationID == OrganizationID).Max(a => a.projectElementNumber);
                if (number != null)
                {
                    number = (Convert.ToInt16(number) + 1).ToString();
                    //Check if the numner already assigned to the project
                    Project project = null;
                    Boolean isExist = true;
                    while (isExist)
                    {
                        
                        if (number.Length < 2)
                        {
                            int diff = 2 - number.Length;
                            for (int i = 0; i < diff; i++)
                            {
                                number = "0" + number;
                            }
                        }
                        project = ctx.Project.Where(a => a.ProjectElementNumber == number && a.ProjectNumber == ProjectNumber).FirstOrDefault();
                        if (project == null)
                            isExist = false;
                        else
                        {
                            ProjectNumber projectNumber = new ProjectNumber() //Create
                            {
                                projectElementNumber = number,
                                projectNumber = ProjectNumber,
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

                    
                }
                else
                {
                    number = "01";
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