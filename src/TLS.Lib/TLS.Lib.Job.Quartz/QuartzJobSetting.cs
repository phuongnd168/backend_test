using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Quartz
{
    public class QuartzJobSetting
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public bool? Disable { get; set; }
        public bool? Durable { get; set; }
        public bool? Recovery { get; set; }
        public JobDataMap JobData { get; set; }
        public List<QuartzJobSettingTrigger> Triggers { get; set; }
    }
}
