using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using WebAPI.Helper;

namespace WebAPI.Models
{
	[Table("change_order")]
	public class ChangeOrder : Audit
	{
		[NotMapped]
		public int Operation;

		[NotMapped]
		public int DocumentID;

		[NotMapped]
		public String DocumentName;

		[NotMapped]
		public Document DocumentDraft;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ChangeOrderID { get; set; }
		public String ChangeOrderName { get; set; }
		public String ChangeOrderNumber { get; set; }
		public String ChangeOrderAmount { get; set; }
		public String ChangeOrderScheduleChange { get; set; }

        public String OrderType { get; set; }
        public String OrderDate { get; set; }
        public int? ProgramElementID { get; set; }
        public DateTime? DurationDate { get; set; } // Jignesh-ChangeOrderPopUpChanges
        public int ScheduleImpact { get; set; } // Jignesh-24-03-2021
        public string Reason { get; set; } // Jignesh-ChangeOrderPopUpChanges
        public int? ModificationTypeId { get; set; } // Jignesh-ChangeOrderPopUpChanges

        // Added by Amruta
        [NotMapped]
        public DateTime ProjectEndDateCO { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public static List<ChangeOrder> getChangeOrder()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

			List<ChangeOrder> ChangeOrderList = new List<ChangeOrder>();
			try
			{

				using (var ctx = new CPPDbContext())
				{
                    //var query1 = ctx.ChangeOrder.
                    //                    Join(ctx.Document, doc => doc.ChangeOrderID, docType => docType.ChangeOrderID, (doc, docType) => new { doc, docType }).
                    //                    // OrderByDescending(a => a.doc.CreatedDate).
                    //                    //OrderBy(doc => doc.LastUpdatedDate).
                    //                    //Where( x => x.doc.ProjectID = projectID).
                    //                    Select(m => new ChangeOrder
                    //                    {
                    //                        DocumentName = String.IsNullOrEmpty(m.docType.DocumentName) ? "" : m.docType.DocumentName,
                    //                        DocumentDraft = null,
                    //                        Operation = 0,
                    //                        ChangeOrderID = m.doc.ChangeOrderID,
                    //                        ChangeOrderName = m.doc.ChangeOrderName,
                    //                        ChangeOrderNumber = m.doc.ChangeOrderNumber,
                    //                        ChangeOrderAmount = m.doc.ChangeOrderAmount,
                    //                        ChangeOrderScheduleChange = m.doc.ChangeOrderScheduleChange,
                    //                        OrderType = m.doc.OrderType,
                    //                        OrderDate = m.doc.OrderDate,
                    //                        ProgramElementID = m.doc.ProgramElementID,
                    //                        CreatedBy = m.doc.CreatedBy,
                    //                        UpdatedBy = m.doc.UpdatedBy,
                    //                        CreatedDate = m.doc.CreatedDate,
                    //                        //UpdatedDate = left.UpdatedDate,
                    //                        DocumentID = m.doc.DocumentID
                    //                    }).AsNoTracking().
                    //                    ToList();

                    var students = (from left in ctx.ChangeOrder
                                    where left.IsDeleted == false
                                    join right in ctx.Document on left.ChangeOrderID equals right.ChangeOrderID into joinedList
                                    from sub in joinedList.DefaultIfEmpty()
                                    orderby left.ChangeOrderID descending
                                    select new
                                    {
                                        DocumentName = String.IsNullOrEmpty(sub.DocumentName) ? "" : sub.DocumentName,
                                       // DocumentName = sub.DocumentID,
                                       //   DocumentDraft = sub.DocumentID,
                                        Operation = 0,
                                        ChangeOrderID = left.ChangeOrderID,
                                        ChangeOrderName = left.ChangeOrderName,
                                        ChangeOrderNumber = left.ChangeOrderNumber,
                                        ChangeOrderAmount = left.ChangeOrderAmount,
                                        ChangeOrderScheduleChange = left.ChangeOrderScheduleChange,
                                        OrderType = left.OrderType,
                                        OrderDate = left.OrderDate,
                                        ProgramElementID = left.ProgramElementID,
                                        CreatedBy = left.CreatedBy,
                                        UpdatedBy = left.UpdatedBy,
                                        CreatedDate = left.CreatedDate,
                                        ModificationTypeId = left.ModificationTypeId, //Jignesh-ChangeOrderPopUpChanges
                                        DurationDate = left.DurationDate, //Jignesh-ChangeOrderPopUpChanges
                                        Reason = left.Reason, //Jignesh-ChangeOrderPopUpChanges
                                        ScheduleImpact = left.ScheduleImpact, // Jignesh-03-2021
                                        //UpdatedDate = left.UpdatedDate,
                                        DocumentID = String.IsNullOrEmpty(sub.DocumentName) ? 0 : sub.DocumentID,// String.IsNullOrEmpty(Convert.ToString(sub.DocumentID)) ? 0 : sub.DocumentID,
                                    }).ToList()
     .Select(x => new ChangeOrder()
     {
         DocumentName = String.IsNullOrEmpty(x.DocumentName) ? "" : x.DocumentName,
                                   //  DocumentDraft = Convert.ToString(x.DocumentDraft),
                                     Operation = 0,
                                     ChangeOrderID = x.ChangeOrderID,
                                     ChangeOrderName = x.ChangeOrderName,
                                     ChangeOrderNumber = x.ChangeOrderNumber,
                                     ChangeOrderAmount = x.ChangeOrderAmount,
                                     ChangeOrderScheduleChange = x.ChangeOrderScheduleChange,
                                     OrderType = x.OrderType,
                                     OrderDate = x.OrderDate,
                                     ProgramElementID = x.ProgramElementID,
                                     CreatedBy = x.CreatedBy,
                                     UpdatedBy = x.UpdatedBy,
                                     CreatedDate = x.CreatedDate,
                                     DocumentID = x.DocumentID,
                                     ModificationTypeId = x.ModificationTypeId, //Jignesh-ChangeOrderPopUpChanges
                                     DurationDate = x.DurationDate, //Jignesh-ChangeOrderPopUpChanges
                                     Reason = x.Reason, //Jignesh-ChangeOrderPopUpChanges
                                     ScheduleImpact = x.ScheduleImpact, // Jignesh-03-2021
     });

                    //var query = (from left in ctx.ChangeOrder
                    //             join right in ctx.Document on left.ChangeOrderID equals right.ChangeOrderID into joinedList
                    //             from sub in joinedList.DefaultIfEmpty()
                    //             orderby left.ChangeOrderID descending
                    //             select new ChangeOrder
                    //             {
                    //                 DocumentName = String.IsNullOrEmpty(sub.DocumentName) ? "" : sub.DocumentName,
                    //                 DocumentDraft = null,
                    //                 Operation = 0,
                    //                 ChangeOrderID = left.ChangeOrderID,
                    //                 ChangeOrderName = left.ChangeOrderName,
                    //                 ChangeOrderNumber = left.ChangeOrderNumber,
                    //                 ChangeOrderAmount = left.ChangeOrderAmount,
                    //                 ChangeOrderScheduleChange = left.ChangeOrderScheduleChange,
                    //                 OrderType = left.OrderType,
                    //                 OrderDate = left.OrderDate,
                    //                 ProgramElementID = left.ProgramElementID,
                    //                 CreatedBy = left.CreatedBy,
                    //                 UpdatedBy = left.UpdatedBy,
                    //                 CreatedDate = left.CreatedDate,
                    //                 //UpdatedDate = left.UpdatedDate,
                    //                 DocumentID = left.DocumentID
                    //             }).ToList();

                    //  ChangeOrderList = query1;
                    ChangeOrderList = students.ToList();
                    //ChangeOrderList = ctx.ChangeOrder.OrderBy(a => a.ChangeOrderName).ToList();
                }

			}
			catch (Exception ex)
			{
				var stackTrace = new StackTrace(ex, true);
				var line = stackTrace.GetFrame(0).GetFileLineNumber();
				Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
			}
			finally
			{
			}
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

			return ChangeOrderList;
		}


