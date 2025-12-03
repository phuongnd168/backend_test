using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Order
{
    public class GetOrderProductHandler : WebAdminHandlersBase<GetOrderProductHandler>, IRequestHandler<GetListOrderInput, ApiResponse>
    {
        
        private IOrderService OrderService => GetService<IOrderService>();
        public GetOrderProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApiResponse> Handle(GetListOrderInput input, CancellationToken cancellationToken)
        {

            return await OrderService.GetOrderProduct();
        }
    }
    public class GetListOrderInput : IRequest<ApiResponse>
    {
      
    }
   
  
}


