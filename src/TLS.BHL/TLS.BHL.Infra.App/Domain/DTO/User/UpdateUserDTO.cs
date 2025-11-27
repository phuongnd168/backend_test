using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.User
{
    public class UpdateUserDTO
    {
        public int? Id { get; set; }

        public string UserId { get; set; }
        public required string FullName { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Avatar { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public string Updated_by { get; set; }
    }
}
