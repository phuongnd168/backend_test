using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Quartz.SystemJobs
{
    [DisallowConcurrentExecution]
    public class RefreshJobListTask : IJob
    {
        private readonly ILogger<RefreshJobListTask> _logger;
        private readonly QuartzSettings _quartzSettings;
        public RefreshJobListTask(ILogger<RefreshJobListTask> logger, IOptionsSnapshot<QuartzSettings> quartzSettings)
        {
            _logger = logger;
            _quartzSettings = quartzSettings.Value;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // All current jobs
                var allJobKeys = await context.Scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

                // List job not of group System
                var jobKeys = allJobKeys.Where(m => m.Group != "System").ToList().AsReadOnly();

                // List enable job
                var enableJobsSetting = new List<QuartzJobSetting>();
                if (_quartzSettings.Jobs != null)
                {
                    enableJobsSetting = _quartzSettings.Jobs!.Where(m => !m.Disable.HasValue || !m.Disable.Value).ToList();
                }

                // If not found jobs in settings => remove all jobs
                if (enableJobsSetting.Count == 0)
                {
                    foreach (var jobKey in jobKeys)
                    {
                        await context.Scheduler.PauseJob(jobKey);
                    }
                    await context.Scheduler.DeleteJobs(jobKeys);
                    return;
                }

                // Get job type from JobData
                var jobTypes = context.JobDetail.JobDataMap["ListJobType"] as IEnumerable<Type>;
                if (jobTypes != null && jobTypes.Count() > 0)
                {
                    await UpdateJobFromSettings(context.Scheduler, jobKeys, enableJobsSetting, jobTypes);
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "There is an error when execute job {0}", this.GetType().Name);
            }
        }
        public async Task UpdateJobFromSettings(IScheduler scheduler, IEnumerable<JobKey> jobKeys, IEnumerable<QuartzJobSetting> jobsSetting, IEnumerable<Type> jobTypes)
        {
            // Job in setting
            foreach (var jobSetting in jobsSetting)
            {
                Type jobType;
                try
                {
                    jobType = QuartzUtils.GetJobType(jobSetting, jobTypes);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "There is an error when parse job type from setting of {0}", JsonConvert.SerializeObject(jobSetting));
                    continue;
                }

                // Neu da ton tai job
                var jobKey = QuartzUtils.GetJobKey(jobSetting.Name, jobSetting.Group);
                //var currentJobKey = jobKeys.Where(m => m.Name == jobKey.Name && m.Group == jobKey.Group).FirstOrDefault();
                var currentJobKey = jobKeys.Where(m => m.Equals(jobKey)).FirstOrDefault();
                if (currentJobKey != null)
                {
                    var currentJob = await scheduler.GetJobDetail(currentJobKey);
                    var currentJobTriggers = await scheduler.GetTriggersOfJob(currentJobKey);

                    var isChangedJob = IsChangedJobSetting(currentJob, jobSetting, jobType);
                    var isChangedTriggers = IsChangedTriggerSetting(currentJob.Key, currentJobTriggers, jobSetting.Triggers);

                    // Neu thay doi job hoac trigger => xoa job di va schedule lai
                    if (isChangedJob || isChangedTriggers)
                    {
                        // Should pause job at first
                        await scheduler.PauseJob(currentJob.Key);
                        foreach (var currentJobTrigger in currentJobTriggers)
                        {
                            await scheduler.PauseTrigger(currentJobTrigger.Key);
                        }

                        // Delete job
                        await scheduler.DeleteJob(currentJobKey);

                        // Reschedule job
                        await ScheduleJobFromSetting(scheduler, currentJobKey, jobType, jobSetting);
                    }
                }
                else
                {
                    // Schedule job
                    await ScheduleJobFromSetting(scheduler, jobKey, jobType, jobSetting);
                }
            }

            // Remove current jobs not in settings (but not system job)
            var notSettingJobs = jobKeys.Where(m => !jobsSetting.Select(s => QuartzUtils.GetJobKey(s.Name, s.Group)).Contains(m)).ToList();
            if (notSettingJobs.Count > 0)
            {
                // Pause jobs
                foreach (var key in notSettingJobs)
                {
                    await scheduler.PauseJob(key);
                }

                // Remove jobs
                await scheduler.DeleteJobs(notSettingJobs.AsReadOnly());
            }
        }
        public bool IsChangedJobSetting(IJobDetail jobDetail, QuartzJobSetting jobSetting, Type jobTypeSetting)
        {
            if (jobDetail.JobType != jobTypeSetting)
            {
                return true;
            }
            if (jobDetail.Description != jobSetting.Description)
            {
                return true;
            }
            if (jobDetail.RequestsRecovery != jobSetting.Recovery)
            {
                return true;
            }
            if (jobDetail.Durable != jobSetting.Durable)
            {
                return true;
            }
            if (jobDetail.JobDataMap != null && jobSetting.JobData != null &&
                jobDetail.JobDataMap.Count > 0 && jobSetting.JobData.Count > 0 &&
                !jobDetail.JobDataMap.EqualsAs(jobSetting.JobData))
            {
                return true;
            }
            if ((jobDetail.JobDataMap == null || jobDetail.JobDataMap.Count == 0) &&
                (jobSetting.JobData != null && jobSetting.JobData.Count > 0))
            {
                return true;
            }
            if ((jobDetail.JobDataMap != null && jobDetail.JobDataMap.Count >0) &&
                (jobSetting.JobData == null || jobSetting.JobData.Count == 0))
            {
                return true;
            }
            return false;
        }

        public bool IsChangedTriggerSetting(JobKey jobKey, IEnumerable<ITrigger> triggers, IEnumerable<QuartzJobSettingTrigger> triggersSetting)
        {
            if (triggers != null && triggersSetting != null &&
                triggers.Count() > 0 && triggersSetting.Count() > 0)
            {
                if (triggers.Count() != triggersSetting.Count())
                {
                    return true;
                }
                foreach(var trigger in triggers)
                {
                    var triggerSetting = triggersSetting.Where(m => trigger.Key.Equals(QuartzUtils.GetTriggerKey(jobKey, m.Name))).FirstOrDefault();
                    if (triggerSetting == null)
                    {
                        return true;
                    }
                    if (triggerSetting.Description != trigger.Description)
                    {
                        return true;
                    }
                    if (triggerSetting.JobData != null && trigger.JobDataMap != null &&
                        triggerSetting.JobData.Count > 0 && trigger.JobDataMap.Count > 0 &&
                        !triggerSetting.JobData.EqualsAs(trigger.JobDataMap))
                    {
                        return true;
                    }
                    if ((triggerSetting.JobData == null || triggerSetting.JobData.Count == 0) &&
                        (trigger.JobDataMap != null && trigger.JobDataMap.Count > 0))
                    {
                        return true;
                    }
                    if ((triggerSetting.JobData != null && triggerSetting.JobData.Count > 0) &&
                        (trigger.JobDataMap == null || trigger.JobDataMap.Count == 0))
                    {
                        return true;
                    }
                }
                return false;
            }
            if ((triggers == null || triggers.Count() == 0) &&
                (triggersSetting != null && triggersSetting.Count() > 0))
            {
                return true;
            }
            if ((triggers != null && triggers.Count() > 0) &&
                (triggersSetting == null || triggersSetting.Count() == 0))
            {
                return true;
            }
            
            return false;
        }

        private async Task<bool> ScheduleJobFromSetting(IScheduler scheduler, JobKey jobKey, Type jobTypeSetting, QuartzJobSetting jobSetting)
        {
            try
            {
                var builder = JobBuilder.Create(jobTypeSetting)
                    .WithIdentity(jobKey)
                    .WithDescription(jobSetting.Description)
                    .RequestRecovery(jobSetting.Recovery ?? false)
                    .StoreDurably(jobSetting.Durable ?? false);
                if (jobSetting.JobData != null)
                {
                    builder.SetJobData(jobSetting.JobData);
                }

                var job = builder.Build();
                if (jobSetting.Triggers != null && jobSetting.Triggers.Count > 0)
                {
                    var triggers = new List<ITrigger>();
                    foreach (var triggerSetting in jobSetting.Triggers)
                    {
                        var triggerIdentity = QuartzUtils.GetTriggerKey(jobKey, triggerSetting.Name);
                        var triggerBuilder = TriggerBuilder.Create()
                        .ForJob(job.Key)
                        .WithIdentity(triggerIdentity)
                        .WithDescription(triggerSetting.Description);

                        // Use CronSchedule
                        if (!string.IsNullOrEmpty(triggerSetting.CronSchedule))
                        {
                            triggerBuilder.WithCronSchedule(triggerSetting.CronSchedule);
                        }

                        // If start at and end at
                        if (triggerSetting.StartAt.HasValue)
                        {
                            triggerBuilder.StartAt(triggerSetting.StartAt.Value);
                        }
                        if (triggerSetting.EndAt.HasValue)
                        {
                            triggerBuilder.EndAt(triggerSetting.EndAt.Value);
                        }
                        triggerBuilder.UsingJobData(triggerSetting.JobData);
                        triggers.Add(triggerBuilder.Build());
                    }

                    await scheduler.ScheduleJob(job, triggers.AsReadOnly(), true);
                }
                else
                {
                    await scheduler.AddJob(job, true);
                }
                return true;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There is an exception when schedule job {jobKey.Group}.{jobKey.Name}");
                return false;
            }
        }
    }
}
