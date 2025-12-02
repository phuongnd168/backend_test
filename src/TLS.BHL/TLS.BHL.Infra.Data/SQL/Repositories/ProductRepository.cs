using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
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

        public async Task<string> DeleteProduct(int id, CancellationToken cancellationToken)
        {
           

            var product = await Context.Products.FindAsync(id);
            if(product == null)
            {
                return "Không tồn tại sản phẩm";
            }
            product.Deleted = true;
            await Context.SaveChangesAsync(cancellationToken);
            return "Xóa sản phẩm thành công";
        }

        public async Task<string> CreateProduct(CreateProductDTO product, CancellationToken cancellationToken)
        {
            using var transaction = await (Context as DbContext).Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var productE = new ProductEntity
            {
                NameVi = product.NameVi,
                NameEn = product.NameEn,
                Img = product.Img,
                Quantity = product.Quantity,
                Price = product.Price,
                Created_at = DateTime.Now
            };
            await Context.Products.AddAsync(productE);
            await Context.SaveChangesAsync(cancellationToken);
            foreach (var category in product.ListCategory)
            {
                var cate = await Context.Categories.FindAsync(category);
                if(cate == null)
                {
                    return "Không tồn tại danh mục";
                }
                await Context.ProductCategories.AddAsync(new ProductCategoryEntity{
                    CategoriesId = category,
                    ProductsId = productE.Id
                });
            }

            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);


                return "Thành công";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<List<GetListProductItemDto>> GetListProduct()
        {
            var query =
                       from o in Context.Products
                       where !o.Deleted
                       join c in Context.ProductCategories on o.Id equals c.ProductsId
                       join ct in Context.Categories on c.CategoriesId equals ct.Id
                       group ct by o into g
                       select new GetListProductItemDto
                        {
                            Id = g.Key.Id,
                            NameVi = g.Key.NameVi,
                            NameEn = g.Key.NameEn,
                            Img = g.Key.Img,
                            Price = g.Key.Price,
                            Quantity = g.Key.Quantity,
                            CategoryId = g.Select(x => x.Id).ToList()
                        };
            return await query.ToListAsync();

        }
        public async Task<string> UpdateProductCount(IList<UpdateProductCountDTO> products, CancellationToken cancellationToken)
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

        public async Task<string> UpdateProduct(int productId, UpdateProductDTO product, CancellationToken cancellationToken)
        {
            using var transaction = await (Context as DbContext).Database.BeginTransactionAsync(cancellationToken);
            try
            {
            var result = await Context.Products.FindAsync(productId);
            if(result == null)
            {
                return "Sản phẩm không tồn tại";
            }
            var productCategories = await Context.ProductCategories
                .Where(pc => pc.ProductsId == result.Id)
                .ToListAsync();
            foreach (var pc in productCategories) {
                 Context.ProductCategories.Remove(pc);
            }
            await Context.SaveChangesAsync(cancellationToken);

 
            foreach (var category in product.ListCategory)
            {
                var cate = await Context.Categories.FindAsync(category);
                if (cate == null)
                {
                    return "Không tồn tại danh mục";
                }
                await Context.ProductCategories.AddAsync(new ProductCategoryEntity
                {
                    CategoriesId = category,
                    ProductsId = result.Id
                });
            }

            result.NameVi = product.NameVi;
            result.NameEn = product.NameEn;
            result.Img = product.Img;
            result.Quantity = product.Quantity;
            result.Price = product.Price;
            result.Updated_at = DateTime.Now;


            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return "Thành công";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<GetListProductItemDto> GetProductById(int id, CancellationToken cancellationToken)
        {

            var query = 
                       from o in Context.Products
                       where !o.Deleted && o.Id == id
                       join c in Context.ProductCategories on o.Id equals c.ProductsId
                       join ct in Context.Categories on c.CategoriesId equals ct.Id
                       group ct by o into g
                       select new GetListProductItemDto
                       {
                           Id = g.Key.Id,
                           NameVi = g.Key.NameVi,
                           NameEn = g.Key.NameEn,
                           Img = g.Key.Img,
                           Price = g.Key.Price,
                           Quantity = g.Key.Quantity,
                           CategoryId = g.Select(x => x.Id).ToList()
                       };
            var product = await query.FirstOrDefaultAsync();
            if (product == null)
            {
                return null;
            }
            return product;
        }
    }
}
