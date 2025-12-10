using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class GetAllProductInput : IRequest<ApiResponse>
    {
    }
    public class GetAllProductHandler
     : WebAdminHandlersBase<GetAllProductHandler>,
       IRequestHandler<GetAllProductInput, ApiResponse>
    {
        private IProductService ProductService => GetService<IProductService>();

        public GetAllProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApiResponse> Handle(GetAllProductInput request, CancellationToken cancellationToken)
        {
            return await ProductService.GetAllProduct();

        }
    }

}
