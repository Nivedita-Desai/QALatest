using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestInventoryController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Inventory> InventoryList = new List<Inventory>();
            InventoryList = WebAPI.Models.Inventory.getInventory();


            var jsonNew = new
            {
                result = InventoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
