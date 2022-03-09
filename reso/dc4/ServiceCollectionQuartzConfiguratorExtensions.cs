using Microsoft.Extensions.Configuration;
using Quartz;
using ResoWebApi.Common;
using System;

namespace ResoWebApi.Wroker.JobFactory
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator quartz,
            IConfiguration config)
            where T : IJob
        {
            try
            {
                var jobName = typeof(T).Name;
                var configKey = $"Quartz:{jobName}";
                var cronSchedule = config[configKey];

                if (cronSchedule.IsNullOrEmpty())
                {
                    throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
                }

                var jobKey = new JobKey(jobName);
                quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

                quartz.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity(jobName + "-trigger")
                    .WithCronSchedule(cronSchedule));
            }
            catch (Exception exception)
            {
                //to do add exception log
            }
        }
    }
}
