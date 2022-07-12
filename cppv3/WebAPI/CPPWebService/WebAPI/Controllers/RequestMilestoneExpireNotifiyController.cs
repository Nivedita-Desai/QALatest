using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestMilestoneExpireNotifiyController //: Controller
    {
        private static String MilestoneExpireIn = ConfigurationManager.AppSettings["MilestoneNotifyBefore"];
        public void GetExpierdMilestone()
        {
            DateTime CurrentDate = DateTime.Now;
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                List<Milestone> milestones = new List<Milestone>();
                using (var ctx = new CPPDbContext())
                {
                    List<DateTime> Dates = new List<DateTime>();
                    milestones = ctx.Milestone.ToList();
                    List<Milestone> milestonesData = ctx.Milestone.Where(m => m.MilestoneDate != "").ToList();
                    NotificationDays notificationDays = ctx.NotificationDays.Where(n => n.MailService == "milestoneExpire").FirstOrDefault();

                    for (int i = 0; i <= milestonesData.Count; i++)
                    {
                        DateTime mDate = DateTime.Parse(milestonesData[i].MilestoneDate, DateTimeFormatInfo.InvariantInfo);
                        //DateTime mDate = Convert.ToDateTime(milestonesData[i].MilestoneDate);
                        //DateTime mDate = DateTime.ParseExact(milestonesData[i].MilestoneDate, "MM/dd/yyyy", null); ;
                        //int expierDays = Int16.Parse(MilestoneExpireIn);
                        int expierDays = Int16.Parse(notificationDays.Days);
                        DateTime exDate = CurrentDate.AddDays(expierDays);
                        if (mDate.Date == exDate.Date)
                        {
                            var projectID = milestonesData[i].ProgramElementID;
                            ProjectApproversDetails ApproverProjectManager = ctx.ProjectApproversDetails.Where(a => a.ApproverMatrixId == 4 && a.ProjectId == projectID).FirstOrDefault();

                            var projectManagerID = ApproverProjectManager.EmpId;
                            User user = ctx.User.Where(u => u.EmployeeID == projectManagerID).FirstOrDefault();

                            if (user != null && projectManagerID != 10000)
                            {
                                ProgramElement project = ctx.ProgramElement.Where(p => p.ProgramElementID == projectID).FirstOrDefault();
                                Program contract = ctx.Program.Where(c => c.ProgramID == project.ProgramID).FirstOrDefault();



                                WebAPI.Services.MailServices.RemindMilestoneExpire(user.FirstName + " " + user.LastName, milestonesData[i].MilestoneName.ToString(), mDate.ToString("MM-dd-yyyy"), user.Email, project.ProgramElementName.ToString(), contract.ProgramName.ToString());
                                //WebAPI.Services.MailServices.RemindMilestoneExpire(user.FirstName + " " + user.LastName, "Test", "31-03-2022", user.Email, "Test element1", "Test program");
                                //System.Diagnostics.Debug.WriteLine("Send One Mail",mDate.Date);
                            }

                            //for (int j = 0; j < ApproverProjectManager.Count; j++)
                            //{
                            //    if(ApproverProjectManager[j].ProjectId == milestonesData[i].ProgramElementID)
                            //    {
                            //        Console.WriteLine(mDate);

                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}