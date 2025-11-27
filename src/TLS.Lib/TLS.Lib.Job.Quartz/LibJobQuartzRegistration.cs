using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using TLS.Core;

namespace TLS.Lib.Job.Quartz
{

    public static class LibJobQuartzRegistration
    {
        private const string DefaultQuartzConfigKey = "Quartz";

        /// <summary>
        /// Add lib AddQuartz use default config section ["Quartz"] with quartz configurator
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <param name="isHostedService">If set true => AddQuartzHostedService to run as service</param>
        /// <param name="startDelay"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobQuartz(this IServiceCollection services, Action<IServiceCollectionQuartzConfigurator> configure, bool isHostedService, TimeSpan? startDelay = null, NameValueCollection properties = null)
        {
            // Use default section
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(DefaultQuartzConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultQuartzConfigKey}] was not found");
            }

            return AddLibJobQuartz(services, section, configure, isHostedService, startDelay, properties);
        }

        /// <summary>
        /// Add lib AddQuartz use special configuration section with quartz configurator
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <param name="configure"></param>
        /// <param name="isHostedService">If set true => AddQuartzHostedService to run as service</param>
        /// <param name="startDelay"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibJobQuartz(this IServiceCollection services, IConfigurationSection configurationSection, Action<IServiceCollectionQuartzConfigurator> configure, bool isHostedService, TimeSpan? startDelay = null, NameValueCollection properties = null)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (configurationSection == null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            // Check param
            if (!configurationSection.Exists())
            {
                throw new Exception($"Configuration section [{configurationSection.Key}] was not found");
            }

            // Configure quartz settings
            services.Configure<QuartzSettings>(configurationSection);

            // Check param
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            // Org configuration
            var orgSection = configurationSection.GetSection("Org");
            if (!orgSection.Exists())
            {
                throw new Exception($"Configuration section [{orgSection.Key}] was not found");
            }
            services.Configure<QuartzOptions>(orgSection);

            // if you are using persistent job store, you might want to alter some options
            /*
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            });
            */

            Action<IServiceCollectionQuartzConfigurator> configureFunc = q =>
            {
                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });
                
                q.Properties.Add("QuartzSettingsKey", configurationSection.Key);

                configure(q);

                var jobTypes = q.Properties.Get("JobTypes");
                if (!string.IsNullOrEmpty(jobTypes))
                {
                    var listJobType = jobTypes.Split('|').Select(m => Type.GetType(m));
                    var cronScheduleForRefreshJobs = configurationSection["CronScheduleForRefreshJobs"];
                    if (string.IsNullOrWhiteSpace(cronScheduleForRefreshJobs))
                    {
                        cronScheduleForRefreshJobs = "35 0/1 * * * ?"; // 1 phut 1 lan vao giay thu 35
                    }
                    QuartzUtils.AddRefreshJobList(q, listJobType, cronScheduleForRefreshJobs);
                }
            };

            // Add the required Quartz.NET services
            if (properties != null)
            {
                services.AddQuartz(properties, configureFunc);
            }
            else
            {
                services.AddQuartz(configureFunc);
            }

            // If use as hosted service (worker serive)
            if (isHostedService)
            {
                services.AddQuartzHostedService(options =>
                {
                    // when shutting down we want jobs to complete gracefully
                    options.WaitForJobsToComplete = true;
                    options.StartDelay = startDelay;
                });
            }

            return services;
        }


        /// <summary>
        /// Add list jobs settings from configuration and bind to list correct jobs (Implement interface Quartz.IJob) form given assemblies
        /// If job config is not valid (not found job type) => throw exception
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="configuration"></param>
        /// <param name="assemblies"></param>
        public static void AddJobsFromSettings(this IServiceCollectionQuartzConfigurator configure, IConfiguration configuration, params Assembly[] assemblies)
        {
            // Check param
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            // Check param
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Check param
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            // Get all job type which is implement from IJob
            var listJobType = assemblies.SelectMany(m => m.GetTypes()).Where(p => p.IsClass).Where(p => !p.IsAbstract)
                .Where(p => typeof(IJob).IsAssignableFrom(p));

            // Save to use in refresh job
            var listJobTypeName = string.Join("|", listJobType.Select(m => $"{m.FullName}, {m.Assembly.FullName.Split(',').First()}"));
            configure.Properties.Add("JobTypes", listJobTypeName);

            // Get Quartz settings
            //var configuration = services.GetConfiguration();
            //var settingKey = configure.Properties.Get("QuartzSettingsKey");
            //var jobConfig = configuration.GetSection(settingKey);
            //services.Configure<QuartzSettings>(jobConfig);
            var settings = configuration.Get<QuartzSettings>();

            // If not found config jobs
            if (settings.Jobs == null || settings.Jobs.Count == 0)
            {
                return;
            }

            // Config all jobs
            var enableJobsSetting = settings.Jobs.Where(m => !m.Disable.HasValue || !m.Disable.Value).ToList();
            foreach (var jobSetting in enableJobsSetting)
            {
                // Abort if job is disabled
                if (jobSetting.Disable.HasValue && jobSetting.Disable.Value)
                {
                    continue;
                }

                // Get job type from setting
                Type jobType = QuartzUtils.GetJobType(jobSetting, listJobType);

                // Job name
                var jobName = jobType.Name;
                var jobKey = QuartzUtils.GetJobKey(jobSetting.Name, jobSetting.Group);
                if (!string.IsNullOrEmpty(jobSetting.Group))
                {
                    jobKey.Group = jobSetting.Group;
                }

                // Config job
                configure.AddJob(jobType, jobKey, jobConfig =>
                {
                    // Description
                    jobConfig.WithDescription(jobSetting.Description);
                    jobConfig.RequestRecovery(jobSetting.Recovery ?? false);
                    jobConfig.StoreDurably(jobSetting.Durable ?? false);
                    if (jobSetting.JobData != null)
                    {
                        jobConfig.SetJobData(jobSetting.JobData);
                    }
                });

                // Config triggers
                if (jobSetting.Triggers != null)
                {
                    foreach (var trigger in jobSetting.Triggers)
                    {
                        configure.AddTrigger(triggerConfig => {
                            var triggerIdentity = QuartzUtils.GetTriggerKey(jobKey, trigger.Name);
                            triggerConfig.ForJob(jobKey);
                            triggerConfig.WithIdentity(triggerIdentity);
                            triggerConfig.WithDescription(trigger.Description);

                            // Use CronSchedule
                            if (!string.IsNullOrEmpty(trigger.CronSchedule))
                            {
                                triggerConfig.WithCronSchedule(trigger.CronSchedule);
                            }    

                            // If start at and end at
                            if (trigger.StartAt.HasValue)
                            {
                                triggerConfig.StartAt(trigger.StartAt.Value);
                            }
                            if (trigger.EndAt.HasValue)
                            {
                                triggerConfig.EndAt(trigger.EndAt.Value);
                            }
                            triggerConfig.UsingJobData(trigger.JobData);
                        });
                    }    
                }
            }
        }
    }
}
