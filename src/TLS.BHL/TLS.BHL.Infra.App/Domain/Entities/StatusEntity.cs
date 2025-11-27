using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("Status")]
    public class StatusEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();


    }
}
