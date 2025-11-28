using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Services;


namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class GetListProductInput : IRequest<List<GetListProductItemDto>>
    {
    }
    public class GetListProductHandler
     : WebAdminHandlersBase<GetListProductHandler>,
       IRequestHandler<GetListProductInput, List<GetListProductItemDto>>
    {
        private IProductService ProductService => GetService<IProductService>();

        public GetListProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<GetListProductItemDto>> Handle(GetListProductInput request, CancellationToken cancellationToken)
        {
            return await ProductService.GetListProduct();
          
        }
    }



}



