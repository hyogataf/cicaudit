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


            // RequestExecutionJob
            IJobDetail requestjob = JobBuilder.Create<RequestExecutionJob>().Build();

            ITrigger requestExecutiontrigger = TriggerBuilder.Create()
                 .WithIdentity("trigger1", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(5)
        //.RepeatForever()
        .WithRepeatCount(0))
    .Build();

           // scheduler.ScheduleJob(requestjob, requestExecutiontrigger);
        }
    }
}