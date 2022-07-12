using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;
using WebAPI.Models.StoredProcedure;

namespace WebAPI.Controllers
{
    public class RegisterPurchaseOrderDetailController : ApiController
    {
        public HttpResponseMessage Post([FromBody] JObject objects)
        {
            //PurchaseOrder po = (PurchaseOrder) objects.ElementAt(1);
            //List<PurchaseOrderDetailSP> poDetails = (List<PurchaseOrderDetailSP>)objects.ElementAt(2);
            String poNumber = "";
            try
            {
                var jobject = objects.Children();

                var first = jobject.FirstOrDefault();

                var purchaseOrderStr = first.FirstOrDefault().ToString();
                var purchaseOrder = JsonConvert.DeserializeObject<PurchaseOrder>(purchaseOrderStr);
                PurchaseOrder po = new PurchaseOrder();
                if (purchaseOrder.operation == "update")
                {
                    po = PurchaseOrder.UpdatePurchaseOrder(purchaseOrder);
                    poNumber = po.PurchaseOrderNumber;
                }
                else
                {
                    po = PurchaseOrder.createNewPurchaseOrder(purchaseOrder);
                    poNumber = po.PurchaseOrderNumber;
                }

                var second = jobject.LastOrDefault();
                var purchaseOrderDetailStr = second.LastOrDefault().ToString();
                var purchaseOrderDetail = JsonConvert.DeserializeObject<List<PurchaseOrderDetailSP>>(purchaseOrderDetailStr);

                PurchaseOrderDetail.savePurchaseOrderDetails(purchaseOrderDetail, po);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }







            var jsonNew = new
            {
                result = poNumber
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }



    }
}
