using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebAPI.Models.StoredProcedure;

namespace WebAPI.Models
{
    [Table("purchase_order_detail")]
    public class PurchaseOrderDetail : Audit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int PurchaseOrderID { get; set; }
        public String ClassRefFullName { get; set; }
        public String TemplateFullName { get; set; }
        public String ItemRefFullName { get; set; }
        public DateTime TxnDate { get; set; }
        public String Memo { get; set; }
        public String Desc { get; set; }
        public Double Quantity { get; set; }
        public String UnitOfMeasurement { get; set; }
        public Double Rate { get; set; }
        public Double Amount { get; set; }
        public String CustomerRefFullName { get; set; }
        public String CostCode { get; set; }

        [ForeignKey("PurchaseOrderID")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        
        public static void savePurchaseOrderDetails(List<PurchaseOrderDetailSP> poDetails, PurchaseOrder po)
        {
            using (var ctx = new CPPDbContext())
            {
                if (po.Status == "Created")
                {

                    if (po.operation == "update")
                    {
                        List<PurchaseOrderDetail> list = ctx.PurchaseOrderDetail.Where(x => x.PurchaseOrderID == po.ID).ToList();
                        ctx.PurchaseOrderDetail.RemoveRange(list);
                    }

                    foreach (var poDetail in poDetails)
                    {
                        double requestedAmountOrQuantity = Convert.ToDouble(poDetail.RequestedAmountOrQuantity);
                        double unitPrice = poDetail.UnitPrice == "" ? 0 : Convert.ToDouble(poDetail.UnitPrice);

                        PurchaseOrderDetail newPo = new PurchaseOrderDetail
                        {
                            PurchaseOrderID = po.ID,
                            ClassRefFullName = poDetail.ClassRefFullName,//TBD
                            TemplateFullName = "CPP Custom Template",
                            TxnDate = DateTime.Now,
                            Memo = "",
                            ItemRefFullName = poDetail.UniqueIdentityNumber,
                            Desc = poDetail.Description,
                            Quantity = poDetail.CostType == "U" ? requestedAmountOrQuantity : 0,
                            UnitOfMeasurement = poDetail.UnitOfMeasurement,
                            Rate = unitPrice,
                            Amount = poDetail.CostType == "U" ? (requestedAmountOrQuantity * unitPrice) : requestedAmountOrQuantity,
                            CustomerRefFullName = poDetail.CustomerRefFullName,//TBD,
                            CostCode = poDetail.CostLineItemID,
                            // EmployeeID = poDetail.EmployeeID,
                            // EmployeeName = poDetail.EmployeeName

                        };

                        ctx.PurchaseOrderDetail.Add(newPo);
                        ctx.SaveChanges();

                        int poDetailsID = newPo.ID;
                        //---------------Aditya 23032022-------------------//
                        List<POODCEmployeeDetails> existingEmployeeDetails = ctx.POODCEmployeeDetails.Where(e => e.PODetailsID == poDetailsID).ToList();

                        ctx.POODCEmployeeDetails.RemoveRange(existingEmployeeDetails);
                        //-------------------------------------------------//
                        if (poDetail.CostType == "ODC")
                        {
                            foreach (int item in poDetail.EmployeeID)
                            {

                                POODCEmployeeDetails emp = new POODCEmployeeDetails
                                {
                                    PODetailsID = poDetailsID,
                                    EmpID = item
                                };

                                ctx.POODCEmployeeDetails.Add(emp);
                                ctx.SaveChanges();
                            }
                        }

                    }

                }


                User requestingUser = ctx.User.First(p => p.UserID == po.CreatedBy);  //Get associated requesting user

                if (po.operation != "update")
                {
                    WebAPI.Services.MailServices.SendPOApprovalEmail(po.PurchaseOrderNumber,
                        requestingUser.FirstName + " " + requestingUser.LastName, po.ProjectID.ToString());
                }

                if (po.Status != "Created")
                {
                    User targetedUser = ctx.User.First(p => p.UserID == po.UpdatedBy);
                    WebAPI.Services.MailServices.SendPOStatusUpdateEmail(po.PurchaseOrderNumber,
                        targetedUser.FirstName + " " + targetedUser.LastName,
                        requestingUser.FirstName + " " + requestingUser.LastName,
                        requestingUser.Email,
                        po.ProjectID.ToString(), po.Status, po.Reason);
                }

            }

        }

        public static String getClass(String costCode)
        {
            String result = "";



            return result;
        }

    }
}