using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    public class RegisterApprovalResendLinkinMatrixController : ApiController
    {
        // GET: RegisterApprovalResendLinkinMatrix
        public HttpResponseMessage Get(int ProjectID, int TrendId, int TrendNumber, String UserID)
        {
            String status = "";
            var ctx = new CPPDbContext();
            int nextApproverEmployeeID = 10000;
            var CheckIfCreatorResendLink = ctx.Trend.Where(x => x.TrendID == TrendId
                                                                    && x.ProjectID == ProjectID
                                                                   // && x.CreatedBy == IfCreatorUserId
                                                                    ).ToList();
            if (CheckIfCreatorResendLink.Count > 0)
            {
                for (int x = 0; x < CheckIfCreatorResendLink.Count; x++)
                {
                    User requestingUser = ctx.User.First(p => p.UserID == UserID);  //Get associated requesting user
                    nextApproverEmployeeID = Convert.ToInt16(CheckIfCreatorResendLink[x].CurrentApprover_EmployeeID);
                    User targetedUser = ctx.User.First(p => p.EmployeeID == nextApproverEmployeeID);  //Get associated targeted user
                    Employee targetedEmployee = ctx.Employee.First(p => p.ID == nextApproverEmployeeID);

                  
                    WebAPI.Services.MailServices.ResendSendApprovalEmail(requestingUser.FirstName + " " + requestingUser.LastName,
                                                                    targetedUser.FirstName + " " + targetedUser.LastName,
                                                                    "Admin", TrendNumber.ToString(), ProjectID.ToString(), targetedUser.Email);
                    status = "Resend of Link for approval is successful. \n" + targetedUser.FirstName + " " + targetedUser.LastName + " will be notified for approval.";
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