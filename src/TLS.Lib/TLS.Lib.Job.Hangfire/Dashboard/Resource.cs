using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Hangfire.Dashboard
{
    public static class Resource
    {
        public static string ResizeMinJs
        {
            get
            {
                return string.Format("{0}.Resources.resize.min.js", typeof(Resource).Namespace);
            }
        }
        public static string ScriptJs
        {
            get
            {
                return string.Format("{0}.Resources.script.js", typeof(Resource).Namespace);
            }
        }
        public static string StyleCss
        {
            get
            {
                return string.Format("{0}.Resources.style.css", typeof(Resource).Namespace);
            }
        }
    }
}
