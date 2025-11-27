using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Quartz
{
    public class QuartzJobSettingTrigger
    {
        public string Name { get; set; }
        public string CronSchedule { get; set; }
        public string Description { get; set; }
        public DateTime? StartAfter { get; set; }
        public TimeSpan? StartAfterTime { get; set; }
        public DateTime? EndBefore { get; set; }
        public TimeSpan? EndBeforeTime { get; set; }
        public DateTimeOffset? StartAt
        {
            get
            {
                if (StartAfter.HasValue)
                {
                    DateTimeOffset startAt = DateTime.SpecifyKind(StartAfter.Value, DateTimeKind.Utc);
                    return DateBuilder.EvenSecondDate(startAt);
                }
                if (StartAfterTime.HasValue)
                {
                    var startAt = DateTimeOffset.UtcNow.Add(StartAfterTime.Value);
                    return DateBuilder.EvenSecondDate(startAt);
                }
                return null;
            }
        }
        public DateTimeOffset? EndAt
        {
            get
            {
                if (EndBefore.HasValue)
                {
                    DateTimeOffset endAt = DateTime.SpecifyKind(EndBefore.Value, DateTimeKind.Utc);
                    return DateBuilder.EvenSecondDate(endAt);
                }
                if (EndBeforeTime.HasValue)
                {
                    var endAt = DateTimeOffset.UtcNow.Add(EndBeforeTime.Value);
                    return DateBuilder.EvenSecondDate(endAt);
                }
                return null;
            }
        }
        public JobDataMap JobData
        {
            get
            {
                var dataMap = new JobDataMap();
                if (!string.IsNullOrEmpty(CronSchedule))
                {
                    dataMap.Add("CronSchedule", CronSchedule);
                }
                if (StartAfter.HasValue)
                {
                    dataMap.Add("StartAfter", StartAfter.Value);
                }
                if (StartAfterTime.HasValue)
                {
                    dataMap.Add("StartAfterTime", StartAfterTime.Value);
                }
                if (EndBefore.HasValue)
                {
                    dataMap.Add("EndBefore", EndBefore.Value);
                }
                if (EndBeforeTime.HasValue)
                {
                    dataMap.Add("EndBeforeTime", EndBeforeTime.Value);
                }
                return dataMap;
            }
        }
    }
}
