using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("cost_line_item_tracker")]
    public class CostLineItemTracker
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String ProjectDivisionID { get; set; }
        public String ProjectNumber { get; set; }
        public String ProjectElementNumber { get; set; }
        public String ProjectID { get; set; }
        public String TrendNumber { get; set; }
        public String PhaseCode { get; set; }
        public String CategoryID { get; set; }
        public String SubCategoryID { get; set; }

        public String CostType { get; set; } //L,F,ODC,U

        //Labor Type
        public String PositionID { get; set; }
        public String EmployeeID { get; set; }

        //Contractor Type
        public String SubcontractorTypeID { get; set; }
        public String SubcontractorID { get; set; }

        //ODC
        public String ODCTypeID { get; set; }

        //Material
        public String MaterialCategoryID { get; set; }
        public String MaterialID { get; set; }

        public String lineItemNumber { get; set; }
        //Nivedita 10022022
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public static CostLineItemTracker save(String ProjectClassID, String ProjectID, String ProjectNumber,String ProjectElementNumber, String TrendNumber, String PhaseCode, String CategoryID,
                                    String SubCategoryID, String CostType, String FTEPositionID, String EmployeeID, String SubcontractorTypeID, String SubcontractorID, 
                                    String ODCTypeID, String MaterialCategoryID, String MaterialID, String lineItemNUmber)
        {
            CostLineItemTracker costLineItem = new CostLineItemTracker();
            using (var ctx = new CPPDbContext())
            {
             
                costLineItem.ProjectDivisionID = ProjectClassID;
                costLineItem.ProjectID = ProjectID;
                costLineItem.ProjectNumber = ProjectNumber;
                costLineItem.ProjectElementNumber = ProjectElementNumber;
                costLineItem.TrendNumber = TrendNumber;
                costLineItem.PhaseCode = PhaseCode;
                costLineItem.CategoryID = CategoryID;
                costLineItem.SubCategoryID = SubCategoryID;
                costLineItem.CostType = CostType;
                costLineItem.PositionID = FTEPositionID;
                costLineItem.EmployeeID = EmployeeID;
                costLineItem.SubcontractorTypeID = SubcontractorTypeID;
                costLineItem.SubcontractorID = SubcontractorID;
                costLineItem.ODCTypeID = ODCTypeID;
                costLineItem.MaterialCategoryID = MaterialCategoryID;
                costLineItem.MaterialID = MaterialID;

                if (lineItemNUmber == null || lineItemNUmber == "") lineItemNUmber = "01";

                costLineItem.lineItemNumber = lineItemNUmber;   
                costLineItem.lineItemNumber = lineItemNUmber;
                
                ctx.CostLineItemTracker.Add(costLineItem);
                ctx.SaveChanges();
            }
            return costLineItem;
          
        }

        /// <summary>
        /// Remove Cost Line
        /// </summary>
        /// <param name="CostType">
        ///      <para>Required</para>
        ///     <para>F - FTE</para> <para>L - Contractor </para> <para>U - Material </para> <para>ODC - Other Direct Cost</para> 
        /// </param>
        /// <param name="ProjectDivisionID">Required</param>
        /// <param name="ProjectNumber">Required</param>
        /// <param name="PhaseCode">Required</param>
        /// <param name="CategoryID">Required</param>
        /// <param name="SubCategoryID">Required</param>
        /// <param name="PositionID">
        ///     Required for FTE, optional otherwise
        /// </param>
        /// <param name="EmployeeID">
        ///     Required for FTE, optional otherwise
        /// </param>
        /// <param name="SubcontractorTypeID">
        ///     Required for L (Lumpsum), optional otherwise
        /// </param>
        /// <param name="SubcontractorID">
        ///     Required for L (Lumpsum), optional otherwise
        /// </param>
        /// <param name="ODCTypeID"></param>
        /// <param name="MaterialCategoryID"></param>
        /// <param name="MaterialID"></param>
        /// <param name="lineItemNumber"></param>
        public static void  removeCostLine(String CostType, String ProjectDivisionID, String ProjectNumber,String ProjectElementNumber, String PhaseCode, String CategoryID, String SubCategoryID,
                                        String PositionID, String EmployeeID, String SubcontractorTypeID, String SubcontractorID, String ODCTypeID,
                                        String MaterialCategoryID, String MaterialID, String lineItemNumber)
        {
            using(var ctx = new CPPDbContext())
            {
                if(CostType == "F")
                {
                    CostLineItemTracker costLineItem = ctx.CostLineItemTracker.Where(
                                                      a => a.CostType == CostType && a.ProjectDivisionID.Equals(ProjectDivisionID)
                                                      && a.ProjectNumber.Equals(ProjectNumber) && a.ProjectElementNumber.Equals(ProjectElementNumber)
                                                      && a.PhaseCode.Equals(PhaseCode) && a.CategoryID.Equals(CategoryID)
                                                      && a.SubCategoryID.Equals(SubCategoryID) && a.PositionID.Equals(PositionID) && a.EmployeeID.Equals(EmployeeID)

                                                  ).FirstOrDefault();
                    if (costLineItem != null)
                    {
                        //Nivedita 10022022
                        //ctx.CostLineItemTracker.Remove(costLineItem);
                        costLineItem.IsDeleted = true;
                        costLineItem.DeletedDate = DateTime.Now;
                        costLineItem.DeletedBy = "";
                        ctx.SaveChanges();
                    }
                }else if(CostType == "L")
                {
                    CostLineItemTracker costLineItem = ctx.CostLineItemTracker.Where(
                                                    a => a.CostType == CostType && a.ProjectDivisionID.Equals(ProjectDivisionID)
                                                    && a.ProjectNumber.Equals(ProjectNumber) && a.ProjectElementNumber.Equals(ProjectElementNumber)
                                                    && a.PhaseCode.Equals(PhaseCode) && a.CategoryID.Equals(CategoryID)
                                                    && a.SubCategoryID.Equals(SubCategoryID) 
                                                    && a.SubcontractorTypeID.Equals(SubcontractorTypeID) && a.SubcontractorID.Equals(SubcontractorID)

                                                ).FirstOrDefault();
                    if (costLineItem != null)
                    {
                        //Nivedita 10022022
                        //ctx.CostLineItemTracker.Remove(costLineItem);
                        costLineItem.IsDeleted = true;
                        costLineItem.DeletedDate = DateTime.Now;
                        costLineItem.DeletedBy = "";
                        ctx.SaveChanges();
                    }
                }else if(CostType == "ODC")
                {
                    CostLineItemTracker costLineItem = ctx.CostLineItemTracker.Where(
                                                  a => a.CostType == CostType && a.ProjectDivisionID.Equals(ProjectDivisionID)
                                                  && a.ProjectNumber.Equals(ProjectNumber) && a.ProjectElementNumber.Equals(ProjectElementNumber)
                                                  && a.PhaseCode.Equals(PhaseCode) && a.CategoryID.Equals(CategoryID)
                                                  && a.SubCategoryID.Equals(SubCategoryID)
                                                  && a.ODCTypeID.Equals(ODCTypeID)

                                              ).FirstOrDefault();
                    if (costLineItem != null)
                    {
                        //Nivedita 10022022
                        //ctx.CostLineItemTracker.Remove(costLineItem);
                        costLineItem.IsDeleted = true;
                        costLineItem.DeletedDate = DateTime.Now;
                        costLineItem.DeletedBy = "";
                        ctx.SaveChanges();
                    }
                }else if(CostType == "U")
                {
                    CostLineItemTracker costLineItem = ctx.CostLineItemTracker.Where(
                                                  a => a.CostType == CostType && a.ProjectDivisionID.Equals(ProjectDivisionID)
                                                  && a.ProjectNumber.Equals(ProjectNumber) && a.ProjectElementNumber.Equals(ProjectElementNumber)
                                                  && a.PhaseCode.Equals(PhaseCode) && a.CategoryID.Equals(CategoryID)
                                                  && a.SubCategoryID.Equals(SubCategoryID)
                                                  && a.MaterialCategoryID.Equals(MaterialCategoryID) && a.MaterialID.Equals(MaterialID)

                                              ).FirstOrDefault();
                    if (costLineItem != null)
                    {
                        //Nivedita 10022022
                        //ctx.CostLineItemTracker.Remove(costLineItem);
                        costLineItem.IsDeleted = true;
                        costLineItem.DeletedDate = DateTime.Now;
                        costLineItem.DeletedBy = "";
                        ctx.SaveChanges();
                    }
                }
              
             


            }
        }



        public static CostLineItemTracker checkIfCostLineItemExist(String CostType, String ProjectDivisionID, String ProjectNumber, String ProjectElementNumber, String PhaseCode, String CategoryID, String SubCategoryID,
                                        String PositionID, String EmployeeID, String SubcontractorTypeID, String SubcontractorID, String ODCTypeID,
                                        String MaterialCategoryID, String MaterialID, String lineItemNumber, String TrendNumber)
        {
            CostLineItemTracker costLineItem = null;
            using (var ctx = new CPPDbContext())
            {
              
                if (CostType == "F")
                {
                     costLineItem = ctx.CostLineItemTracker.Where(
                                              a => a.CostType == CostType && a.ProjectDivisionID == ProjectDivisionID  //ProjectDivisionId is the same as ProjectClassID
                                              && a.ProjectNumber == ProjectNumber && a.ProjectElementNumber == ProjectElementNumber
                                              && a.PhaseCode == PhaseCode && a.CategoryID == CategoryID
                                              && a.SubCategoryID == SubCategoryID
                                              && a.PositionID == PositionID
                                              && a.EmployeeID == EmployeeID
                                              && a.TrendNumber == TrendNumber
                                          ).FirstOrDefault();
                }
                else if(CostType == "L")
                {
                    costLineItem = ctx.CostLineItemTracker.Where(
                                         a => a.CostType == CostType && a.ProjectDivisionID == ProjectDivisionID  //ProjectDivisionId is the same as ProjectClassID
                                         && a.ProjectNumber == ProjectNumber && a.ProjectElementNumber == ProjectElementNumber
                                         && a.PhaseCode == PhaseCode && a.CategoryID == CategoryID
                                         && a.SubCategoryID == SubCategoryID
                                         && a.SubcontractorTypeID == SubcontractorTypeID
                                         && a.SubcontractorID == SubcontractorID

                                     ).FirstOrDefault();
                }
                else if(CostType == "U")
                {
                    costLineItem = ctx.CostLineItemTracker.Where(
                                         a => a.CostType == CostType && a.ProjectDivisionID == ProjectDivisionID  //ProjectDivisionId is the same as ProjectClassID
                                         && a.ProjectNumber == ProjectNumber && a.ProjectElementNumber == ProjectElementNumber
                                         && a.PhaseCode == PhaseCode && a.CategoryID == CategoryID
                                         && a.SubCategoryID == SubCategoryID
                                         && a.MaterialCategoryID == MaterialCategoryID
                                         && a.MaterialID == MaterialID

                                     ).FirstOrDefault();
                }
                else if(CostType == "ODC")
                {
                    costLineItem = ctx.CostLineItemTracker.Where(
                                        a => a.CostType == CostType && a.ProjectDivisionID == ProjectDivisionID  //ProjectDivisionId is the same as ProjectClassID
                                        && a.ProjectNumber == ProjectNumber && a.ProjectElementNumber == ProjectElementNumber
                                        && a.PhaseCode == PhaseCode && a.CategoryID == CategoryID
                                        && a.SubCategoryID == SubCategoryID
                                        && a.ODCTypeID == ODCTypeID

                                    ).FirstOrDefault();
                }
              

                if (costLineItem != null)
                    return costLineItem;
            }

            return costLineItem;
          
        }
    }
}