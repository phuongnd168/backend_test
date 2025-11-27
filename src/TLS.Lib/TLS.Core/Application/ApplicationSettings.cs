using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Application
{
    public class ApplicationSettings
    {
        public IHostEnvironment Environment { get; set; }
        public string Application { get; set; }
        public string ProjectName { get; set; }
        public string ProjectVersion { get; set; }
        public string TargetVersion { get; set; }
    }
}
