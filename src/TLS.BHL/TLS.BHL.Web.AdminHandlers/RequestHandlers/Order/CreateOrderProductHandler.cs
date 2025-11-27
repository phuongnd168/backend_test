using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Services;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Order
{
    public class CreateOrderProductHandler : WebAdminHandlersBase<CreateOrderProductHandler>, IRequestHandler<CreateOrderProductInput, CreateOrderProductOutput>
    {
        private IOrderService OrderService => GetService<IOrderService>();
        public CreateOrderProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<CreateOrderProductOutput> Handle(CreateOrderProductInput request, CancellationToken cancellationToken)
        {
            var order = Mapper.Map<OrderEntity>(request.createOrder);

            var res = await OrderService.CreateOrderProduct(order, cancellationToken);

            return new CreateOrderProductOutput
            {
                Data = res
            };
        }
    }
    public class CreateOrderProductInput : IRequest<CreateOrderProductOutput>
    {
        public CreateOrderProductDTO createOrder { get; set; }

    }
    public class CreateOrderProductOutput
    {
        public string Data { get; set; }
    }
    
}
