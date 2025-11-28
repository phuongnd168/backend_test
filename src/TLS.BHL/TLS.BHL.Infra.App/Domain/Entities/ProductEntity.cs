using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("Products")]
    public class ProductEntity : BaseEntity
    {
     
        public string? NameVi { get; set; }
        public string? NameEn { get; set; }
        public string? Img { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
   
        public ICollection<ProductCategoryEntity> ProductCategorys  { get; set; } = new List<ProductCategoryEntity>();

    }

}
