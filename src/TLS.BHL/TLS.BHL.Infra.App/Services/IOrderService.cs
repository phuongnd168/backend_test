using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Core.Service;

namespace TLS.BHL.Infra.App.Services
{
    public interface IOrderService : IService
    {
        Task<IEnumerable<OrderEntity>> GetOrderProduct();
        Task<string> CreateOrderProduct(OrderEntity request, CancellationToken cancellationToken);

    }
}
