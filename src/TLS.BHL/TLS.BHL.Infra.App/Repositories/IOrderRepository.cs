using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Core.Data;

namespace TLS.BHL.Infra.App.Repositories
{
    public interface IOrderRepository :IRepository
    {
        Task<IEnumerable<OrderEntity>> GetOrderProduct();
        Task<string> CreateOrderProduct(OrderEntity request, CancellationToken cancellationToken);
    }
}
