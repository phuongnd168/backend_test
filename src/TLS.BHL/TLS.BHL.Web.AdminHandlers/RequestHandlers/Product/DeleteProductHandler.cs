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
    public class DeleteProductHandler : WebAdminHandlersBase<DeleteProductHandler>, IRequestHandler<DeleteProductInput, ApiResponse>
    {
        private IProductService ProductService => GetService<IProductService>();
        public DeleteProductHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(DeleteProductInput request, CancellationToken cancellationToken)
        {

            return await ProductService.DeleteProduct(request.id, cancellationToken);

        }
    }

}
public record DeleteProductInput(int id) : IRequest<ApiResponse>;

