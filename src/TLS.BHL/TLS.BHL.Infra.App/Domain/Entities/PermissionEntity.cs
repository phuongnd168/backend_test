using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("Permissions")]
    public class PermissionEntity : BaseEntity
    {
        public string Name {  get; set; }

        public string Description { get; set; }
    }
}
