using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;
using TLS.Core.Data;


namespace TLS.BHL.Infra.Data.SQL.Repositories
{
    public class ProductRepository : BHLRepositoryBase<ProductRepository>, IProductRepository
    {
        private IBHLDbContext Context;
        public ProductRepository(IServiceProvider serviceProvider, IBHLDbContext context) : base(serviceProvider)
        {
            Context = context;
        }

        public async Task<IEnumerable<ProductEntity>> GetListProduct()
        {
           
            return await Context.Products
            .Include(p => p.Categories)
            .ToListAsync();
        }
        public async Task<string> UpdateProductCount(IList<UpdateProductDTO> products, CancellationToken cancellationToken)
        {

            foreach (var product in products)
            {
                var result = await Context.Products.FindAsync(product.Id);
               
                if (result == null)
                {
                    return "Không tồn tại sản phẩm";
                }
                if(result.Quantity == 0)
                {
                    return "Không thể cập nhật vì số lượng sản phẩm là 0";
                }
                if (result.Quantity < product.Count)
                {
                    return "Không thể cập nhật vì số lượng sản phẩm vượt quá số lượng thực tế";
                }
                
                result.Quantity -=product.Count;
            }



            await Context.SaveChangesAsync(cancellationToken);

            return "Update thành công";
        }

      
    }
}
