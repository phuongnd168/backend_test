using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Product
{
    public class CreateProductDTO
    {
        public string NameVi { get; set; }
        public string NameEn { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public List<int> ListCategory { get; set; }
    }
}
