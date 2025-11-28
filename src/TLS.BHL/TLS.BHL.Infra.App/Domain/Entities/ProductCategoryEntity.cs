using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    public class ProductCategoryEntity : BaseEntity
    {

        public int ProductsId { get; set; }
        public ProductEntity? Product { get; set; }

        public int CategoriesId { get; set; }
        public CategoryEntity? Category { get; set; }
    }
}
