using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterVendorController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Vendor> VendorList)
        {

            String status = "";
            foreach (var Vendor in VendorList)
            {

                if (Vendor.Operation == 1)
                    status += WebAPI.Models.Vendor.registerVendor(Vendor);

                if (Vendor.Operation == 2)
                    status += WebAPI.Models.Vendor.updateVendor(Vendor);

                if (Vendor.Operation == 3)
                    status += WebAPI.Models.Vendor.deleteVendor(Vendor);

                //4 Do nothing
                if (Vendor.Operation == 4)
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
