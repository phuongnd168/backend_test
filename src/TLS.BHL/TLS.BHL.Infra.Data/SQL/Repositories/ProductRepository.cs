using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
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

        public async Task<ApiResponse> DeleteProduct(int id, CancellationToken cancellationToken)
        {

            try {
                var product = await Context.Products.FindAsync(id);
                if (product == null)
                {
                    return ResponseHelper.Error(404, "Không tìm thấy sản phẩm");
                }
                product.Deleted = true;
                await Context.SaveChangesAsync(cancellationToken);
                return ResponseHelper.Success("Xóa sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }

        }

        public async Task<ApiResponse> CreateProduct(CreateProductDTO product, CancellationToken cancellationToken)
        {
            using var transaction = await (Context as DbContext).Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var productE = new ProductEntity
            {
                NameVi = product.NameVi,
                NameEn = product.NameEn,
                Img = product.Img,
                Quantity = product.Quantity ?? 0,
                Price = product.Price ?? 0,
                Created_at = DateTime.Now
            };
            await Context.Products.AddAsync(productE);
            await Context.SaveChangesAsync(cancellationToken);
            foreach (var category in product.ListCategory)
            {
                var cate = await Context.Categories.FindAsync(category);
                if(cate == null)
                {
                    return ResponseHelper.Error(404, "Không tìm thấy danh mục");
                }
                await Context.ProductCategories.AddAsync(new ProductCategoryEntity{
                    CategoriesId = category,
                    ProductsId = productE.Id
                });
            }

            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);


                return ResponseHelper.Created("Thêm sản phẩm thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ResponseHelper.Error(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetListProduct()
        {
            try {
                var query =
                    from o in Context.Products
                    where !o.Deleted
                    join pc in Context.ProductCategories on o.Id equals pc.ProductsId into pcJoin
                    from pc in pcJoin.DefaultIfEmpty()
                    join ct in Context.Categories on pc.CategoriesId equals ct.Id into ctJoin
                    from ct in ctJoin.DefaultIfEmpty()
                    group ct by o into g
                    select new GetListProductItemDTO
                    {
                        Id = g.Key.Id,
                        NameVi = g.Key.NameVi,
                        NameEn = g.Key.NameEn,
                        Img = g.Key.Img,
                        Price = g.Key.Price,
                        Quantity = g.Key.Quantity,
                        CategoryId = g.Where(x => x != null).Select(x => x.Id).ToList()
                    };
                var products = await query.ToListAsync();
                return ResponseHelper.Success("Thành công", products);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }

         

        }
        public async Task<ApiResponse> UpdateProductCount(IList<UpdateProductCountDTO> products, CancellationToken cancellationToken)
        {
            try {
                foreach (var product in products)
                {
                    var result = await Context.Products.FindAsync(product.Id);

                    if (result == null)
                    {
                        return ResponseHelper.Error(404, "Không tìm thấy sản phẩm");
                    }
                    if (result.Quantity == 0)
                    {
                        return ResponseHelper.Error(404, "Không thể cập nhật vì số lượng sản phẩm đã hết");
                    }
                    if (result.Quantity < product.Count)
                    {
                        return ResponseHelper.Error(404, "Không thể cập nhật vì số lượng sản phẩm vượt quá số lượng thực tế");
                    }

                    result.Quantity -= product.Count??0;
                }



                await Context.SaveChangesAsync(cancellationToken);

                return ResponseHelper.Updated("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }


        }

        public async Task<ApiResponse> UpdateProduct(int productId, UpdateProductDTO product, CancellationToken cancellationToken)
        {
            using var transaction = await (Context as DbContext).Database.BeginTransactionAsync(cancellationToken);
            try
            {
            var result = await Context.Products.FindAsync(productId);
            if(result == null)
            {
                  return ResponseHelper.Error(404, "Không tìm thấy sản phẩm");
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
                    return ResponseHelper.Error(404, "Không tìm thấy danh mục");
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
            result.Quantity = product.Quantity??0;
            result.Price = product.Price??0;
            result.Updated_at = DateTime.Now;


            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ResponseHelper.Updated("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ResponseHelper.Error(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetProductById(int id, CancellationToken cancellationToken)
        {

            try
            {
                var query =
                    from o in Context.Products
                    where !o.Deleted && o.Id==id
                    join pc in Context.ProductCategories on o.Id equals pc.ProductsId into pcJoin
                    from pc in pcJoin.DefaultIfEmpty()
                    join ct in Context.Categories on pc.CategoriesId equals ct.Id into ctJoin
                    from ct in ctJoin.DefaultIfEmpty()
                    group ct by o into g
                    select new GetListProductItemDTO
                    {
                        Id = g.Key.Id,
                        NameVi = g.Key.NameVi,
                        NameEn = g.Key.NameEn,
                        Img = g.Key.Img,
                        Price = g.Key.Price,
                        Quantity = g.Key.Quantity,
                        CategoryId = g.Where(x => x != null).Select(x => x.Id).ToList()
                    };
                var product = await query.FirstOrDefaultAsync();
                if(product == null)
                {
                    return ResponseHelper.Error(404, "Không tìm thấy sản phẩm");
                }
                return ResponseHelper.Success("Thành công", product);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }
        }
    }
}
