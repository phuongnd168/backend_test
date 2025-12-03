using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Product
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm tiếng Việt không được để trống")]
        public string NameVi { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm tiếng Anh không được để trống")]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh không được để trống")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Số lượng sản phẩm không được để trống")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm không được để trống")]
        public List<int>? ListCategory { get; set; }
    }
}
