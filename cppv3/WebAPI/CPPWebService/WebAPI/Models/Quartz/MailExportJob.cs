using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quartz;
using WebAPI.Controllers;

namespace WebAPI.Models.Quartz
{
    public class MailExportJob : IJob
    {
        //readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ContractExpiredMailCtrl contractExpiredMailCtrl = new ContractExpiredMailCtrl(); // Aditya 01032022
        RequestMilestoneExpireNotifiyController requestMilestoneExpireNotifiy = new RequestMilestoneExpireNotifiyController(); //Narayan 01032022
        public void Execute(IJobExecutionContext context)
        {
            try
            {

                contractExpiredMailCtrl.Get(); // Aditya 01032022
                requestMilestoneExpireNotifiy.GetExpierdMilestone(); //Narayan 01032022

            }
            catch (Exception ex)
            {
                // logger.Info(ex);
            }
        }



    }
}