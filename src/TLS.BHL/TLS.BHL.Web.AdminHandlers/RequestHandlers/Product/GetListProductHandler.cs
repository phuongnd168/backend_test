using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;


namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class GetListProductInput : IRequest<ApiResponse>
    {
        public string? sortField { get; set; }
        public string? sortDirection { get; set; }
        public string? keyword { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
    public class GetListProductHandler
     : WebAdminHandlersBase<GetListProductHandler>,
       IRequestHandler<GetListProductInput, ApiResponse>
    {
        private IProductService ProductService => GetService<IProductService>();

        public GetListProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ApiResponse> Handle(GetListProductInput request, CancellationToken cancellationToken)
        {
            return await ProductService.GetListProduct(request.keyword, request.sortField, request.sortDirection, request.page, request.pageSize);
          
        }
    }



}



