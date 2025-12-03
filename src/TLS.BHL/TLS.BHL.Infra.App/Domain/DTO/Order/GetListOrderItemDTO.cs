using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Order
{
    public class GetListOrderItemDTO
    {
        public string Product { get; set; }
        public string StatusName { get; set; }
        public string OrderId { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
