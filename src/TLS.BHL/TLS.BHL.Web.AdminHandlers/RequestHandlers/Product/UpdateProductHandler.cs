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
    public class UpdateProductHandler : WebAdminHandlersBase<UpdateProductHandler>, IRequestHandler<UpdateProductInput, UpdateProductOutput>
    {
        private IProductService ProductService => GetService<IProductService>();
        public UpdateProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<UpdateProductOutput> Handle(UpdateProductInput request, CancellationToken cancellationToken)
        {


            var res = await ProductService.UpdateProduct(request.ProductId, request.UpdateProduct, cancellationToken);

            return new UpdateProductOutput
            {
                Data = res
            };
        }
    }
    public class UpdateProductInput() : IRequest<UpdateProductOutput>
    {
        public int ProductId { get; set; } 
        public UpdateProductDTO UpdateProduct { get; set; }

    }
    public class UpdateProductOutput
    {
        public string Data { get; set; }
    }
}
