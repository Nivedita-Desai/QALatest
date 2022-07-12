using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestApprovedCodeSubmitController : ApiController
    {
        public HttpResponseMessage Get(int ProjectID, int TrendNumber, String UserID, string UniqueCode,string CurrentThreshold)
        {
            var ctx = new CPPDbContext();
            List<TrendApprovalTrackLog> TrendApprovalTrackLog = new List<TrendApprovalTrackLog>();
            var UserEmailId = ctx.User.Where(x => x.UserID == UserID).FirstOrDefault();
            var CheckIfItsApproved = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID 
                                                                   && x.TrendId == TrendNumber 
                                                                   && x.IsApproved == 1 
                                                                   && x.CurrentThreshold == CurrentThreshold).ToList();
            if (CheckIfItsApproved.Count == 0) // Only if Trend Is not Approved by any user in that threshold than only will go inside
            {
                List<TrendApprovalTrackLog> UpdateVal = new List<TrendApprovalTrackLog>();
               
                UpdateVal = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID 
                                                                && x.TrendId == TrendNumber
                                                                && x.CurrentThreshold == CurrentThreshold 
                                                                //&& x.UserID == UserID
                                                                ).ToList();
                foreach (var item in UpdateVal)
                {
                    item.IsActive = 0;
                    ctx.Entry(item).State = EntityState.Modified;
                 }
                ctx.SaveChanges();
                TrendApprovalTrackLog retreivedTrendApproval = new TrendApprovalTrackLog();
                retreivedTrendApproval = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID
                                                                         && x.TrendId == TrendNumber
                                                                         && x.UserID == UserID
                                                                         && x.UniqueCode == UniqueCode 
                                                                         && x.CurrentThreshold == CurrentThreshold).FirstOrDefault();
                retreivedTrendApproval.IsApproved = 1;
                ctx.Entry(retreivedTrendApproval).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                var DataAfterAproval = ctx.TrendApprovalTrackLog.Where(x => x.ProjectId == ProjectID 
                                                                       && x.TrendId == TrendNumber
                                                                       && x.UserID == UserID 
                                                                       && x.UniqueCode == UniqueCode).ToList();
                TrendApprovalTrackLog = DataAfterAproval;
                foreach (var ListDet in TrendApprovalTrackLog)
                {
                    ListDet.IsApproved = 0;
                }
            }
            else
            {
                TrendApprovalTrackLog = CheckIfItsApproved;
                foreach (var ListDet in TrendApprovalTrackLog)
                {
                    ListDet.IsApproved = 1;
                }
            }
            var jsonNew = new
            {
                result = TrendApprovalTrackLog
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}