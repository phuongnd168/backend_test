using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.Core.Data;

namespace TLS.BHL.Infra.App.Repositories
{
    public interface IOrderRepository :IRepository
    {
        Task<ApiResponse> GetOrderProduct();
        Task<ApiResponse> CreateOrderProduct(CreateOrderProductDTO request, CancellationToken cancellationToken);
    }
}
