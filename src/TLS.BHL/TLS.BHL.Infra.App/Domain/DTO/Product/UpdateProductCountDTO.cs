using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Product
{
    public class UpdateProductCountDTO
    {
        [Required(ErrorMessage = "Id sản phẩm không được để trống")]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public int? Count { get; set; }

    }
}
