using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using ResoWebApi.Wroker.JobFactory;

namespace ResoWebApi.Wroker
{
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Add scope of Quartz service
        /// </summary>
        /// <param name="services"> Service collection object for add scope of Quartz service </param>
        /// <param name="configuration"> To get configuration from config file </param>
        public static void AddScope(IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q => 
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                q.RegisterWorker(configuration);
                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            });
        }

        /// <summary>
        /// Register all cron jobs 
        /// </summary>
        /// <param name="quartzConfigurator"> To update cron job configuration </param>
        /// <param name="configuration"> To get configuration from config file </param>
        private static void RegisterWorker(this IServiceCollectionQuartzConfigurator quartzConfigurator, IConfiguration configuration)
        {
            quartzConfigurator.AddJobAndTrigger<UpdateProjectDetailToInEight>(configuration);
            quartzConfigurator.AddJobAndTrigger<UpdateTradeCraftInEight>(configuration);
            quartzConfigurator.AddJobAndTrigger<UpdateKeyPayWebhookData>(configuration);
            quartzConfigurator.AddJobAndTrigger<UpdateTimeSheetInKeyPay>(configuration);
            quartzConfigurator.AddJobAndTrigger<UpdateActualCost>(configuration);
        }
    }
}
