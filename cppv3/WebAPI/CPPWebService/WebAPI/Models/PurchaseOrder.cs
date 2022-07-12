using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("purchase_order")]
    public class PurchaseOrder : Audit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String PurchaseOrderNumber { get; set; }
        public String PurchaseOrderBaseNumber { get; set; } //P
        public String ProjectNumber { get; set; }
        public String OrderNumber { get; set; }
        public String Description { get; set; }
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
        public string Status { get; set; }
        public int? VendorId { get; set; }
        public string Reason { get; set; }

        [NotMapped]
        public string operation { get; set; }
        [NotMapped]
        public string projectName { get; set; }
        [NotMapped]
        public string projectElementName { get; set; }
        //[ForeignKey("TrendID")]
        //public virtual Trend Trend { get; set; }

        public static PurchaseOrder createNewPurchaseOrder(PurchaseOrder order)
        {
            using(var ctx = new CPPDbContext())
            {
                order.Status = "Created";
                ctx.PurchaseOrder.Add(order);
                ctx.SaveChanges();
            }

            return order;
        }

        public static PurchaseOrder UpdatePurchaseOrder(PurchaseOrder order)
        {
            using (var ctx = new CPPDbContext())
            {
                User retreivedUser = new User();
                ctx.Entry(order).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }

            return order;
        }
        public PurchaseOrder getNewPurchaseOrder(int projectID)
        {
            PurchaseOrder po = new PurchaseOrder();

            using(var ctx = new CPPDbContext())
            {
                Project project = ctx.Project.Where(a => a.ProjectID == projectID).FirstOrDefault();
                ProgramElement programElement = ctx.ProgramElement.Where(a => a.ProgramElementID == project.ProgramElementID)
                                                        .FirstOrDefault();

                var projectNumber = programElement.ProjectNumber.ToString() + project.ProjectElementNumber.ToString();

                var existingPO = ctx.PurchaseOrder.Where(a => a.ProjectNumber == projectNumber).FirstOrDefault();
                if(existingPO == null)
                {
                    PurchaseOrder newPO = new PurchaseOrder
                    {
                        PurchaseOrderNumber = "P" + projectNumber + "0001",
                        ProjectID = project.ProjectID,
                        Description = "",
                        PurchaseOrderBaseNumber = "P" + projectNumber,
                        ProjectNumber = projectNumber,
                        OrderNumber = "0001",
                        Status = "Initiated",
                        VendorId = null
                    };
                    po = newPO;
                    //ctx.PurchaseOrder.Add(newPO);
                    //ctx.SaveChanges();
                }
                else
                {
                    String newPONumber = ctx.PurchaseOrder.Where(a => a.ProjectNumber == projectNumber).Max(a => a.OrderNumber);
                    if(newPONumber != null)
                    {
                        var nextPO = int.Parse(newPONumber);
                        nextPO += 1;
                        newPONumber = nextPO.ToString().PadLeft(4, '0');
                    }
                    PurchaseOrder newPO = new PurchaseOrder
                    {
                        PurchaseOrderNumber = "P" + projectNumber + newPONumber,
                        ProjectID = project.ProjectID,
                        Description = "",
                        PurchaseOrderBaseNumber = "P" + projectNumber,
                        ProjectNumber = projectNumber,
                        OrderNumber = newPONumber,
                        Status = "Initiated",
                        VendorId = null
                    };
                    po = newPO;
                    //ctx.PurchaseOrder.Add(newPO);
                    //ctx.SaveChanges();
                }
               
            }


            return po;
        }

        public static List<POList> getPOList(int projectID)
        {
            List<POList> poList = new List<POList>();
            try
            {

            

            using (var ctx = new CPPDbContext())
            {
                if (projectID == 0)
                {
                  
                     poList =
                       (from po in ctx.PurchaseOrder 
                       // orderby po.CreatedDate descending
                       join prj in ctx.Project on po.ProjectID equals prj.ProjectID
                      join elm in ctx.ProgramElement on prj.ProgramElementID equals elm.ProgramElementID
                       select new POList
                       {
                       ID = po.ID,
                       PurchaseOrderNumber = po.PurchaseOrderNumber,
                       PurchaseOrderBaseNumber = po.PurchaseOrderBaseNumber,
                       ProjectNumber = po.ProjectNumber,
                       OrderNumber  = po.OrderNumber,
                       Description = po.Description,
                       ProjectID = po.ProjectID,
                       Status = po.Status,
                       VendorId = po.VendorId,
                       Reason = po.Reason,
                       projectName = elm.ProgramElementName,
                       projectElementName = prj.ProjectName,
                       CreatedDate = po.CreatedDate,
                       UpdatedDate = po.UpdatedDate,
                       CreatedBy = po.CreatedBy,
                       UpdatedBy = po.UpdatedBy
                       }).OrderByDescending(c => c.CreatedDate).ToList();
                }
                else
                {
                    // poList = ctx.PurchaseOrder.Include("Project").Where(a => a.ProjectID == projectID).ToList();
                    poList =
                       (from po in ctx.PurchaseOrder
                        //orderby po.CreatedDate descending
                        join prj in ctx.Project on po.ProjectID equals prj.ProjectID
                        join elm in ctx.ProgramElement on prj.ProgramElementID equals elm.ProgramElementID
                        where po.ProjectID == projectID
                        select new POList
                        {
                            ID = po.ID,
                            PurchaseOrderNumber = po.PurchaseOrderNumber,
                            PurchaseOrderBaseNumber = po.PurchaseOrderBaseNumber,
                            ProjectNumber = po.ProjectNumber,
                            OrderNumber = po.OrderNumber,
                            Description = po.Description,
                            ProjectID = po.ProjectID,
                            Status = po.Status,
                            VendorId = po.VendorId,
                            Reason = po.Reason,
                            projectName = elm.ProgramElementName,
                            projectElementName = prj.ProjectName,
                            CreatedDate = po.CreatedDate,
                            UpdatedDate = po.UpdatedDate,
                            CreatedBy = po.CreatedBy,
                            UpdatedBy = po.UpdatedBy
                        }).OrderByDescending(c => c.CreatedDate).ToList();
                }

                 
            }

            }
            catch (Exception ex)
            {

                throw;
            }


            return poList;
        }

        public static PurchaseOrder getPO(int PurchaseOrderID)
        {

            PurchaseOrder po = new PurchaseOrder();
            using (var ctx = new CPPDbContext())
            {

                po = ctx.PurchaseOrder.Where(a => a.ID == PurchaseOrderID)
                                                       .FirstOrDefault();
            }


            return po;
        }

    }
}