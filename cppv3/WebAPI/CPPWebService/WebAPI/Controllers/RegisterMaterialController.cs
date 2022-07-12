using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterMaterialController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Material> material_list)
        {

            String status = "";
            foreach (var material in material_list)
            {

                if (material.Operation == 1)
                    status += WebAPI.Models.Material.registerMaterial(material);

                if (material.Operation == 2)
                    status += WebAPI.Models.Material.updateMaterial(material);

                if (material.Operation == 3)
                    status += WebAPI.Models.Material.deleteMaterial(material);

                //4 Do nothing
                if (material.Operation == 4)
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
