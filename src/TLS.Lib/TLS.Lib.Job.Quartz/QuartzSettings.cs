using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Quartz
{
    public class QuartzSettings
    {
        public IList<QuartzJobSetting> Jobs { get; set; }
        public QuartzSettings()
        {
            Jobs = new List<QuartzJobSetting>();
        }
    }
}
