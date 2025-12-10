using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Service;

namespace TLS.BHL.Infra.Business.Services
{
    public class ProductServie : ServiceBase, IProductService
    {
        private IProductRepository ProductRepo;
        public ProductServie(IServiceProvider serviceProvider, IProductRepository productRepo) : base(serviceProvider)
        {
            ProductRepo = productRepo;
        }

        public async Task<ApiResponse> CreateProduct(CreateProductDTO product, CancellationToken cancellationToken)
        {
            return await ProductRepo.CreateProduct(product, cancellationToken);
        }

        public async Task<ApiResponse> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            return await ProductRepo.DeleteProduct(id, cancellationToken);
        }

        public async Task<ApiResponse> GetAllProduct()
        {
            return await ProductRepo.GetAllProduct();
        }

        public async Task<ApiResponse> GetListProduct(string keyword, string sortField, string sortDirection, int page, int pageSize)
        {
            return await ProductRepo.GetListProduct(keyword, sortField, sortDirection, page, pageSize);
        }

        public async Task<ApiResponse> GetProductById(int id)
        {
            return await ProductRepo.GetProductById(id);
        }


        public async Task<ApiResponse> UpdateProduct(int productId, UpdateProductDTO product, CancellationToken cancellationToken)
        {
            return await ProductRepo.UpdateProduct(productId, product, cancellationToken);
        }

        public async Task<ApiResponse> UpdateProductCount(IList<UpdateProductCountDTO> products, CancellationToken cancellationToken)
        {
            
       
                return await ProductRepo.UpdateProductCount(products, cancellationToken);

              
      
            

            
        }

       
    }
}

