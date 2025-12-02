using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Core.Service;

namespace TLS.BHL.Infra.App.Services
{
    public interface IProductService : IService
    { 
        Task<List<GetListProductItemDto>> GetListProduct();
        Task<string> UpdateProductCount(IList<UpdateProductCountDTO> products, CancellationToken cancellationToken);
        Task<string> DeleteProduct(int id, CancellationToken cancellationToken);
        Task<string> CreateProduct(CreateProductDTO product, CancellationToken cancellationToken);
        Task<string> UpdateProduct(int productId, UpdateProductDTO product, CancellationToken cancellationToken);
        Task<GetListProductItemDto> GetProductById(int id, CancellationToken cancellationToken);
    }





}
