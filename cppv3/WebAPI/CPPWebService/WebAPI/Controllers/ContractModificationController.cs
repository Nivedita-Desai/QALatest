using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ContractModificationController : ApiController
    {
        public HttpResponseMessage Post([FromBody] ContractModification contractModification)
        {
            if (contractModification.Operation == 1)
            {
                ContractModification.SaveModificationData(contractModification);
            }
            else if (contractModification.Operation == 2)
            {
                ContractModification.UpdateContractModification(contractModification);
            }
            else if (contractModification.Operation == 3)
            {
                ContractModification.DeleteContractModification(contractModification);
            }
            DateTime ProgramCurrentEndDate=DateTime.Now;
            List<ContractModification> contractModificationList = ContractModification.GetContractModificationList(contractModification.ProgramID);
            //if (contractModification.ModificationType != 1)
            //{
                using (var ctx = new CPPDbContext())
                {
                    Program objProgram = ctx.Program.Where(p => p.ProgramID == contractModification.ProgramID).FirstOrDefault();
                    if (objProgram != null)
                    {
                        ProgramCurrentEndDate = Convert.ToDateTime(objProgram.CurrentEndDate);
                    }
                }
            //}
            var jsonNew = new
            {
                result = "success",
                data = contractModificationList,
                CurrentEndDate= ProgramCurrentEndDate
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
        public HttpResponseMessage Get(int? programId=null,int? modId = null)
        {
            
            if (programId != null)
            {
                List<ContractModification> contractModificationList = ContractModification.GetContractModificationList((int)programId);
                var jsonNew = new
                {
                    result = "success",
                    data = contractModificationList
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            else if (modId != null)
            {
                ContractModification contractModification = ContractModification.GetContractModification((int)modId);
                var jsonNew = new
                {
                    result = "success",
                    data = contractModification
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
