using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models;
using System.Linq;

namespace WebAPI.Services
{
    public class MailServices
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void SendApprovalEmail(string RequestingUser, string TargetedUser, string Role, string TrendId, string ProjectId, string toEmail, string sub = "CPP - APPROVAL REQUEST NOTIFICATION")
        {

            int ProjectID = Convert.ToInt32(ProjectId);
            Trend trend = Models.Trend.getTrendById(TrendId, ProjectID);
            List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
            Project project = projects.Find(e => e.ProjectID == ProjectID);
            List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
            ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
            List<Program> ps = Models.Program.getProgram("null", "null", "null");
            Program p = ps.Find(e => e.ProgramID == project.ProgramID);
            List<User> users = Models.User.getUser();
            List<string> to = new List<string>();
            foreach (User user in users)

            {
                //if (trend.PostTrendCost != null && user.Threshold != "" && user.Threshold != null && trend.PostTrendCost != "")
                //if (Convert.ToDouble(trend.PostTrendCost) < Convert.ToDouble(user.Threshold))
                //to.Add(user.Email);

            }
            to.Add(toEmail); //luan here
            string localHostUrl = "http://localhost:54364/index.html#/app/cost-gantt/";
            string routeInfo = ConfigurationManager.AppSettings["RouteInfo"];
            string testServerUrl = routeInfo + "/app/cost-gantt/";

            String subject = sub; // Swapnil 18-09-2020 
            string message = "<html>Hello " + TargetedUser + " , <br><br> <strong>" + RequestingUser + "</strong> requested for an approval of Element:<strong> " +
                project.ProjectName + "</strong> Project :<strong>" + pe.ProgramElementName + "</strong> Contract :<strong>" + p.ProgramName + "</strong> ."

                + "<br><br>Access directly using the following link - "
                + "<br><a href='" + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID + "' target='_blank' >"
                + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID + "</a>"
                + "<br><br>Please log in CPP and respond accordingly.<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";

            var sendMailThread = new Task(() => {

                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }


        public static void SendPOApprovalEmail(string PONumber, string RequestingUser, string ProjectId, string sub = "CPP - PO APPROVAL REQUEST NOTIFICATION")
        {

            string TargetedUser = "";
            int ProjectID = Convert.ToInt32(ProjectId);
            List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
            Project project = projects.Find(e => e.ProjectID == ProjectID);
            List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
            ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
            List<Program> ps = Models.Program.getProgram("null", "null", "null");
            Program p = ps.Find(e => e.ProgramID == project.ProgramID);
            List<User> users = Models.User.getUser().Where(x => x.Role == "Accounting").ToList();
            List<string> to = new List<string>();
            foreach (User user in users)

            {
                //if (trend.PostTrendCost != null && user.Threshold != "" && user.Threshold != null && trend.PostTrendCost != "")
                //if (Convert.ToDouble(trend.PostTrendCost) < Convert.ToDouble(user.Threshold))
                if (user.Email != "N/A")
                {
                    TargetedUser = user.FirstName + " " + user.LastName;
                    to.Add(user.Email);
                    //string localHostUrl = "http://localhost:54364/index.html#/app/po-Approval/" + PONumber;
                    string routeInfo = ConfigurationManager.AppSettings["RouteInfo"];
                    string testServerUrl = routeInfo + "/app/po-Approval/" + PONumber;

                    String subject = sub; // Swapnil 18-09-2020 
                    string message = "<html>Hello " + TargetedUser + " , <br><br> <strong>" + RequestingUser + "</strong> requested for an approval of PO :<strong>" + PONumber + "</strong> Element :<strong> " +
                        project.ProjectName + "</strong> Project :<strong>" + pe.ProgramElementName + "</strong> Contract :<strong>" + p.ProgramName + "</strong> ."

                        + "<br><br>Access directly using the following link - "
                        + "<br><a href='" + testServerUrl + "' target='_blank' >" + testServerUrl + "</a>"
                        + "<br><br>Please log in CPP and respond accordingly.<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";

                    //var sendMailThread = new Task(() => {

                    SendMail(to, "", "", subject, message);
                    to.Remove(user.Email);
                    //});
                    //sendMailThread.Start();

                }


            }
            //to.Add(toEmail); //luan here

        }

        public static void SendPOStatusUpdateEmail(string PONumber, string TargetedUser, string RequestingUser, string EMail, string ProjectId, string Status, string reason, string sub = "CPP - PO STATUS")
        {

            //string RequestingUser = "";
            int ProjectID = Convert.ToInt32(ProjectId);
            List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
            Project project = projects.Find(e => e.ProjectID == ProjectID);
            List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
            ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
            List<Program> ps = Models.Program.getProgram("null", "null", "null");
            Program p = ps.Find(e => e.ProgramID == project.ProgramID);
            //List<User> users = Models.User.getUser().Where(x => x.Role == "Accounting").ToList();
            List<string> to = new List<string>();

            //if (trend.PostTrendCost != null && user.Threshold != "" && user.Threshold != null && trend.PostTrendCost != "")
            //if (Convert.ToDouble(trend.PostTrendCost) < Convert.ToDouble(user.Threshold))
            if (EMail != "N/A")
            {
                //TargetedUser = user.FirstName + " " + user.LastName;
                to.Add(EMail);
                //string localHostUrl = "http://localhost:54364/index.html#/app/cost-gantt/";
                string routeInfo = ConfigurationManager.AppSettings["RouteInfo"];
                //string testServerUrl = routeInfo + "/app/po-Approval";

                String subject = sub; // Swapnil 18-09-2020 

                if (Status == "Approved")
                    subject = "CPP - PO :" + PONumber + " Approved";
                else
                    subject = "CPP - PO :" + PONumber + " Rejected";


                string message = "";

                if (Status == "Approved")
                {

                    message = "<html>Hello " + RequestingUser + " , <br><br> <strong>" + TargetedUser + "</strong> Approved PO :<strong>" + PONumber + "</strong> Element :<strong> " +
                    project.ProjectName + "</strong> Project :<strong>" + pe.ProgramElementName + "</strong> Contract :<strong>" + p.ProgramName + "</strong> ."

                    // + "<br><br>Access directly using the following link - "
                    // + "<br><a href='" + testServerUrl + "' target='_blank' >" + testServerUrl + "</a>"
                    //+ "<br><br>Please log in CPP and respond accordingly."
                    + "<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
                }
                else
                {

                    message = "<html>Hello " + RequestingUser + " , <br><br> <strong>" + TargetedUser + "</strong> Rejected PO :<strong>" + PONumber + "</strong> Element :<strong> " +
                    project.ProjectName + "</strong> Project :<strong>" + pe.ProgramElementName + "</strong> Contract :<strong>" + p.ProgramName + "</strong> ."
                     + "<br><br>Due to - <strong>" + reason + "</strong>"
                    // + "<br><a href='" + testServerUrl + "' target='_blank' >" + testServerUrl + "</a>"
                    + "<br><br>Please log in CPP and respond accordingly.<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
                }
                var sendMailThread = new Task(() => {

                    SendMail(to, "", "", subject, message);
                });
                sendMailThread.Start();
                //to = new List<string>();
            }



            //to.Add(toEmail); //luan here

        }

        public static void ResendSendApprovalEmail(string RequestingUser, string TargetedUser, string Role, string TrendId, string ProjectId, string toEmail)
        {

            int ProjectID = Convert.ToInt32(ProjectId);
            Trend trend = Models.Trend.getTrendById(TrendId, ProjectID);
            List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
            Project project = projects.Find(e => e.ProjectID == ProjectID);
            List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
            ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
            List<Program> ps = Models.Program.getProgram("null", "null", "null");
            Program p = ps.Find(e => e.ProgramID == project.ProgramID);
            List<User> users = Models.User.getUser();
            List<string> to = new List<string>();
            foreach (User user in users)

            {
                //if (trend.PostTrendCost != null && user.Threshold != "" && user.Threshold != null && trend.PostTrendCost != "")
                //if (Convert.ToDouble(trend.PostTrendCost) < Convert.ToDouble(user.Threshold))
                //to.Add(user.Email);

            }
            to.Add(toEmail); //luan here
            string localHostUrl = "http://localhost:54364/index.html#/app/cost-gantt/";
            string routeInfo = ConfigurationManager.AppSettings["RouteInfo"];
            string testServerUrl = routeInfo + "/app/cost-gantt/";

            String subject = "CPP - APPROVAL REQUEST NOTIFICATION";
            string message = "<html>Hello " + TargetedUser + " , <br><br> <strong>" + RequestingUser + "</strong> has requested for an approval of: " +
                               "<br>Element:<strong> " +
                             project.ProjectName + "</strong> Project :<strong>" + pe.ProgramElementName + "</strong> Contract :<strong>" + p.ProgramName + "</strong> ."
                              + "<br><br>You can Access directly using the following link - "
                              // + "<br><a href='" + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID + "' />"
                              + "<br><a href='" + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID + "' target='_blank' >"
                              + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID + "</a>"
                            + "<br><br>Please log in CPP and respond accordingly. <br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
            //string message = "<html>Hello " + TargetedUser + " , " +
            //    //"<br><br> <strong>" + RequestingUser + "</strong> requested for an approval of Element:<strong> " +
            //    project.ProjectName + "</strong> Project :<strong>" + pe.ProgramElementName + "</strong> Contract :<strong>" + p.ProgramName + "</strong> ."

            //    + "<br><br>Access directly using the following Resend link - " + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID

            //    + "<br><br>Please log in CPP and respond accordingly. \n \n";
            var sendMailThread = new Task(() =>
            {

                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        //Email for the approve button
        public static void EmailToGetApproved(String UserId, string Role, string TrendId, string ProjectId)
        {

            int ProjectID = Convert.ToInt32(ProjectId);
            Trend trend = Models.Trend.getTrendById(TrendId, ProjectID);
            List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
            Project project = projects.Find(e => e.ProjectID == ProjectID);
            List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
            ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
            List<Program> ps = Models.Program.getProgram("null", "null", "null");
            Program p = ps.Find(e => e.ProgramID == project.ProgramID);
            List<User> users = Models.User.getUser();
            List<string> to = new List<string>();
            foreach (User user in users)

            {
                //if (trend.PostTrendCost != null && user.Threshold != "" && user.Threshold != null && trend.PostTrendCost != "")
                //if (Convert.ToDouble(trend.PostTrendCost) < Convert.ToDouble(user.Threshold))
                //to.Add(user.Email);

            }
            to.Add("lkhong@birdi-inc.com"); //luan here

            string localHostUrl = "http://localhost:54364/index.html#/app/cost-gantt/";
            string testServerUrl = "http://birdi-dev02:5555/cpp/#/app/cost-gantt/";

            String subject = "CPP - APPROVAL REQUEST NOTIFICATION";
            string message = "<html>Hello, <br><br> <strong>" + UserId + "</strong> requested for an approval for the baseline of Project:<strong> " +
                project.ProjectName + "</strong> Program Element :<strong>" + pe.ProgramElementName + "</strong> Program :<strong>" + p.ProgramName + "</strong> ."

                + "<br><br>Access directly using the following link - " + testServerUrl + project.ProjectID + "/" + trend.TrendNumber + "/" + p.OrganizationID

                + "<br><br>Please log in CPP and respond accordingly. <br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
            var sendMailThread = new Task(() => {

                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        public static void EmailToGetTrendApprovedByCode(String UserId, string TrendId, string ProjectId, string UniqueCode, bool IsResend)
        {
            try
            {
                int ProjectID = Convert.ToInt32(ProjectId);
                //  Trend trend = Models.Trend.getTrendById(TrendId, ProjectID);
                List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
                Project project = projects.Find(e => e.ProjectID == ProjectID);
                List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
                ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
                List<Program> ps = Models.Program.getProgram("null", "null", "null");
                Program p = ps.Find(e => e.ProgramID == project.ProgramID);
                List<User> users = Models.User.getUser();

                var USerName = Models.User.getUserByUserID(UserId);

                //----------------------Swapnil 17-09-2020--------------------------------------------------------------
                var ctx = new CPPDbContext();
                MFAConfiguration mfaDet = ctx.MFAConfigurationDetails.Where(u => u.MFAConfigID == 1).FirstOrDefault();
                //---------------------------------------------------------------------------------------------------------

                //var ctx = new CPPDbContext();
                //var TrendDescp = ctx.Trend.Where(u => u.TrendID == Convert.ToInt16(TrendId) && u.ProjectID == Convert.ToInt16(ProjectId)).FirstOrDefault(); TrendDescp

                List<string> to = new List<string>();
                WebAPI.Helper.RegexUtilities regex = new WebAPI.Helper.RegexUtilities();
                //if (regex.IsValidEmail("pritesh_bbari84@yahoo.in"))
                //    to.Add("pritesh_bbari84@yahoo.in"); 

                if (regex.IsValidEmail(USerName.Email))
                    to.Add(USerName.Email);


                String subject = "CPP - TREND APPROVAL CODE";
                //string message = "<html>Hello,"  +

                //   " <br><br>Your Approval code:" +
                //    " <br><strong>" + UniqueCode + "</strong>" +
                //    "<br>Valid for: 30 Minutes" +
                //  "<br><br>Regards,";
                string message = "<html>Hello " + USerName.FirstName + ", <br><br>Here is the one-time trend approval code for Project:<strong> " +
                    project.ProjectName + "</strong> Program Element :<strong>" + pe.ProgramElementName + "</strong> Program :<strong>" + p.ProgramName + "</strong> ." +
                    " <br><br>Your Approval code:" +
                     "<strong>" + UniqueCode + "</strong>";
                if (!IsResend)
                {
                    //message = message + "<br>Valid for: 30 Minutes";
                    message = message + "<br>Valid for: " + "<strong>" + mfaDet.ApprovalCodeValidity + " Minutes </strong>"; // Swapnil 17-09-2020
                }

                message = message + "<br><br><strong>Regards,<br><br> CPP Team</strong>";

                SendMail(to, "", "", subject, message);
            }
            catch (Exception ex)
            {
                logger.Info("SendForgotPassword - " + ex);
            }
        }

        public static void SendUserRegistration(String fullName, string userID, string userEmail)
        {


            SendRegistrationToAdmin(fullName, userID);
            List<string> to = new List<string>();
            WebAPI.Helper.RegexUtilities regex = new WebAPI.Helper.RegexUtilities();
            if (regex.IsValidEmail(userEmail))
                to.Add(userEmail);

            String subject = "CPP - USER CONFIRMATION";
            string message = "<html>Hello " + fullName + "," +
                "<br><br>Thank you for your registration! We will validate your information and respond back to you to confirm your registration."
                + "<br><br> Regards,";

            var sendMailThread = new Task(() => {

                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        public static void SendForgotPassword(User user, String token)
        {
            try
            {
                var url = user.routeInfo + "/update-password/?token=" + token;
                //SendRegistrationToAdmin(fullName, userID);
                List<string> to = new List<string>();
                WebAPI.Helper.RegexUtilities regex = new WebAPI.Helper.RegexUtilities();
                if (regex.IsValidEmail(user.Email))
                    to.Add(user.Email);

                String subject = "CPP - Password Recovery";
                string message = "<html>Hello " + user.FirstName + ",<br>";
                message += "<br>";
                message += "Per your request, you may reset your CPP password within 24 hours using the link below.<br>";
                message += "Your CPP username remains the same and is <strong>" + user.UserID + "</strong>. ";
                message += "If you did not make this request, please email us immediately at cppsupport@birdi-inc.com with the subject line 'Invalid Password Change Request'.<br>";
                message += "<br>";
                message += "<a href='" + url + "'>" + url + "</a><br>";
                message += "<br><br>";
                message += "Regards,";

                // var sendMailThread = new Task(() =>
                //{

                SendMail(to, "", "", subject, message);
                //});
                //sendMailThread.Start();
            }
            catch (Exception ex)
            {
                logger.Info("SendForgotPassword - " + ex);
            }
        }


        private static void SendRegistrationToAdmin(String fullName, string userID)
        {

            var ctx = new CPPDbContext();
            //   List<User> adminList = Models.User.getUser("null", "null", "null", "Admin");
            List<User> adminList = getUser("null", "null", "null", "Admin");

            List<string> to = new List<string>();
            WebAPI.Helper.RegexUtilities regex = new WebAPI.Helper.RegexUtilities();
            foreach (User user in adminList)

            {
                if (regex.IsValidEmail(user.Email))

                    to.Add(user.Email);

            }
            String subject = "CPP - USER REGISTRATION NOTIFICATION";
            string message = "<html>Hello Administrators," +
                "<br><br> " + fullName + " just registered ! Please log in CPP to assign a role for the new user."
                + "<br><br> Regards,";
            var sendMailThread = new Task(() => {

                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }
        public static void SendUserActivation(String UserId, string Role, string TrendId, string ProjectId)
        {

            int ProjectID = Convert.ToInt32(ProjectId);
            Trend trend = Models.Trend.getTrendById(TrendId, ProjectID);
            List<Project> projects = Models.Project.getProject("null", "null", ProjectId, "null");
            Project project = projects.Find(e => e.ProjectID == ProjectID);
            List<ProgramElement> pes = Models.ProgramElement.getProgramElement("null", "null", "null");
            ProgramElement pe = pes.Find(e => e.ProgramElementID == project.ProgramElementID);
            List<Program> ps = Models.Program.getProgram("null", "null", "null");
            Program p = ps.Find(e => e.ProgramID == project.ProgramID);
            List<User> users = Models.User.getUser();
            List<string> to = new List<string>();
            foreach (User user in users)

            {
                if (trend.PostTrendCost != null && user.Threshold != "" && user.Threshold != null && trend.PostTrendCost != "")
                    if (Convert.ToInt32(trend.PostTrendCost) < Convert.ToInt32(user.Threshold))
                        to.Add(user.Email);
            }
            String subject = "CPP - APPROVAL REQUEST NOTIFICATION";
            string message = "<html>Hello All, <br><br> <strong>" + UserId + "</strong> request for an approval for Trend <strong>" + trend.TrendNumber + "</strong> from Project :<strong> " +
             project.ProjectName + "</strong> Program Element :<strong>" + pe.ProgramElementName + "</strong> Program :<strong>" + p.ProgramName + "</strong> ."

                + "Please log in CPP and respond accordingly. <br><br> Regards,";
            // var sendMailThread = new Task(() => {

            SendMail(to, "", "", subject, message);
            //  });
            //sendMailThread.Start();
        }

        // Key Milestone Reminder Mail to Project Manager // Narayan 01-03-2022
        public static void RemindMilestoneExpire(string TargetedUser, string MilestoneName, string ExpiryDate, string toEmail, string ProjectName, string ContractName, string sub = "CPP - KEY PROJECT MILESTONES EXPIRY NOTICE.")
        {
            List<string> to = new List<string>();
            to.Add(toEmail);
            String subject = sub;
            string message = "<html>Hello " + TargetedUser + " , <br><br> <strong>" + MilestoneName + "</strong> Will Expire On : <strong>" + ExpiryDate + "</strong>  Under Project : <strong>" + ProjectName + "</strong> Contract : <strong>" + ContractName + "</strong> ."
                + "<br><br>Please log in CPP and respond accordingly.<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
            var sendMailThread = new Task(() =>
            {
                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        //Contract Expiry Mail // Aditya 01-03-2022
        public static void SendReminderEmailContractExp(string toEmail, string TargetedUser, string ContractName, string ContractNumber, string CurrentEndDate, string sub = "CPP Contract Expiry Notice")
        {
            List<string> to = new List<string>();
            //string mail = "aditya.kamat@softlabsgroup.com";
            to.Add(toEmail); //actual mails go here
           //to.Add(mail); //use this for testing
            String subject = sub;
            string message = "<html>Hello " + TargetedUser + " , <br><br> <strong>" + ContractName + "</strong>, with contract no <strong>" + ContractNumber + "</strong> will expire on <strong>" + CurrentEndDate + "</strong>.<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
            System.Diagnostics.Debug.WriteLine(message);
            var sendMailThread = new Task(() => {

                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        // Narayan - Contract Cost Change Mail 
        public static void NotifyCostChangeToManager(string TargetedUser, string toEmail, string ContractNumber, string ContractName, string ModifiedValue, string CurruntValue, string sub = "CPP - Contract Value Change.")
        {
            List<string> to = new List<string>();
            to.Add(toEmail);
            String subject = sub;
            string message = "<html>Hello " + TargetedUser + " , <br><br> Contract Ammount for Contract - <strong>" + ContractName + "</strong>, with contract no <strong>" + ContractNumber + "</strong> has been modified by <strong>$" + ModifiedValue + "</strong> ."
                + "<br> Current Contract Value : <strong>$" + CurruntValue + "</strong>"
                + "<br><br>Please log in CPP and respond accordingly.<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
            var sendMailThread = new Task(() =>
            {
                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        // Narayan - Contract Project Manager for Contract Created Mail 
        public static void ContractProjectManagerMail(string toEmail, string ContractNumber, string ContractName)
        {
            List<string> to = new List<string>();
            to.Add(toEmail);
            String subject = "Contract Setup Notification - " + ContractName + " & " + ContractNumber + "";
            string message = "<html>Hello, <br><br>Contract - <strong>" + ContractName + "</strong>, with contract no <strong>" + ContractNumber + "</strong> has been created."
                + "<br><br><strong>Regards,<br><br> CPP Team</strong><br><br>";
            var sendMailThread = new Task(() =>
            {
                SendMail(to, "", "", subject, message);
            });
            sendMailThread.Start();
        }

        public static void SendMail(List<string> to, string cc, string bcc, string subject, string message)
        {
            //Reading sender Email credential from web.config file  
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
            try
            {
                string stmpHost = ConfigurationManager.AppSettings["Host"].ToString();
                string FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();

                //creating the object of MailMessage  

                mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                mailMessage.IsBodyHtml = true;

                List<string> toList = to;
                WebAPI.Helper.RegexUtilities regex = new WebAPI.Helper.RegexUtilities();
                foreach (string toEmail in toList)
                {
                    if (regex.IsValidEmail(toEmail))
                        mailMessage.To.Add(new MailAddress(toEmail)); //adding multiple TO Email Id  
                }


                string[] ccList = cc.Split(',');
                regex = new WebAPI.Helper.RegexUtilities();
                foreach (string ccEmail in ccList)
                {
                    if (regex.IsValidEmail(ccEmail))

                        mailMessage.CC.Add(new MailAddress(ccEmail)); //Adding Multiple CC email Id  
                }

                string[] bccList = bcc.Split(',');
                regex = new WebAPI.Helper.RegexUtilities();

                foreach (string bccEmail in bccList)
                {
                    if (regex.IsValidEmail(bccEmail))
                        mailMessage.Bcc.Add(new MailAddress(bccEmail)); //Adding Multiple BCC email Id  
                }


                smtp.Host = stmpHost;              //host of emailaddress for example smtp.gmail.com etc  

                //network and security related credentials  

                //using (MailMessage mail = new MailMessage())
                //{
                //  mail.From = new MailAddress("email@gmail.com");
                //mail.To.Add("somebody@domain.com");
                //      mail.Subject = "Hello World";
                //    mail.Body = "<h1>Hello</h1>";
                //  mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                //      using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //    {
                //     smtp.Credentials = new NetworkCredential("email@gmail.com", "password");
                //   smtp.EnableSsl = true;
                // smtp.Send(mail);
                //}
                //}

                //using (SmtpClient smt = new SmtpClient("smtp.gmail.com", 587))
                // {
                //  smt.UseDefaultCredentials = false;
                //  smt.Credentials = new NetworkCredential("email@gmail.com", "password");
                //  smt.EnableSsl = true;
                // smt.Send(mailMessage);
                // }
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
               // NetworkCredential NetworkCred = new NetworkCredential(mailMessage.From.Address, password);
               // NetworkCred.UserName = mailMessage.From.Address;
                //NetworkCred.Password = password;
                smtp.Credentials = new NetworkCredential(mailMessage.From.Address, password); ;
                smtp.Timeout = 1000000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = NetworkCred;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                if (!(mailMessage.To.Count == 0 && mailMessage.Bcc.Count == 0 && mailMessage.CC.Count == 0))
                    smtp.Send(mailMessage); //sending Email  
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                WebAPI.Models.Logger.LogExceptions("MailServices", "sendApprovalEmail", ex.Message, "", Models.Logger.logLevel.Exception);
                logger.Info("SendMail - " + ex);
            }
            finally
            {
                mailMessage = null;
                smtp = null;

            }
        }

        public static void SendMailWithAttachment(List<string> to, string cc, string bcc, string subject, string message, string AttachmentFiles = null)
        {
            //Reading sender Email credential from web.config file  
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
            try
            {
                string stmpHost = ConfigurationManager.AppSettings["Host"].ToString();
                string FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();

                //creating the object of MailMessage  

                mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                mailMessage.IsBodyHtml = true;

                List<string> toList = to;
                WebAPI.Helper.RegexUtilities regex = new WebAPI.Helper.RegexUtilities();
                foreach (string toEmail in toList)
                {
                    if (regex.IsValidEmail(toEmail))
                        mailMessage.To.Add(new MailAddress(toEmail)); //adding multiple TO Email Id  
                }


                string[] ccList = cc.Split(',');
                regex = new WebAPI.Helper.RegexUtilities();
                foreach (string ccEmail in ccList)
                {
                    if (regex.IsValidEmail(ccEmail))

                        mailMessage.CC.Add(new MailAddress(ccEmail)); //Adding Multiple CC email Id  
                }

                string[] bccList = bcc.Split(',');
                regex = new WebAPI.Helper.RegexUtilities();

                foreach (string bccEmail in bccList)
                {
                    if (regex.IsValidEmail(bccEmail))
                        mailMessage.Bcc.Add(new MailAddress(bccEmail)); //Adding Multiple BCC email Id  
                }

                if (!string.IsNullOrEmpty(AttachmentFiles))
                {
                    foreach (string a in AttachmentFiles.Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                            Attachment att = new Attachment(a);
                            mailMessage.Attachments.Add(att);
                        }
                    }
                }

                smtp.Host = stmpHost;              //host of emailaddress for example smtp.gmail.com etc  

                //network and security related credentials  

                //using (MailMessage mail = new MailMessage())
                //{
                //  mail.From = new MailAddress("email@gmail.com");
                //mail.To.Add("somebody@domain.com");
                //      mail.Subject = "Hello World";
                //    mail.Body = "<h1>Hello</h1>";
                //  mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                //      using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                //    {
                //     smtp.Credentials = new NetworkCredential("email@gmail.com", "password");
                //   smtp.EnableSsl = true;
                // smtp.Send(mail);
                //}
                //}

                //using (SmtpClient smt = new SmtpClient("smtp.gmail.com", 587))
                // {
                //  smt.UseDefaultCredentials = false;
                //  smt.Credentials = new NetworkCredential("email@gmail.com", "password");
                //  smt.EnableSsl = true;
                // smt.Send(mailMessage);
                // }
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = mailMessage.From.Address;
                NetworkCred.Password = password;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Timeout = 1000000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = NetworkCred;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                if (!(mailMessage.To.Count == 0 && mailMessage.Bcc.Count == 0 && mailMessage.CC.Count == 0))
                    smtp.Send(mailMessage); //sending Email  
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                WebAPI.Models.Logger.LogExceptions("MailServices", "sendApprovalEmail", ex.Message, "", Models.Logger.logLevel.Exception);
                logger.Info("SendMail - " + ex);
            }
            finally
            {
                mailMessage = null;
                smtp = null;

            }
        }

        public static List<User> getUser(String UserID, String FirstName, String LastName, String Role)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<User> UserList = new List<User>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                //Nivedita 22-03-2022
                String query = "SELECT * FROM user where Id in (";
                query += " select UserID from user_role_relation where UserRoleId=(select Id from user_roles WHERE Role = '" + Role + "'))";
                //String query = "SELECT * FROM user";
                //query += " WHERE Role = '" + Role + "'";
                ////   String query = "SELECT u.Role, u.FirstName, u.MiddleName, u.LastName, u.Email, u.UserID, u.Id,a.Cost, '' as loginPassword FROM user u left outer join approval_matrix  a on u.Role = a.Role";
                MySqlCommand command = new MySqlCommand(query, conn);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //User returnedUser = new User();
                        //returnedUser.FullName = reader.GetValue(0).ToString().Trim();
                        //returnedUser.Role = reader.GetValue(1).ToString().Trim();
                        //UserList.Add(returnedUser);
                        User RetreivedUser = new User(
                                                    );
                        RetreivedUser.Role = Role;
                        //RetreivedUser.Role = reader.GetValue(7).ToString().Trim();
                        RetreivedUser.Email = reader.GetValue(8).ToString().Trim();
                        RetreivedUser.Threshold = Role;
                        //RetreivedUser.Threshold = reader.GetValue(7).ToString().Trim();
                        UserList.Add(RetreivedUser);
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return UserList;
        }
    }
}