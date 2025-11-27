using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Order
{
    public class GetOrderProductHandler : WebAdminHandlersBase<GetOrderProductHandler>, IRequestHandler<GetListOrderInput, GetListOrderOutput>
    {
        
        private IOrderService OrderService => GetService<IOrderService>();
        public GetOrderProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<GetListOrderOutput> Handle(GetListOrderInput input, CancellationToken cancellationToken)
        {
            var orders = await OrderService.GetOrderProduct();

            return new GetListOrderOutput
            {
                Data = Mapper.Map<IEnumerable<GetListOrderItem>>(orders)
            };
        }
    }
    public class GetListOrderInput : IRequest<GetListOrderOutput>
    {
      
    }
    public class GetListOrderOutput
    {
        public IEnumerable<GetListOrderItem>? Data { get; set; }
    }
    public class GetListOrderItem
    {
        public string Product { get; set; }
        public string StatusName { get; set; }
        public string OrderId { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}