		public static String registerChangeOrder(ChangeOrder ChangeOrder)
		{

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					using (var dbContextTransaction = ctx.Database.BeginTransaction())
					{
						try
						{
							ChangeOrder retreivedChangeOrder = new ChangeOrder();
                            ChangeOrder retreivedChangeOrderNo = new ChangeOrder();

                            Document transferDocument = new Document();

                            //retreivedChangeOrder = ctx.ChangeOrder.Where(u => u.ChangeOrderName == ChangeOrder.ChangeOrderName).FirstOrDefault();

                            retreivedChangeOrder = ctx.ChangeOrder.Where(u => u.ChangeOrderName == ChangeOrder.ChangeOrderName
                                                                                && u.ProgramElementID == ChangeOrder.ProgramElementID && u.IsDeleted == false).FirstOrDefault();   //Manasi

                            retreivedChangeOrderNo = ctx.ChangeOrder.Where(u => u.ChangeOrderNumber == ChangeOrder.ChangeOrderNumber
                                                                                && u.ProgramElementID == ChangeOrder.ProgramElementID && u.IsDeleted == false).FirstOrDefault();

                            #region old logic
                            //if (true) //retreivedChangeOrder == null
                            //{

                            //    //register
                            //    if (ChangeOrder.ProgramElementID == 0)
                            //    {
                            //        ChangeOrder.ProgramElementID = null;
                            //    }
                            //    //if (!String.IsNullOrEmpty(ChangeOrder.OrderDate))
                            //    //{
                            //    //    String abc = ChangeOrder.OrderDate;

                            //    //    for (int i = 2; i <= abc.Length - 3; i += 2)
                            //    //    {
                            //    //        abc = abc.Insert(i, "/");
                            //    //        i++;
                            //    //    }

                            //    //    ChangeOrder.OrderDate = abc;
                            //    //}

                            //    ctx.ChangeOrder.Add(ChangeOrder);
                            //    ctx.SaveChanges();


                            //    if (ChangeOrder.DocumentID > 0)
                            //    {
                            //        transferDocument = ctx.Document.Where(u => u.DocumentID == ChangeOrder.DocumentID).FirstOrDefault();
                            //        transferDocument.ChangeOrderID = ChangeOrder.ChangeOrderID;
                            //        ctx.Entry(transferDocument).State = System.Data.Entity.EntityState.Modified;
                            //    }
                            //    else
                            //    {
                            //        Document.registerDocument(ChangeOrder.DocumentDraft);
                            //    }

                            //    ctx.SaveChanges();

                            //    result = "Success," + ChangeOrder.ChangeOrderID;
                            //}
                            //else
                            //{
                            //    result += ChangeOrder.ChangeOrderName + "' failed to be created, it already exist.\n";
                            //}
                            #endregion old logic

                            if (retreivedChangeOrder == null && retreivedChangeOrderNo == null)
                            {
                                if (ChangeOrder.ProgramElementID == 0)
                                {
                                    ChangeOrder.ProgramElementID = null;
                                }
                                ctx.ChangeOrder.Add(ChangeOrder);
                                ctx.SaveChanges();


                                if (ChangeOrder.DocumentID > 0)
                                {
                                    transferDocument = ctx.Document.Where(u => u.DocumentID == ChangeOrder.DocumentID).FirstOrDefault();
                                    transferDocument.ChangeOrderID = ChangeOrder.ChangeOrderID;
                                    ctx.Entry(transferDocument).State = System.Data.Entity.EntityState.Modified;
                                }
                                else
                                {
                                    Document.registerDocument(ChangeOrder.DocumentDraft);
                                }

                                ctx.SaveChanges();

                                //Added by Amruta for saving the project end date on change order creation
                                ProgramElement programElement = ctx.ProgramElement.Where(p => p.ProgramElementID == ChangeOrder.ProgramElementID).FirstOrDefault();
                                if (programElement != null)
                                {
                                    programElement.ProjectPEndDate = ChangeOrder.ProjectEndDateCO;
                                    ctx.SaveChanges();
                                }


                                //var FileUploadCtrl =new WebAPI.Controllers.FilesUploadController();
                                //_ = FileUploadCtrl.Post("", 0, 0, 0, 0, ChangeOrder.ChangeOrderID, 0);
                                result = "Success," + ChangeOrder.ChangeOrderID;
                            }
                            else if(retreivedChangeOrder != null)
                            {
                                //result += ChangeOrder.ChangeOrderName + "' failed to be created, it already exist.\n";
                                //result += "Duplicate order # will be created. Failed to create Change Order";  //Manasi
                                result += "Failed to create change order due to duplicate title. Please update and retry.";  //Manasi 14-07-2020
                            }
                            else if (retreivedChangeOrderNo != null)
                            {
                                //result += ChangeOrder.ChangeOrderName + "' failed to be created, it already exist.\n";
                                //result += "Duplicate order # will be created. Failed to create Change Order";  //Manasi
                                result += "Failed to create change order due to duplicate Client Change Order #. Please update and retry.";  //Jignesh-01-03-2021
                            }


                            dbContextTransaction.Commit();
						}
						catch (Exception ex)
						{
							//Log, handle or absorbe I don't care ^_^
						}
					}
				}


			}
			catch (Exception ex)
			{
				var stackTrace = new StackTrace(ex, true);
				var line = stackTrace.GetFrame(0).GetFileLineNumber();
				Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
			}
			finally
			{
			}

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

