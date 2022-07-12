using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PrelimnaryNoticeController : ApiController
    {
        // Narayan -  get all prelimnary notice by contract id
        public HttpResponseMessage Get(int? programId = null)
        {
            if (programId != null)
            {
                List<PrelimnaryNotice> prelimnaryNotices = PrelimnaryNotice.GetPrelimnaryNoticeList((int)programId);
                var jsonNew = new
                {
                    result = "success",
                    data = prelimnaryNotices
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] PrelimnaryNotice prelimnaryNotice)
        {
            if (prelimnaryNotice.Operation == 1)
            {
                PrelimnaryNotice.SaveNotice(prelimnaryNotice);
            }
            else if (prelimnaryNotice.Operation == 2)
            {
                PrelimnaryNotice.UpdateNotice(prelimnaryNotice);
            }
            List<PrelimnaryNotice> prelimnaryNoticesList = PrelimnaryNotice.GetPrelimnaryNoticeList(prelimnaryNotice.ProgramID);
            var jsonNew = new
            {
                result = "success",
                data = prelimnaryNoticesList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}