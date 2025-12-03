using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Order
{
    public class CreateOrderProductHandler : WebAdminHandlersBase<CreateOrderProductHandler>, IRequestHandler<CreateOrderProductInput, ApiResponse>
    {
        private IOrderService OrderService => GetService<IOrderService>();
        public CreateOrderProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(CreateOrderProductInput request, CancellationToken cancellationToken)
        {
        

            return await OrderService.CreateOrderProduct(request.createOrder, cancellationToken);
        }
    }
    public class CreateOrderProductInput : IRequest<ApiResponse>
    {
        public CreateOrderProductDTO createOrder { get; set; }

    }
  
    
}
