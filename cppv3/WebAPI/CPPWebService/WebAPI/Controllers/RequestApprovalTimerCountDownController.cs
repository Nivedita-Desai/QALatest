using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestApprovalTimerCountDownController : ApiController
    {
        //// GET: RequestTrendApproval
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public HttpResponseMessage Get(int ProjectID, int TrendNumber, String UserID,string CurrentThreshold)
        {
            var ctx = new CPPDbContext();
            Boolean IsRecreatedCheck = false;
            List<TrendApprovalTrackLog> TrendApprovalTrackLog = new List<TrendApprovalTrackLog>();
            var UserEmailId = ctx.User.Where(x => x.UserID == UserID).FirstOrDefault();
            var CheckIfItsApproved = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID 
                                                                     && x.TrendId == TrendNumber 
                                                                     && x.IsApproved == 1 
                                                                     && x.CurrentThreshold == CurrentThreshold).ToList();

            if (CheckIfItsApproved.Count == 0) // Only if Trend Is not Approved by any user in that threshold than only will go inside
            {


                var resultCode = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID
                                                                 && x.TrendId == TrendNumber
                                                                 && x.UserID == UserID && x.IsActive == 1
                                                                 && x.CurrentThreshold == CurrentThreshold
                                                                 && x.ExpiredDate > DateTime.Now).ToList();

                if (resultCode.Count == 0) // For the first time any user requested for Approval
                {
                ineligible:
                    Random rand = new Random();
                    int randomNumber = rand.Next(100000, 999999);
                    string UniqueCode = Convert.ToString(randomNumber);

                    bool has = ctx.TrendApprovalTrackLog.Any(x => x.UniqueCode.Contains(UniqueCode));
                    if (has)
                    {
                        goto ineligible;
                    }
                    else
                    {
                        #region For Recreate Code for Approval
                        var RecreateIfExpire = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID && x.TrendId == TrendNumber
                                                           && x.UserID == UserID && x.IsActive == 1 && x.CurrentThreshold == CurrentThreshold 
                                                           && x.ExpiredDate < DateTime.Now).ToList();
                        if (RecreateIfExpire.Count > 0) // If user Tries to approve again after 30 min
                        {
                            TrendApprovalTrackLog retreivedTrendApproval = new TrendApprovalTrackLog();
                            retreivedTrendApproval = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID && x.TrendId == TrendNumber
                                                           && x.UserID == UserID && x.IsActive == 1 && x.CurrentThreshold == CurrentThreshold
                                                           && x.ExpiredDate < DateTime.Now).FirstOrDefault();
                            retreivedTrendApproval.IsActive = 0;
                            ctx.Entry(retreivedTrendApproval).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            IsRecreatedCheck = true;
                        }
                        #endregion

                        WebAPI.Services.MailServices.EmailToGetTrendApprovedByCode(UserID, Convert.ToString(TrendNumber), Convert.ToString(ProjectID), UniqueCode,false);

                        TrendApprovalTrackLog AddTrendRecord = new TrendApprovalTrackLog();
                        AddTrendRecord.ProjectId = ProjectID;
                        AddTrendRecord.TrendId = TrendNumber;
                        AddTrendRecord.UserID = UserID;
                        AddTrendRecord.UniqueCode = UniqueCode;
                        AddTrendRecord.CreatedDate = DateTime.Now;
                        AddTrendRecord.CurrentThreshold = CurrentThreshold;
                        //ExpiredDate = ExpiredDate.AddMinutes(30);
                        //-----------------------------Swapnil 17-09-2020 -----------------------------------------
                        MFAConfiguration mfaDet = ctx.MFAConfigurationDetails.Where(u => u.MFAConfigID == 1).FirstOrDefault();
                        DateTime ExpiredDate = DateTime.Now;
                        ExpiredDate = ExpiredDate.AddMinutes(mfaDet.ApprovalCodeValidity);
                        //-----------------------------------------------------------------------------------------
                        AddTrendRecord.ExpiredDate = ExpiredDate;
                        AddTrendRecord.IsActive = 1;
                        AddTrendRecord.IsApproved = 0;
                        AddTrendRecord.IsRecreated = IsRecreatedCheck;
                        AddTrendRecord.UserEmailid = UserEmailId.Email;
                        ctx.TrendApprovalTrackLog.Add(AddTrendRecord);
                        ctx.SaveChanges();
                        TrendApprovalTrackLog.Add(AddTrendRecord);
                    }

                }
                else
                {
                    TrendApprovalTrackLog = resultCode;
                    foreach (var ListDet in TrendApprovalTrackLog)
                    {
                        ListDet.UserEmailid = UserEmailId.Email;
                        ListDet.IsRecreated = false;
                    }
                    // TrendApprovalTrackLog.UserEmailid =  UserEmailId.Email;
                }
            }
            else
            {
                TrendApprovalTrackLog = CheckIfItsApproved;
                foreach (var ListDet in TrendApprovalTrackLog)
                {
                    ListDet.IsApproved =1;
                    ListDet.IsRecreated = false;
                }
                // TrendApprovalTrackLog.UserEmailid =  UserEmailId.Email;
            }
            var jsonNew = new
            {
                result = TrendApprovalTrackLog
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }



    }
}