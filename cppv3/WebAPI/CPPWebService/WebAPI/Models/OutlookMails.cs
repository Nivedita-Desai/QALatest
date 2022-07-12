using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Outlook;
using WebAPI.Services;

namespace WebAPI.Models
{
    public class OutlookEmails
    {

        public string EmailFrom { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public List<string> DocPath { get; set; }




        public static List<OutlookEmails> ReadMailItems()
        {
            Application outlookApplication = null;
            NameSpace outlookNameSpace = null;
            MAPIFolder inboxFolder = null;

            Items mailItems = null;
            List<OutlookEmails> listEmailDetails = new List<OutlookEmails>();
            OutlookEmails emailDetails;

            try
            {
                outlookApplication = new Application();
                outlookNameSpace = outlookApplication.GetNamespace("MAPI");
                inboxFolder = outlookNameSpace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                mailItems = inboxFolder.Items;

                foreach (MailItem item in mailItems)
                {
                    emailDetails = new OutlookEmails();
                    emailDetails.EmailFrom = item.SenderEmailAddress;
                    emailDetails.EmailSubject = item.Subject;
                    emailDetails.EmailBody = item.Body;
                    listEmailDetails.Add(emailDetails);
                    //if (item.SenderEmailAddress == "swapnil@softlabsgroup.com")
                    //{
                    if (item.Attachments.Count > 0)
                    {
                        for (int i = 1; i <= item.Attachments.Count; i++)
                        {
                            item.Attachments[i].SaveAsFile(@"C:\TestFileSave\" + item.Attachments[i].FileName);
                        }
                    }
                    //}

                    ReleaseComObject(item);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ReleaseComObject(mailItems);
                ReleaseComObject(inboxFolder);
                ReleaseComObject(outlookNameSpace);
                ReleaseComObject(outlookApplication);
            }
            return listEmailDetails;
        }



        public static List<string> GetAttachedFiles()
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("GetAttachedFiles");
            string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";
            List<string> files = new List<string>();

            String subject = "CPP - Co-Ad Import Response";
            List<string> to = new List<string>();
            string message = string.Empty;
            string toMailId = ConfigurationManager.AppSettings["Co-Ad_To_MailId"].ToString();
            to.Add(toMailId);

            Application outlookApplication = null;
            NameSpace outlookNameSpace = null;
            MAPIFolder inboxFolder = null;

            Items mailItems = null;
            List<OutlookEmails> listEmailDetails = new List<OutlookEmails>();
            OutlookEmails emailDetails;

            try
            {
                outlookApplication = new Application();
                outlookNameSpace = outlookApplication.GetNamespace("MAPI");
                inboxFolder = outlookNameSpace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                mailItems = inboxFolder.Items;

                string mailSenderUserName = ConfigurationManager.AppSettings["Co-Ad_Sender_User_Name"].ToString();
                for (int j = mailItems.Count; j >= 1; j--)
                {
                    MailItem item = inboxFolder.Items[j];
                    if (item.SenderName == mailSenderUserName)
                    {
                        emailDetails = new OutlookEmails();
                        emailDetails.EmailFrom = item.SenderEmailAddress;
                        emailDetails.EmailSubject = item.Subject;
                        emailDetails.EmailBody = item.Body;

                        List<string> docPath = new List<string>();

                        if (item.Attachments.Count > 0)
                        {
                            logger.Info("attaching files");
                            for (int i = 1; i <= item.Attachments.Count; i++)
                            {
                                string filePath = Path.Combine(workingFolder, item.Attachments[i].FileName);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                item.Attachments[i].SaveAsFile(filePath);
                                docPath.Add(filePath);
                            }
                        }
                        emailDetails.DocPath = docPath;
                        listEmailDetails.Add(emailDetails);
                        break;
                    }
                    else
                    {
                        //message = "Sorry!! No latest mail found for user \" " + mailSenderUserName + " \" from outlook to import.";
                        //MailServices.SendMail(to, "", "", subject, message);
                    }
                    ReleaseComObject(item);
                }
                files = listEmailDetails[0].DocPath;
            }
            catch (System.Exception ex)
            {
                message = "Sorry!! There is an following issue to import Co-Ad file. " + ex.Message.ToString();
                MailServices.SendMail(to, "", "", subject, message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ReleaseComObject(mailItems);
                ReleaseComObject(inboxFolder);
                ReleaseComObject(outlookNameSpace);
                ReleaseComObject(outlookApplication);
            }
            return files;
        }

        public static void ReleaseComObject(object obj)
        {
            if (obj != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
        }

    }
}

//public static List<string> GetAttachedFiles()
//{
//    string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";
//    List<string> files = new List<string>();

//    Application outlookApplication = null;
//    NameSpace outlookNameSpace = null;
//    MAPIFolder inboxFolder = null;

//    Items mailItems = null;
//    List<OutlookEmails> listEmailDetails = new List<OutlookEmails>();
//    OutlookEmails emailDetails;

//    try
//    {
//        outlookApplication = new Application();
//        outlookNameSpace = outlookApplication.GetNamespace("MAPI");
//        inboxFolder = outlookNameSpace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
//        mailItems = inboxFolder.Items;

//        MailItem msgI = mailItems.GetLast();
//        emailDetails = new OutlookEmails();
//        emailDetails.EmailFrom = msgI.SenderEmailAddress;
//        emailDetails.EmailSubject = msgI.Subject;
//        emailDetails.EmailBody = msgI.Body;
//        listEmailDetails.Add(emailDetails);

//        if (msgI.Attachments.Count > 0)
//        {
//            for (int i = 1; i <= msgI.Attachments.Count; i++)
//            {
//                string filePath = Path.Combine(workingFolder,msgI.Attachments[i].FileName);
//                if (File.Exists(filePath))
//                {
//                    File.Delete(filePath);
//                }
//                msgI.Attachments[i].SaveAsFile(filePath);
//                files.Add(filePath);
//            }
//        }

//        ReleaseComObject(msgI);

//    }
//    catch (System.Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }
//    finally
//    {
//        ReleaseComObject(mailItems);
//        ReleaseComObject(inboxFolder);
//        ReleaseComObject(outlookNameSpace);
//        ReleaseComObject(outlookApplication);
//    }
//    return files;
//}