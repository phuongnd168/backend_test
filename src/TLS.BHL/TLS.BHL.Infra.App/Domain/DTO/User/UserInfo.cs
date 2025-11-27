using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.User
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
