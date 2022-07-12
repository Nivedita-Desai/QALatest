using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models.Quartz;

namespace WebAPI.Models
{
    public class Scheduler
    {
        private static String mppNoRespondKillerFrequency = ConfigurationManager.AppSettings["MppNoRespondKillerFrequency"]; // ConfigurationManager.AppSettings["GET_INCIDENT_FREQUENCY_IN_SECONDS"];
        private static String exportInsperityFrequency = ConfigurationManager.AppSettings["InsperityExportFrequency"];
        private static String exportInsperityCronExpression = ConfigurationManager.AppSettings["InsperityExportCronExp"];
        private static String exportMailCronExpression = ConfigurationManager.AppSettings["MailExportCronExp"]; // Aditya-Narayan 01032022
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void start()
        {
            try
            {
                int mppNoRespondKillerFrequencyInSecond = Int32.Parse(mppNoRespondKillerFrequency);

                //Construct a scheduler factory
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

                //get a scheduler
                IScheduler scheduler = schedulerFactory.GetScheduler();
                scheduler.Start();


                //MppNoRespondKiller
                IJobDetail job = JobBuilder.Create<MppNoRespondKillerJob>()
                                    .WithIdentity("MppNoRespondKillerJob", "group1")
                                    .Build();

                //MppNoRespondKiller Trigger
                ITrigger trigger = TriggerBuilder.Create()
                                        .WithDailyTimeIntervalSchedule(
                                            s => s.WithIntervalInSeconds(mppNoRespondKillerFrequencyInSecond)
                                        .OnEveryDay()
                                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                                        )
                                        .Build();

                scheduler.ScheduleJob(job, trigger);

                //Insperity
                int exportInsperityFrequencyInSecond = Int32.Parse(exportInsperityFrequency);

                //Construct a scheduler factory
                ISchedulerFactory schedulerInsperityFactory = new StdSchedulerFactory();

                //get a scheduler
                IScheduler schedulerInsperity = schedulerInsperityFactory.GetScheduler();
                schedulerInsperity.Start();


                //MppNoRespondKiller
                IJobDetail jobInsperity = JobBuilder.Create<InsperityExportJob>()
                                    .WithIdentity("exportInsperityFrequencyInSecond", "group1")
                                    .Build();

                //MppNoRespondKiller Trigger
                ITrigger triggerInsperity = TriggerBuilder.Create()
                                         .ForJob(jobInsperity)
            .WithCronSchedule(exportInsperityCronExpression)//1hr//"0 0/1 * * * ?" - 1 min
            .WithIdentity("TestTrigger1")
            .StartNow()
            .Build();
                schedulerInsperity.ScheduleJob(jobInsperity, triggerInsperity);
                schedulerInsperity.Start();


                //-----------------------------------------Aditya-Narayan 01032022-----------------------------------------------------------------//

                //mail scheduler

                ISchedulerFactory schedulerMailFactory = new StdSchedulerFactory();
                //get a scheduler
                IScheduler schedulerMailContract = schedulerMailFactory.GetScheduler();
                schedulerMailContract.Start();

                //Job
                IJobDetail jobMail = JobBuilder.Create<MailExportJob>()
                .WithIdentity("exportMailFrequencyInSecond", "group2")
                .Build();

                //Trigger
                ITrigger triggerMail = TriggerBuilder.Create()
                .ForJob(jobMail)
                .WithCronSchedule(exportMailCronExpression)//1hr//"0 0/1 * * * ?" - 1 min
                .WithIdentity("TestTrigger2")
                .StartNow()
                .Build();
                schedulerMailContract.ScheduleJob(jobMail, triggerMail);
                schedulerMailContract.Start();
                //------------------------------------------------------------------------------------------------------------------------//
            }
            catch (Exception ex)
            {
                logger.Info("ERROR: " + ex.InnerException);
            }
        }
    }
}
