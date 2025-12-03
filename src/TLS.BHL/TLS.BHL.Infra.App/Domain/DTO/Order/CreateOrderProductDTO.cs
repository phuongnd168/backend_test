using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Order
{
    public class CreateOrderProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Products { get; set; }


        [Required(ErrorMessage = "Id người dùng không được để trống")]
        public int? UserId { get; set; }
    }
}
