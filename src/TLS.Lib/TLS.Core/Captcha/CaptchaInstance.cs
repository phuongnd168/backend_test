using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Captcha
{
    public class CaptchaInstance
    {
        //private const string DefaultLang = "vi";
        //private static IDictionary<string, IEnumerable<string>> _dicText = new Dictionary<string, IEnumerable<string>>(
        //    new KeyValuePair<string, IEnumerable<string>>[]{
        //        new KeyValuePair<string, IEnumerable<string>>("vi", new string[] {
        //        "Nhập ký tự ở vị trí thứ {0}",
        //        "Nhập ký tự ở vị trí thứ {0}"
        //        })
        //    }
        //);
        
        //public string Text { get; set; }
        public string InstanceId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int Position { get; set; }
        public string FullCode { get; set; }

        [JsonIgnore]
        public MemoryStream Image { get; set; }

        [JsonIgnore]
        public string InstanceToken { get; set; }

        [JsonIgnore]
        public string ImageBase64
        {
            get
            {
                if (Image != null)
                {
                    return Convert.ToBase64String(Image.ToArray());
                }
                return null;
            }
        }

        [JsonIgnore]
        public string RealCode
        {
            get
            {
                if (!string.IsNullOrEmpty(FullCode) && FullCode.Length >= Position)
                {
                    return FullCode[Position - 1].ToString();
                }
                return null;
            }
        }

        //public CaptchaInstance(string instanceId, DateTime expiredDate, string codeValue, int position, MemoryStream image)
        //{
        //    ExpiredDate = expiredDate;
        //    InstanceId = instanceId;
        //    CodeValue = codeValue;
        //    Image = image;
        //    Position = position;
        //}
    }
}
