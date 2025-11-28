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

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public class CretaProductHandler : WebAdminHandlersBase<CretaProductHandler>, IRequestHandler<CreateProductInput, CreateProductOutput>
    {
        private IProductService ProductService => GetService<IProductService>();
        public CretaProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<CreateProductOutput> Handle(CreateProductInput request, CancellationToken cancellationToken)
        {
            

            var res = await ProductService.CreateProduct(request.createProduct, cancellationToken);

            return new CreateProductOutput
            {
                Data = res
            };
        }
    }
    public class CreateProductInput : IRequest<CreateProductOutput>
    {
        public CreateProductDTO createProduct { get; set; }

    }
    public class CreateProductOutput
    {
        public string Data { get; set; }
    }
}
