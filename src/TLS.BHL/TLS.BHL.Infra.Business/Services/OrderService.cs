using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Service;

namespace TLS.BHL.Infra.Business.Services
{
    public class OrderService : ServiceBase, IOrderService
    {
        
        private IOrderRepository OrderRepo;
        public OrderService(IServiceProvider serviceProvider, IOrderRepository orderRepo) : base(serviceProvider)
        {
            OrderRepo = orderRepo;
        }

        public async Task<ApiResponse> CreateOrderProduct(CreateOrderProductDTO request, CancellationToken cancellationToken)
        {
            return await OrderRepo.CreateOrderProduct(request, cancellationToken);
        }

        public async Task<ApiResponse> GetOrderProduct()
        {
            return await OrderRepo.GetOrderProduct();
        }
    }
}
