using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TLS.Lib.Job.Quartz.SystemJobs;

namespace TLS.Lib.Job.Quartz
{
    public static class QuartzUtils
    {
        public static void AddRefreshJobList(IServiceCollectionQuartzConfigurator configure, IEnumerable<Type> listJobType, string cronScheduleForRefreshJobs)
        {
            var jobKey = JobKey.Create(typeof(RefreshJobListTask).Name, "System");
            var jobDataMap = new JobDataMap();
            jobDataMap.Add("ListJobType", listJobType);
            configure.AddJob<RefreshJobListTask>(jobKey, jobConfig => {
                jobConfig.WithDescription("Auto refresh jobs from config");
                jobConfig.RequestRecovery(false);
                jobConfig.StoreDurably(false);
                jobConfig.UsingJobData(jobDataMap);
            });
            configure.AddTrigger(triggerConfig => {
                // Trigger for current job
                triggerConfig.ForJob(jobKey);

                // Identity, CronSchedule and Description
                triggerConfig.WithIdentity($"Trg-{jobKey.Name}", jobKey.Group);
                triggerConfig.WithCronSchedule(cronScheduleForRefreshJobs);
                triggerConfig.WithDescription("Trigger for auto refresh jobs from config");
                //triggerConfig.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(1))));
            });
        }
        public static Type GetJobType(QuartzJobSetting jobSetting, IEnumerable<Type> listJobType)
        {
            Type jobType = null;
            if (!string.IsNullOrEmpty(jobSetting.Type))
            {
                var details = jobSetting.Type.Split(',', StringSplitOptions.TrimEntries);
                var goodJobTypes = listJobType.Where(m => string.Equals(m.FullName, details[0], StringComparison.OrdinalIgnoreCase)).ToArray();
                if (goodJobTypes.Length == 1)
                {
                    jobType = goodJobTypes.Single();
                }
                else if (goodJobTypes.Length > 1 && details.Length > 1)
                {
                    jobType = goodJobTypes.Where(m => string.Equals(m.Assembly.GetName().Name, details[1], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }

                // Throw exception if not found job type
                if (jobType == null)
                {
                    throw new Exception($"Can not found Job type from type config [{jobSetting.Type}]");
                }
            }
            else
            {
                jobType = listJobType.Where(m => string.Equals(jobSetting.Name, m.FullName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (jobType == null)
                {
                    jobType = listJobType.Where(m => string.Equals(jobSetting.Name, m.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                }

                // Throw exception if not found job type
                if (jobType == null)
                {
                    throw new Exception($"Can not found Job type from name config [{jobSetting.Name}], Pls given valid Name with your job or set [Type] is full type of your job");
                }
            }
            return jobType;
        }
        public static IEnumerable<Type> GetListJobType(IEnumerable<QuartzJobSetting> jobSettings, IEnumerable<Assembly> assemblies)
        {
            // Get all job type which is implement from IJob
            var listJobType = assemblies.SelectMany(m => m.GetTypes()).Where(p => p.IsClass).Where(p => !p.IsAbstract)
                .Where(p => typeof(IJob).IsAssignableFrom(p));

            // List job type
            var jobTypeList = new List<Type>();
            foreach (var job in jobSettings)
            {
                Type jobType = null;
                if (!string.IsNullOrEmpty(job.Type))
                {
                    var details = job.Type.Split(',', StringSplitOptions.TrimEntries);
                    var goodJobTypes = listJobType.Where(m => string.Equals(m.FullName, details[0], StringComparison.OrdinalIgnoreCase)).ToArray();
                    if (goodJobTypes.Length == 1)
                    {
                        jobType = goodJobTypes.Single();
                    }
                    else if (goodJobTypes.Length > 1 && details.Length > 1)
                    {
                        jobType = goodJobTypes.Where(m => string.Equals(m.Assembly.GetName().Name, details[1], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    }

                    // Throw exception if not found job type
                    if (jobType == null)
                    {
                        throw new Exception($"Can not found Job type from type config [{job.Type}]");
                    }
                }
                else
                {
                    jobType = listJobType.Where(m => string.Equals(job.Name, m.FullName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (jobType == null)
                    {
                        jobType = listJobType.Where(m => string.Equals(job.Name, m.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    }

                    // Throw exception if not found job type
                    if (jobType == null)
                    {
                        throw new Exception($"Can not found Job type from name config [{job.Name}], Pls given valid Name with your job or set [Type] is full type of your job");
                    }
                }
                jobTypeList.Add(jobType);
            }
            return jobTypeList;
        }
        public static JobKey GetJobKey(string jobSettingName, string jobSettingGroup)
        {
            if (!string.IsNullOrEmpty(jobSettingGroup))
            {
                return new JobKey(jobSettingName, jobSettingGroup);
            }
            return new JobKey(jobSettingName, SchedulerConstants.DefaultGroup);
        }
        public static TriggerKey GetTriggerKey(JobKey jobKey, string triggerSettingName)
        {
            var triggerName = string.IsNullOrEmpty(triggerSettingName) ? $"Trg-{jobKey.Name}" : triggerSettingName;
            return new TriggerKey(triggerName, jobKey.Group);
        }

        public static bool EqualsAs(this JobDataMap jobData, JobDataMap other)
        {
            var otherKeys = other.Keys.ToList();
            foreach (var item in jobData)
            {
                if (!other.Contains(item.Key))
                {
                    return false;
                }
                if ((item.Value == null && other[item.Key] != null) ||
                    (item.Value != null && other[item.Key] == null))
                {
                    return false;
                }
                if (item.Value != null && other[item.Key] != null)
                {
                    if (item.Value.GetType() != other[item.Key].GetType())
                    {
                        return false;
                    }
                    var valueEquals = item.Value.Equals(other[item.Key]);
                    if (!valueEquals)
                    {
                        return false;
                    }
                }
                otherKeys.Remove(item.Key);
            }
            if (otherKeys.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
