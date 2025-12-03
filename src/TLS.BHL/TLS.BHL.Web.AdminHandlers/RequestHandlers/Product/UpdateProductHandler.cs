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
    public class UpdateProductHandler : WebAdminHandlersBase<UpdateProductHandler>, IRequestHandler<UpdateProductInput, ApiResponse>
    {
        private IProductService ProductService => GetService<IProductService>();
        public UpdateProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(UpdateProductInput request, CancellationToken cancellationToken)
        {

            return await ProductService.UpdateProduct(request.ProductId, request.UpdateProduct, cancellationToken);

        }
    }
    public class UpdateProductInput() : IRequest<ApiResponse>
    {
        public int ProductId { get; set; } 
        public UpdateProductDTO UpdateProduct { get; set; }

    }

}
