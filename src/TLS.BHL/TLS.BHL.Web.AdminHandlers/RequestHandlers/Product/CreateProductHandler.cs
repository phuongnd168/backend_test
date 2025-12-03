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

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class CreateProductHandler : WebAdminHandlersBase<CreateProductHandler>, IRequestHandler<CreateProductInput, ApiResponse>
    {
        private IProductService ProductService => GetService<IProductService>();
        public CreateProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(CreateProductInput request, CancellationToken cancellationToken)
        {
            

            return await ProductService.CreateProduct(request.createProduct, cancellationToken);

            
        }
    }
    public class CreateProductInput : IRequest<ApiResponse>
    {
        public CreateProductDTO createProduct { get; set; }

    }
}
