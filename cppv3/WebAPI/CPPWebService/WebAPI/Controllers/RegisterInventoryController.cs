using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterInventoryController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Inventory> InventoryList)
        {

            String status = "";
            foreach (var Inventory in InventoryList)
            {

                if (Inventory.Operation == 1)
                    status += WebAPI.Models.Inventory.registerInventory(Inventory);

                if (Inventory.Operation == 2)
                    status += WebAPI.Models.Inventory.updateInventory(Inventory);

                if (Inventory.Operation == 3)
                    status += WebAPI.Models.Inventory.deleteInventory(Inventory);

                //4 Do nothing
                if (Inventory.Operation == 4)
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
