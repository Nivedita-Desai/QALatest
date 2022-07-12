using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterMaterialCategoryController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<MaterialCategory> material_category_list)
        {

            String status = "";
            foreach (var material_category in material_category_list)
            {

                if (material_category.Operation == 1)
                    status += WebAPI.Models.MaterialCategory.registerMaterialCategory(material_category);

                if (material_category.Operation == 2)
                    status += WebAPI.Models.MaterialCategory.updateMaterialCategory(material_category);

                if (material_category.Operation == 3)
                    status += WebAPI.Models.MaterialCategory.deleteMaterialCategory(material_category);

                //4 Do nothing
                if (material_category.Operation == 4)
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
