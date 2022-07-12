using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    public class RegisterWrapController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Wrap> wrap_list)
        {

            String status = "";
            foreach (var wrap in wrap_list)
            {

                if (wrap.Operation == 1)
                    status += WebAPI.Models.Wrap.registerWrap(wrap);

                if (wrap.Operation == 2)
                    status += WebAPI.Models.Wrap.updateWrap(wrap);

                if (wrap.Operation == 3)
                    status += WebAPI.Models.Wrap.deleteWrap(wrap);

                //4 Do nothing
                if (wrap.Operation == 4)
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
