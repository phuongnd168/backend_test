using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    public class ProductCategoryEntity
    {
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;
    }
}
