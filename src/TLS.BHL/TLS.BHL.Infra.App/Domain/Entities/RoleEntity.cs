using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("Roles")]
    public class RoleEntity : BaseEntity
    {
        public string RoleName { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public IList<UserRoleEntity> RoleUsers { get; set; }
    }
}
