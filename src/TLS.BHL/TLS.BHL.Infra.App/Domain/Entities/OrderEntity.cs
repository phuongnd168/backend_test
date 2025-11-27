using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("Orders")]
    public class OrderEntity : BaseEntity
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public required string Products { get; set; }
     
        public int UserId { get; set; }  // Foreign key
        public UserEntity User { get; set; }

        public int statusId { get; set; }  // Foreign key
        public StatusEntity Status { get; set; }
    }
}
