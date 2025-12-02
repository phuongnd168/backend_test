using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Services;
using static TLS.BHL.Web.AdminHandlers.RequestHandlers.Product.GetProductByIdHandler;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Product
{
    public record GetProductByIdInput(int id) : IRequest<GetProductByIdOutput>
    {
    }
    public class GetProductByIdHandler
     : WebAdminHandlersBase<GetProductByIdHandler>,
       IRequestHandler<GetProductByIdInput, GetProductByIdOutput>
    {
        private IProductService ProductService => GetService<IProductService>();

        public GetProductByIdHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<GetProductByIdOutput> Handle(GetProductByIdInput request, CancellationToken cancellationToken)
        {
            var result = await ProductService.GetProductById(request.id, cancellationToken);
            return new GetProductByIdOutput
            {
                Data = result == null ? "Không tồn tại sản phẩm":result
            };
        }

        public class GetProductByIdOutput
        {
            public object Data { get; set; }
        }
    }

}
