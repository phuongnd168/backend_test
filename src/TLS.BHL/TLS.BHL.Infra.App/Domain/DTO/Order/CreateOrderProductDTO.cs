using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Order
{
    public class CreateOrderProductDTO
    {

        public string Products { get; set; }
        public int UserId { get; set; }
    }
}
