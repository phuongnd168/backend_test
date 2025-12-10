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
using TLS.Core.Service;

namespace TLS.BHL.Infra.App.Services
{
    public interface IProductService : IService
    {
        Task<ApiResponse> GetAllProduct();
        Task<ApiResponse> GetListProduct(string keyword, string sortField, string sortDirection, int page, int pageSize);
        Task<ApiResponse> UpdateProductCount(IList<UpdateProductCountDTO> products, CancellationToken cancellationToken);
        Task<ApiResponse> DeleteProduct(int id, CancellationToken cancellationToken);
        Task<ApiResponse> CreateProduct(CreateProductDTO product, CancellationToken cancellationToken);
        Task<ApiResponse> UpdateProduct(int productId, UpdateProductDTO product, CancellationToken cancellationToken);
        Task<ApiResponse> GetProductById(int id);
       
    }





}
