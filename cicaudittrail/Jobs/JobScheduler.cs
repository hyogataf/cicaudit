using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace cicaudittrail.Jobs
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //Les classes ***Job doivent heriter de IJob
            // RequestExecutionJob 
            IJobDetail requestjob = JobBuilder.Create<RequestExecutionJob>().Build();
            //SyncMailJob
            IJobDetail syncmailjob = JobBuilder.Create<SyncMailJob>().Build();

            // RequestJob trigger
            ITrigger requestExecutiontrigger = TriggerBuilder.Create()
                 .WithIdentity("trigger1", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(5)
        //.RepeatForever()
        .WithRepeatCount(0))
    .Build();

            // SyncMailJob trigger
            ITrigger syncmailtrigger = TriggerBuilder.Create()
                 .WithIdentity("trigger2", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(5)
        //.RepeatForever()
        .WithRepeatCount(0))
    .Build();

            // delenchement tâche execution requetes sql requestExecutiontrigger
            //TODO tache chaque matin à 5h
           //  scheduler.ScheduleJob(requestjob, requestExecutiontrigger);

            // delenchement tâche recupération mails syncmailtrigger
            //TODO tache toutes les 15 mn
         //   scheduler.ScheduleJob(syncmailjob, syncmailtrigger);
        }
    }
}