			return result;
		}
		public static String updateChangeOrder(ChangeOrder ChangeOrder)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";

			try
			{
                using (var ctx = new CPPDbContext())
                {
                    ChangeOrder retreivedChangeOrder = new ChangeOrder();
                    retreivedChangeOrder = ctx.ChangeOrder.Where(u => u.ChangeOrderID == ChangeOrder.ChangeOrderID).FirstOrDefault();

                    ChangeOrder duplicateChangeOrder = new ChangeOrder();
                    ChangeOrder duplicateChangeOrderNo = new ChangeOrder();
                    //duplicateChangeOrder = ctx.ChangeOrder.Where(u => u.ChangeOrderName == ChangeOrder.ChangeOrderName
                    //                                                    && u.ChangeOrderID != ChangeOrder.ChangeOrderID).FirstOrDefault();

                    duplicateChangeOrder = ctx.ChangeOrder.Where(u => u.ChangeOrderName == ChangeOrder.ChangeOrderName
                                                                        && u.ProgramElementID == ChangeOrder.ProgramElementID
                                                                        && u.ChangeOrderID != ChangeOrder.ChangeOrderID).FirstOrDefault();  //Manasi

                    duplicateChangeOrderNo = ctx.ChangeOrder.Where(u => u.ChangeOrderNumber == ChangeOrder.ChangeOrderNumber
                                                                        && u.ProgramElementID == ChangeOrder.ProgramElementID
                                                                        && u.ChangeOrderID != ChangeOrder.ChangeOrderID).FirstOrDefault();  //Manasi

                    //if (duplicateChangeOrder != null || duplicateChangeOrderNo != null)
                    //{
                    //    //result = ChangeOrder.ChangeOrderName + " failed to be updated, duplicate will be created.\n";
                    //    //result = "Duplicate order # will be created. Failed to update Change Order";  //Manasi
                    //    result += "Failed to update change order due to duplicate entry. Please update and retry.";  //Manasi 13-07-2020
                    //}
                    if(duplicateChangeOrder != null)
                    {
                        result += "Failed to update change order due to duplicate title. Please update and retry.";  //Manasi 14-07-2020
                    }
                    else if(duplicateChangeOrderNo != null)
                    {
                        result += "Failed to update change order due to duplicate order #. Please update and retry.";  //Manasi 14-07-2020
                    }
                    else
                    {
                        if (retreivedChangeOrder != null)
                        {
                            CopyUtil.CopyFields<ChangeOrder>(ChangeOrder, retreivedChangeOrder);
                            if (retreivedChangeOrder.ProgramElementID == 0)
                            {
                                retreivedChangeOrder.ProgramElementID = null;
                            }
                            ctx.Entry(retreivedChangeOrder).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();

                            WebAPI.Models.ProgramElement.updatePojectEndDateOnChangeOrder(ChangeOrder.ProgramElementID.GetValueOrDefault(), ChangeOrder.ProjectEndDateCO);

                            result = ChangeOrder.ChangeOrderName + " has been updated successfully.\n";
                        }
                        else
                        {
                            result += ChangeOrder.ChangeOrderName + " failed to be updated, it does not exist.\n";
                        }
                    }
				}

			}
			catch (Exception ex)
			{
				var stackTrace = new StackTrace(ex, true);
				var line = stackTrace.GetFrame(0).GetFileLineNumber();
				Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);


			}
			finally
			{

			}
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

			return result;

		}
		public static String deleteChangeOrder(ChangeOrder ChangeOrder)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					ChangeOrder retreivedChangeOrder = new ChangeOrder();
					retreivedChangeOrder = ctx.ChangeOrder.Where(u => u.ChangeOrderID == ChangeOrder.ChangeOrderID).FirstOrDefault();

					if (retreivedChangeOrder != null)
					{
						//Delete old list
						List<Document> documentList = ctx.Document.Where(t => t.ChangeOrderID == retreivedChangeOrder.ChangeOrderID).ToList();

						for (int x = 0; x < documentList.Count; x++)
						{
                            documentList[x].DeletedBy = ChangeOrder.DeletedBy;
							Document.deleteDocument(documentList[x]);
						}

                        retreivedChangeOrder.IsDeleted = true;
                        retreivedChangeOrder.DeletedDate = DateTime.Now;
                        retreivedChangeOrder.DeletedBy = ChangeOrder.DeletedBy;
                        
                        //ctx.ChangeOrder.Remove(retreivedChangeOrder);
                        ctx.SaveChanges();

                        WebAPI.Models.ProgramElement.updatePojectEndDateOnChangeOrder(ChangeOrder.ProgramElementID.GetValueOrDefault(), ChangeOrder.ProjectEndDateCO);

                        result = ChangeOrder.ChangeOrderName + " has been deleted successfully.\n";
					}
					else
					{
						result = ChangeOrder.ChangeOrderName + " failed to be deleted, it does not exist.\n";
					}
				}

			}
			catch (Exception ex)
			{
				result += ChangeOrder.ChangeOrderName + " failed to be deleted due to dependencies.\n";
				var stackTrace = new StackTrace(ex, true);
				var line = stackTrace.GetFrame(0).GetFileLineNumber();
				Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
			}
			finally
			{
			}
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

			return result;
		}
	}
}