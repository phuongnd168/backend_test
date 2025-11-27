using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Core.Data;

namespace TLS.BHL.Infra.App.Repositories
{
    public interface IProductRepository : IRepository
    {
        Task<IEnumerable<ProductEntity>> GetListProduct();
        Task<string> UpdateProductCount(IList<UpdateProductDTO> products, CancellationToken cancellationToken);
    }
}
